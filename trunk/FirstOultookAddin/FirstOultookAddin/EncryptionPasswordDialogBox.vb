
Imports Org.BouncyCastle.Cms

Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Security.Cryptography
Imports FirstOultookAddin.PBKDF2_PKCS5


Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop.Outlook
Imports System.Text.RegularExpressions


Public Class EncryptionPasswordDialogBox

    'Global Variables
    Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveInspector.CurrentItem, Outlook.MailItem)
    Dim originalMessage As String = currentItem.Body.ToString
    Dim originalMsgID As String = currentItem.EntryID
    Dim originalMsgDate As Date = currentItem.TaskCompletedDate
    Dim originalMsgTo As String = currentItem.To
    Dim originalMsgSubject As String = currentItem.Subject
    Dim originalMsgAttachments As Attachments = currentItem.Attachments
    Dim originalAttachmentsSource As String() = New String(currentItem.Attachments.Count - 1) {}

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendEncryptedMessageButton.Click


        Try
            'Validations
            If isPasswordValid() = False Then
                Exit Sub
            End If

            'Progress Bar and Label formatting
            encryptionStatusLabel.ForeColor = Drawing.Color.Maroon
            encryptionStatusLabel.Text = "Encrypting Message..."
            EncryptionProgressBar.Refresh()

            'Retrieve the Current MailMessage Details

            Dim messageBody As String
            Dim messageRecipients As String
            Dim messageCCRecipients As String
            Dim messageBCCRecipients As String
            Dim messageSubject As String
            Dim messageAttachments As Outlook.Attachments

            'Set messageBodyFormat variable according to the selected BodyFormat 
            Dim msgBodyFormat As OlBodyFormat = currentItem.BodyFormat
            If msgBodyFormat = OlBodyFormat.olFormatHTML Then
                messageBody = currentItem.HTMLBody                  'HTML formatted message
            ElseIf msgBodyFormat = OlBodyFormat.olFormatPlain Then
                messageBody = currentItem.Body                      'PlainText message
            Else
                messageBody = currentItem.HTMLBody 'For Rich-Text and Unspecified formats and any others that might come in future
            End If

            'Initialise Local variables
            messageRecipients = currentItem.To
            messageCCRecipients = currentItem.CC
            messageBCCRecipients = currentItem.BCC
            messageSubject = currentItem.Subject
            messageAttachments = currentItem.Attachments

            'Global Variables: Set value 
            originalMsgTo = messageRecipients
            originalMsgSubject = currentItem.Subject

            'Set progress bar to 25%
            EncryptionProgressBar.Value = 50
            EncryptionProgressBar.Refresh()
            encryptionStatusLabel.Text = "Message Data Retrieval Succesful"
            encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
                                                             EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

            encryptionStatusLabel.Refresh()
            'System.Threading.Thread.Sleep(500)

            'Adding Custom Message Headers
            Dim pa As Outlook.PropertyAccessor
            pa = currentItem.PropertyAccessor
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version", "ECube-1.0")
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Hint", HintTextBox.Text.ToString)
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-Encryption-Type", "Quick")
            'pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-Encryption-Status", "AES-Encrypted")
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/Content-Description", "S/MIME Encrypted Message")


            'Set progress bar to 50%
            EncryptionProgressBar.Value = 75
            EncryptionProgressBar.Refresh()
            encryptionStatusLabel.Text = "Message Headers Added"
            encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
                                                             EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

            encryptionStatusLabel.Refresh()
            System.Threading.Thread.Sleep(500)

            'Convert MessageBody (as String) to Byte Array using UTF-8 Encoding
            Dim messageBodyData() As Byte
            messageBodyData = System.Text.UTF8Encoding.UTF8.GetBytes(messageBody)    'Microsoft Outlook 2007 uses Wester European (UTF-8) Encoding Scheme to compose New Messages

            'hence the msg body text is converted to UTF-8 encoded bytes not ASCII.
            'A simple rule Read the text in the encoding it is present (eg: UTF-8 in Outlook) and Retrieve or Write the text in the same Encoding scheme to get the correct output.

            'Encrypt the messageBodyData
            Dim encodedData As Byte() = PasswordBasedEncryption(CmsEnvelopedDataGenerator.DesEde3Cbc, messageBodyData, PasswordTextBox.Text.ToString)

            'currentItem.Body = "<html><u><b>Instructions to read this Encrypted Email</b></u><br><br> 1) Open the message, enter the Secret-Phrase and click the """"Display Message"""" button to decrypt the message. <br><br><i> You may use the <b>Hint</b> to guess the Secret!</i></html>"
            'currentItem.Body = "Instructions to read this Encrypted Email * Open the message, enter the Secret-Phrase and click the ""Display Message"" button to decrypt the message. <br><br>* While closing the message Reading pane, you can decide whether to <br><ul><li> save the message in <i>decrypted </i>form (plain text) by choosing <i>""Yes"" </i></li>or<li> save the message in <i>encrypted</i> form by choosing <i>""No""</i>,<br> when prompted by Outlook. </li><br></ul><i> You may use the Hint to guess the Secret!</i>"
            currentItem.Body = RichTextBox1.Text

            'before encodedData

            'Encrypt the attachment(s) if any
            Dim encryptedAttachmentsSource As String() = New String(currentItem.Attachments.Count - 1) {}

            If messageAttachments.Count > 0 Then
                encryptedAttachmentsSource = EncryptAttachments(messageAttachments, CmsEnvelopedDataGenerator.DesEde3Cbc, PasswordTextBox.Text.ToString)
                'Delete all unencrypted attachments
                Dim k As Integer = currentItem.Attachments.Count
                For j As Integer = 1 To k
                    currentItem.Attachments(1).Delete()
                Next
            End If

            'Add encoded message as an S/MIME attachment

            'Creating a smime file on users local hard disk
            Dim smimeSource As String = System.IO.Path.GetTempPath & "smime.p7m"
            Dim fs As New FileStream(smimeSource, FileMode.Create, FileAccess.Write)
            fs.Write(encodedData, 0, encodedData.Length)
            fs.Close()
            'Attach the S/MIME file (containing message body) to the current message first
            ' TODO: Replace with attachment name
            Dim smimeDisplayName As String = "smime.p7m"

            Dim sBodyLen As String = currentItem.Body.Length
            Dim oAttachs As Outlook.Attachments = currentItem.Attachments
            Dim oAttach As Outlook.Attachment
            oAttach = oAttachs.Add(smimeSource, , sBodyLen + 1, smimeDisplayName)

            'Now Attach the encrypted attachment(s)
            If messageAttachments.Count > 0 Then
                For Each attachmentSource As String In encryptedAttachmentsSource

                    'Dim attachLen As String = currentItem.Body.Length
                    Dim msgAttachs As Outlook.Attachments = currentItem.Attachments
                    Dim msgAttach As Outlook.Attachment
                    msgAttach = msgAttachs.Add(attachmentSource.ToString)
                Next
            End If

            'Set progress bar to 75%
            EncryptionProgressBar.Value = 100
            EncryptionProgressBar.Refresh()
            encryptionStatusLabel.ForeColor = Drawing.Color.Green
            encryptionStatusLabel.Font = New System.Drawing.Font("Arial", 9, Drawing.FontStyle.Bold)
            encryptionStatusLabel.Text = "Message Successfully Encrypted!"
            encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
                                                             EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

            encryptionStatusLabel.Refresh()
            System.Threading.Thread.Sleep(500)

            'Send the current Password Encrypted Message
            currentItem.Send()

            '' Clean up
            'delete temp smime.p7m file as well
            If My.Computer.FileSystem.FileExists(smimeSource) Then
                My.Computer.FileSystem.DeleteFile(smimeSource, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
            End If
            'Delete all attachment stuff
            '(In Form_Closing() Event)

            currentItem = Nothing
            oAttach = Nothing
            oAttachs = Nothing
            messageBody = ""

            Me.TopMost = True
            Me.CenterToScreen()
            'Set progress bar to 100%
            EncryptionProgressBar.Value = EncryptionProgressBar.Maximum

            encryptionStatusLabel.Text = "Encryption Successfull! Sending Message.."
            encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
                                                             EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)
            EncryptionProgressBar.Refresh()
            encryptionStatusLabel.Refresh()
            'System.Threading.Thread.Sleep(2000)

            'Display the UserChoicePanel and hide the previous panel
            ButtonPanelInitial.Hide()
            ButtonPanelInitial.Enabled = False
            TopPanel.Enabled = False

            UserChoicePanel.Visible = True
            UserChoicePanel.Enabled = True

            UserChoicePanel.Show()
            UserChoicePanel.Focus()

            Me.CancelButton = doneButton


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Catch ex As System.Exception
            'MsgBox(ex.ToString)
            encryptionStatusLabel.ForeColor = Drawing.Color.Red
            encryptionStatusLabel.Text = "Failed to encrypt the message! Please try again."
            'MsgBox(ex.Message.ToString, MsgBoxStyle.Exclamation, "An Error Occured:")
            EncryptionProgressBar.Value = 0
            EncryptionProgressBar.Refresh()
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Try


            Me.Close()

            MyOutlookAddIn.currentEncryptionRibbon.quickEncryption.Enabled = True

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try
    End Sub

    Private Sub EncryptionPasswordDialogBox_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            'Cleanup
            'Set all global variables to nothing

            originalMessage = Nothing
            currentItem = Nothing
            originalMsgID = Nothing
            originalMsgDate = Nothing
            originalMsgTo = Nothing
            originalMsgSubject = Nothing

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try
    End Sub


    Public Function keyDerivation(ByRef Password As String, ByRef Salt As Byte(), ByRef iterationCounter As Int32) As Rfc2898DeriveBytes

        Dim key As New Rfc2898DeriveBytes(Password, Salt, iterationCounter)

        Return key

    End Function



    Public Sub keyEncryption(ByRef KEK As String, ByVal CEK As String, ByVal IV As Byte())

        'Dim fCEK As Integer = formatCEK(CEK)

        'encryptCEK1(fCEK, IV, KEK)

        'encryptCEK2(ct3, encryptCEK1(), KEK)

        Dim FinalEncryptedCEK As Integer = encryptCEK2()

        keyVerification(FinalEncryptedCEK)


    End Sub

    Function encryptCEK2() As Integer

        Return 0
    End Function

    Function keyVerification(ByRef FinalEncryptedCEK As Integer) As Boolean

        Return True
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ICTimeTest.Click

        Get_Accurate_ProcessTime()
        MsgBox("Total Time Taken for IC Test 10^3 = " & IC10e3 & vbCrLf _
               & "Total Time Taken for IC Test 10^4 = " & IC10e4 & vbCrLf _
               & "Total Time Taken for IC Test 10^5 = " & IC10e5 & vbCrLf _
               & "Total Time Taken for IC Test 10^6 = " & IC10e6)

    End Sub

    Public Sub ICTest(ByVal ICounter As Int32)

        Dim saltforKEK(8) As Byte
        Using rngCsp As New RNGCryptoServiceProvider()
            rngCsp.GetBytes(saltforKEK)                               'Get a random number of 8 bytes and puts it into salt
        End Using

        Dim derivedKey As Byte() = keyDerivation(Password:="HolaKaPita987654321@#-RandomPasswdasddada", iterationCounter:=ICounter, Salt:=saltforKEK).GetBytes(16)
    End Sub

    Dim IC10e3 As String = ""
    Dim IC10e4 As String = ""
    Dim IC10e5 As String = ""
    Dim IC10e6 As String = ""


    Sub Get_Accurate_ProcessTime()

        Dim oWatch As New Stopwatch

        oWatch.Start()
        ICTest(1000)    'Icounter valuea as 10^3
        oWatch.Stop()
        IC10e3 = oWatch.ElapsedMilliseconds.ToString


        oWatch.Restart()
        ICTest(10000)    'Icounter valuea as 10^4
        oWatch.Stop()
        IC10e4 = oWatch.ElapsedMilliseconds.ToString

        oWatch.Restart()
        ICTest(100000)    'Icounter valuea as 10^5
        oWatch.Stop()
        IC10e5 = oWatch.ElapsedMilliseconds.ToString

        oWatch.Restart()
        ICTest(1000000)    'Icounter valuea as 10^6
        oWatch.Stop()
        IC10e6 = oWatch.ElapsedMilliseconds.ToString

    End Sub


    Private Function PasswordBasedEncryption(ByVal CEKEncryptionAlgorithm As String, ByVal data As Byte(), ByVal PBEkey As String) As Byte()
        Try

            'Encryption Process
            Dim edGen As New CmsEnvelopedDataGenerator()

            edGen.AddPasswordRecipient(New Pkcs5Scheme2Utf8PbeKey(PBEkey.ToCharArray(), New Byte(19) {}, 10000), CEKEncryptionAlgorithm)

            Dim ed As CmsEnvelopedData = edGen.Generate(New CmsProcessableByteArray(data), CmsEnvelopedDataGenerator.Aes128Cbc) 'Encrypted msgbody data using AES-128CBC 

            Dim encodedData As Byte() = ed.GetEncoded 'Creates ASN.1 encoded data

            Return encodedData 'Returns the ASN.1 encoded data containing the CMSEnvelopedData object. Basically encrypted and encoded data.

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try
        '''''''''''''''''''''''''''''''''''''''Experimental Code created while Developing'''''''''''''''''''''''''''''''''''''''''''''

        ''Self attempted Byte() to String conversion and vice-versa instead of using base64 Encoding (You can also call this My Custom Encoding!)

        ''Encryption Side
        ''Byte() to String() conversion
        'Dim temp3 As String() = New String(encodedData.Length - 1) {}
        'Dim j As Integer = 0
        'For Each value In encodedData
        '    temp3(j) = value.ToString & "#"
        '    j = j + 1
        'Next

        ''String() to String conversion with Delimiter
        'Dim temp4 As String = ""
        'For Each value In temp3
        '    temp4 = temp4 & value.ToString
        'Next

        ' MsgBox("Temp4: " & encodedData)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Dim objOutlook1 As Outlook._Application
        'objOutlook1 = Globals.ThisAddIn.Application
        'Dim objNS1 As Outlook._NameSpace = objOutlook1.Session
        'Dim objFolder1 As Outlook.MAPIFolder
        'objFolder1 = objNS1.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)
        '' objFolder1.Items.Add(Outlook.OlItemType.olMailItem)
        ''objFolder1.Items.Add(CType(currentItem, Outlook.MailItem)) 'Save the Sent Mail in SentBox Folder of outlook


        'currentItem.Attachments.Item(1).
        ''Decryption Side
        ''String to String() conversion using Delimiter
        'Dim temp6 As String() = New String(1248) {}
        'temp6 = temp4.Split("#")
        'temp6.SetValue(Nothing, 1249)

        ''String() to Byte() retrieval conversion
        'Dim dataRetrieved As Byte() = New Byte(1249) {}
        'Dim k As Integer = 0
        'For Each value In temp6
        '    If k < 1249 Then
        '        dataRetrieved(k) = Byte.Parse(value)
        '        k = k + 1
        '    End If
        'Next

        ''Byte() content display code
        'Dim temp2 As String = ""
        'For Each value In dataRetrieved
        '    temp2 = temp2 & value
        'Next
        'MsgBox("Retrived Byte array: " & temp2)

        ''''''''''''''''''''''''''''''''''''''''''''''''

        'Decryption Process at Receiver's end

        'Dim recipients As RecipientInformationStore = ed.GetRecipientInfos()
        ''Assert.AreEqual(ed.EncryptionAlgOid, CmsEnvelopedDataGenerator.Aes128Cbc).

        'Dim c As ICollection = recipients.GetRecipients()

        ''Assert.AreEqual(1, c.Count)

        'For Each recipient As PasswordRecipientInformation In c

        '    'Dim key As CmsPbeKey = New Pkcs5Scheme2Utf8PbeKey("abc\u5639\u563b".ToCharArray(), recipient.KeyDerivationAlgorithm)
        '    Dim key As CmsPbeKey = New Pkcs5Scheme2Utf8PbeKey(PBEkey.ToCharArray(), recipient.KeyDerivationAlgorithm) ' 
        '    Dim recievedData As Byte() = recipient.GetContent(key)  ' 

        '    Dim abc As String = ""
        '    For Each temp In recData
        '        abc = abc & temp & " "
        '    Next
        '    MsgBox(abc.ToString)

        '    'Assert.IsTrue(Arrays.AreEqual(data, recData))
        'Next
        ''New Decryption Code
        'Dim recipientID As New RecipientID()
        'Dim decodedEnvelopeData As New CmsEnvelopedData(encodedData)
        'Dim recipient As RecipientInformation = decodedEnvelopeData.GetRecipientInfos().GetFirstRecipient(recipientID)

        'Dim data2 As Byte() = recipient.GetContent(key)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Function

    Private Sub EncryptionPasswordDialogBox_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            '' Clean up continued...
            'Delete all attachment stuff
            If My.Computer.FileSystem.DirectoryExists(System.IO.Path.GetTempPath & "EcubeEncryptedAttachments") Then
                My.Computer.FileSystem.DeleteDirectory(System.IO.Path.GetTempPath & "EcubeEncryptedAttachments", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            If My.Computer.FileSystem.DirectoryExists(System.IO.Path.GetTempPath & "EcubeOriginalAttachments") Then
                My.Computer.FileSystem.DeleteDirectory(System.IO.Path.GetTempPath & "EcubeOriginalAttachments", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try
    End Sub

    Private Sub EncryptionPasswordDialogBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.Icon = My.Resources.ecubeicon

        'Sizing
        Dim screenWidth As Integer = My.Computer.Screen.Bounds.Width
        Dim screenHeight As Integer = My.Computer.Screen.Bounds.Height

        Me.Size = New System.Drawing.Size((screenWidth * 0.48), (screenHeight * 0.38))
        Me.CenterToScreen()

        LogoPictureBox.Size = New Drawing.Size((Me.Size.Width * 0.22), (Me.Size.Height * 0.92))
        TopPanel.Size = New Drawing.Size((Me.Size.Width * 0.6), (Me.Size.Height * 0.38))
        EncryptionProgressBar.Size = New Drawing.Size(Me.Size.Width * 0.48, Me.Size.Height * 0.072)
        ButtonPanelInitial.Size = New Drawing.Size((Me.Size.Width - LogoPictureBox.Width), (Me.Size.Height * 0.31))
        UserChoicePanel.Size = New Drawing.Size((Me.Size.Width - LogoPictureBox.Width), (Me.Size.Height * 0.31))

        'Label Resizing
        Dim fontScaleFactor As Double = screenHeight / 1080

        IntroLabel.Font = New Drawing.Font(IntroLabel.Font.FontFamily, IntroLabel.Font.Size * (fontScaleFactor + 0.1))
        PasswordLabel.Font = New Drawing.Font(PasswordLabel.Font.FontFamily, PasswordLabel.Font.Size * fontScaleFactor)
        ConfirmPasswordLabel.Font = New Drawing.Font(ConfirmPasswordLabel.Font.FontFamily, ConfirmPasswordLabel.Font.Size * fontScaleFactor)
        HintLabel.Font = New Drawing.Font(HintLabel.Font.FontFamily, HintLabel.Font.Size * fontScaleFactor)
        EncryptionProgressLabel.Font = New Drawing.Font(EncryptionProgressLabel.Font.FontFamily, EncryptionProgressLabel.Font.Size * fontScaleFactor)
        EncryptionProgressLabel.Font = New Drawing.Font(EncryptionProgressLabel.Font.FontFamily, EncryptionProgressLabel.Font.Size, Drawing.FontStyle.Bold)

        NoteLabel.Font = New Drawing.Font(NoteLabel.Font.FontFamily, NoteLabel.Font.Size * fontScaleFactor)
        NoteLabel.Width = NoteLabel.Parent.Width * 0.55
        encryptionStatusLabel.Font = New Drawing.Font(encryptionStatusLabel.Font.FontFamily, encryptionStatusLabel.Font.Size * (fontScaleFactor + 0.1))
        
        PasswordTextBox.Size = New Drawing.Size(PasswordTextBox.Parent.Width * 0.36, PasswordTextBox.Parent.Height * 0.141)
        PasswordTextBox.Font = New Drawing.Font(PasswordTextBox.Font.FontFamily, PasswordTextBox.Font.Size * (fontScaleFactor + 0.1))
        ConfirmPasswordTextBox.Size = New Drawing.Size(ConfirmPasswordTextBox.Parent.Width * 0.36, ConfirmPasswordTextBox.Parent.Height * 0.141)
        ConfirmPasswordTextBox.Font = New Drawing.Font(ConfirmPasswordTextBox.Font.FontFamily, ConfirmPasswordTextBox.Font.Size * (fontScaleFactor + 0.1))
        HintTextBox.Size = New Drawing.Size(HintTextBox.Parent.Width * 0.52, HintTextBox.Parent.Height * 0.141)
        HintTextBox.Font = New Drawing.Font(HintTextBox.Font.FontFamily, HintTextBox.Font.Size * (fontScaleFactor + 0.1))

        Cancel.Size = New System.Drawing.Size((Cancel.Parent.Width * 0.13), (Cancel.Parent.Height * 0.46))
        Cancel.Font = New Drawing.Font(Cancel.Font.FontFamily, Cancel.Font.Size * fontScaleFactor)
        SendEncryptedMessageButton.Size = New System.Drawing.Size((SendEncryptedMessageButton.Parent.Width * 0.2), _
                                                                  (SendEncryptedMessageButton.Parent.Height * 0.46))
        SendEncryptedMessageButton.Font = New Drawing.Font(SendEncryptedMessageButton.Font.FontFamily, SendEncryptedMessageButton.Font.Size * fontScaleFactor)

        userChoiceGroupBox.Size = New System.Drawing.Size((userChoiceGroupBox.Parent.Width * 0.53), _
                                                          (userChoiceGroupBox.Parent.Height * 0.78))
        doneButton.Size = New System.Drawing.Size((doneButton.Parent.Width * 0.2), (doneButton.Parent.Height * 0.46))


        'Position
        Dim V_Spacing As Integer = Me.Size.Height * 0.025 '(Almost 5)
        Dim H_Spacing As Integer = Me.Size.Width * 0.023 '(Almost 2 X V_Spacing i.e 10)

        TopPanel.Location = New Drawing.Point(LogoPictureBox.Width + H_Spacing, V_Spacing)
        IntroLabel.Location = New Drawing.Point(H_Spacing / 2, V_Spacing)
        PasswordLabel.Location = New Drawing.Point(H_Spacing / 2, _
                                                   IntroLabel.Location.Y + IntroLabel.Height + 2 * V_Spacing)
        ConfirmPasswordLabel.Location = New Drawing.Point(H_Spacing / 2, _
                                                          PasswordLabel.Location.Y + PasswordLabel.Height + V_Spacing)
        HintLabel.Location = New Drawing.Point(H_Spacing / 2, _
                                               ConfirmPasswordLabel.Location.Y + ConfirmPasswordLabel.Height + V_Spacing)

        PasswordTextBox.Location = New Drawing.Point(PasswordLabel.Width + H_Spacing, PasswordLabel.Location.Y)
        ConfirmPasswordTextBox.Location = New Drawing.Point(PasswordTextBox.Location.X, ConfirmPasswordLabel.Location.Y)
        HintTextBox.Location = New Drawing.Point(ConfirmPasswordTextBox.Location.X, HintLabel.Location.Y)

        EncryptionProgressLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - EncryptionProgressLabel.Width) / 2) + LogoPictureBox.Width, _
                                                             TopPanel.Location.Y + TopPanel.Size.Height + V_Spacing)
        EncryptionProgressBar.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - EncryptionProgressBar.Width) / 2) + LogoPictureBox.Width, _
                                                           EncryptionProgressLabel.Location.Y + EncryptionProgressLabel.Height)
        encryptionStatusLabel.Location = New Drawing.Point(EncryptionProgressLabel.Location.X, _
                                                           EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

        ButtonPanelInitial.Location = New Drawing.Point(LogoPictureBox.Width, _
                                                     encryptionStatusLabel.Location.Y + encryptionStatusLabel.Height)
        NoteLabel.Location = New Drawing.Point(H_Spacing, (ButtonPanelInitial.Height - NoteLabel.Height) / 2)
        Cancel.Location = New Drawing.Point(ButtonPanelInitial.Width - (2 * H_Spacing) - Cancel.Width - SendEncryptedMessageButton.Width, (ButtonPanelInitial.Height - Cancel.Height) / 2)
        SendEncryptedMessageButton.Location = New Drawing.Point(ButtonPanelInitial.Width - H_Spacing - SendEncryptedMessageButton.Width, (ButtonPanelInitial.Height - SendEncryptedMessageButton.Height) / 2)

        UserChoicePanel.Location = New Drawing.Point(LogoPictureBox.Width, _
                                                     encryptionStatusLabel.Location.Y + encryptionStatusLabel.Height)
        userChoiceIntroLabel.Location = New Drawing.Point(H_Spacing, V_Spacing)
        userChoiceGroupBox.Location = New Drawing.Point(2 * H_Spacing, _
                                            userChoiceIntroLabel.Location.Y + userChoiceIntroLabel.Height)
        doneButton.Location = New Drawing.Point(UserChoicePanel.Width - H_Spacing - doneButton.Width, (UserChoicePanel.Height - doneButton.Height) / 2)

            If currentItem.Subject = "" Or currentItem.To = "" Then
                Me.Text = """" & "[Empty Message]" & """" & " | Quick Security"
            Else
                Me.Text = """" & currentItem.Subject.ToString & """" & " send to " & currentItem.To.ToString & " | Quick Security" 'Set DialogBox Title to Message Subject
            End If
        saveEncryptedRB.Checked = True
        PasswordTextBox.Focus()

        MyOutlookAddIn.currentEncryptionRibbon.quickEncryption.Enabled = False

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try

    End Sub

    

    Private Sub doneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles doneButton.Click
        Try
            Me.Hide()
            BackgroundWorker1.RunWorkerAsync()
            Timer1.Enabled = True
            Timer1.Start()

        Catch ex As System.Exception
            'MsgBox(ex.Message, , "Something Went Wrong : donebutton Click!")
        End Try

    End Sub
    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Try
            'User Choice Code running in the background
            'MsgBox("BG started...")

            Dim objOutlook1 As Outlook.Application
            objOutlook1 = Globals.ThisAddIn.Application
            Dim objNS1 As Outlook.NameSpace = objOutlook1.Session
            Dim objFolder1 As Outlook.MAPIFolder
            objFolder1 = objNS1.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

            'Keeps checking whether the message has been saved or not after every 2 seconds.
            While (Not isSentMessageSaved())
                System.Threading.Thread.Sleep(2000)
            End While

            'Function to perform on various choice selections
            If saveEncryptedRB.Checked = True Then
                'Save_Encrypted_Message
                Exit Sub

            ElseIf saveDecryptedRB.Checked = True Then
                If isSentMessageSaved() Then
                    'Save_Decrypted_Message
                    'Get the message from Sent Box folder decrypt it and set its body as unencrypted message and save it.
                    Dim objOutlook As Outlook._Application
                    objOutlook = Globals.ThisAddIn.Application
                    Dim objNS As Outlook._NameSpace = objOutlook.Session
                    Dim objFolder As Outlook.MAPIFolder
                    objFolder = objNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

                    Dim recentlySentMessage As Outlook.MailItem = CType(objFolder.Items.GetLast(), Outlook.MailItem)
                    recentlySentMessage.HTMLBody = originalMessage
                    recentlySentMessage.Save()

                    'Save Decrypted Attachments
                    If originalMsgAttachments.Count > 0 Then
                        'Delete all encrypted attachments
                        Dim k As Integer = recentlySentMessage.Attachments.Count
                        For j As Integer = 1 To k
                            recentlySentMessage.Attachments(1).Delete()
                        Next

                        'Now Attach the original unencrypted attachment(s)
                        For Each attachmentSource As String In originalAttachmentsSource

                            Dim msgAttachs As Outlook.Attachments = recentlySentMessage.Attachments
                            Dim msgAttach As Outlook.Attachment
                            msgAttach = msgAttachs.Add(attachmentSource.ToString)
                        Next
                    End If

                    Exit Sub

                End If

            ElseIf deleteMessageRB.Checked = True Then
                'Delete_Message
                'Get the message from Sent Box folder and delete it.

                If isSentMessageSaved() Then

                    Dim objOutlook2 As Outlook._Application
                    objOutlook2 = Globals.ThisAddIn.Application
                    Dim objNS2 As Outlook._NameSpace = objOutlook2.Session
                    Dim objFolder2 As Outlook.MAPIFolder
                    objFolder2 = objNS2.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

                    Dim recentMessage2 As Outlook.MailItem = CType(objFolder2.Items.GetLast(), Outlook.MailItem)
                    recentMessage2.Delete()

                    Exit Sub


                End If

            End If

        Catch ex As System.Exception
            '  MsgBox(ex.Message, , "Something Went Wrong : bgworker!")
        End Try
    End Sub


    Public Function isSentMessageSaved() As Boolean
        Try

            Dim objOutlook1 As Outlook.Application
            objOutlook1 = Globals.ThisAddIn.Application
            Dim objNS1 As Outlook.NameSpace = objOutlook1.Session
            Dim objFolder1 As Outlook.MAPIFolder
            objFolder1 = objNS1.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

            Dim checkRecentMessage As Outlook.MailItem = CType(objFolder1.Items.GetLast(), Outlook.MailItem)
            'Dim currentMsgID As String = CType(checkRecentMessage.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x1035001E").ToString(), String)
            'Dim currentMsgID As String = checkRecentMessage.EntryID

            Dim savedMsgDate As Date = checkRecentMessage.TaskCompletedDate
            Dim savedMsgTo As String = checkRecentMessage.To
            savedMsgTo = savedMsgTo.Remove(0, 1)
            Dim l As Integer = savedMsgTo.Length
            savedMsgTo = savedMsgTo.Substring(0, l - 1)
            Dim savedMsgSubject As String = checkRecentMessage.Subject

            Dim isMessageSavedInSentBox As Boolean = False

            If (savedMsgDate.Equals(originalMsgDate)) Then
                If (savedMsgTo = originalMsgTo) Then
                    If (savedMsgSubject = originalMsgSubject) Then
                        isMessageSavedInSentBox = True
                    End If
                End If
            End If
            Return isMessageSavedInSentBox

        Catch ex As System.Exception
            'MsgBox(ex.Message, , "Something Went Wrong : Isentmessagesaved function!")

        End Try

    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not BackgroundWorker1.IsBusy Then
            Me.Close()
        End If
    End Sub


    Public Function isPasswordValid() As Boolean
        Try
            Dim b As Integer = 0
            Dim c As Integer = 0
            ' User Password

            If String.IsNullOrEmpty(PasswordTextBox.Text) Then
                PasswordTextBox.BackColor = Drawing.Color.Red
                MsgBox("Secret-Phrase cannot be left Empty.", MsgBoxStyle.Exclamation, "No Secret")

                'tt1.Show(tt1.GetToolTip(PasswordTextBox), PasswordTextBox, 4000)
                Return False
                Exit Function

            Else

                PasswordTextBox.BackColor = Drawing.Color.White
                'Dim password As Regex = New Regex("^(?=.*\d)(?=.*[a-zA-Z])(?!.*\s).{8,100}$") ' at least 8 chars and contains at least 1 Numeric character
                Dim password As Regex = New Regex(".{8,}")
                Dim M As Match = password.Match(PasswordTextBox.Text)

                If M.Success Then
                    b = 1
                Else
                    PasswordTextBox.BackColor = Drawing.Color.Red
                    MsgBox("Secret-Phrase must be at least 8 characters long.", MsgBoxStyle.Exclamation, "Invalid Secret")

                    ToolTip1.Show(ToolTip1.GetToolTip(PasswordTextBox), PasswordTextBox, 4000)
                    Return False
                    Exit Function

                End If

            End If

            ' Confirm User Password

            If String.IsNullOrEmpty(ConfirmPasswordTextBox.Text) Then
                ConfirmPasswordTextBox.BackColor = Drawing.Color.Red
            Else
                ConfirmPasswordTextBox.BackColor = Drawing.Color.White
                If ConfirmPasswordTextBox.Text = PasswordTextBox.Text Then
                    c = 1
                Else
                    ConfirmPasswordTextBox.BackColor = Drawing.Color.Red
                    MsgBox("Secrets Do Not Match!.", MsgBoxStyle.Exclamation, "Secrets Mismatch!")

                    'tt1.Show(tt1.GetToolTip(ConfirmPasswordTextBox), ConfirmPasswordTextBox, 4000)
                    Return False
                    Exit Function
                End If

            End If


            If b = 1 And c = 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try

    End Function

    Private Function EncryptAttachments(ByRef msgAttachments As Outlook.Attachments, ByVal CEKEncryptionAlgorithm As String, ByVal PBEkey As String) As String()
        Try

            My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath & "EcubeOriginalAttachments")
            My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath & "EcubeEncryptedAttachments")


            'Dim originalAttachmentsSource As String() = New String(msgAttachments.Count - 1) {} 'Declared as Global Variable
            Dim encryptedAttachmentsSource As String() = New String(msgAttachments.Count - 1) {}

            For i As Integer = 0 To msgAttachments.Count - 1

                'Save attachment to Temp\EcubeOriginalAttachments
                msgAttachments(i + 1).SaveAsFile(System.IO.Path.GetTempPath & "EcubeOriginalAttachments\" & (msgAttachments(i + 1).FileName))
                originalAttachmentsSource(i) = System.IO.Path.GetTempPath & "EcubeOriginalAttachments\" & (msgAttachments(i + 1).FileName)

                'Convert attachment to byte() {byteData()}
                Dim byteData As Byte() = Nothing
                Dim fs As New System.IO.FileStream(originalAttachmentsSource(i), FileMode.Open, FileAccess.Read)
                Dim tempBuffer As Byte() = New Byte(fs.Length - 1) {}

                fs.Read(tempBuffer, 0, fs.Length)
                byteData = tempBuffer
                fs.Close()

                'Envelope and encrypt the byteData()
                Dim envelopedData As Byte() = PasswordBasedEncryption(CEKEncryptionAlgorithm, byteData, PBEkey)

                'Write the envelopedData() byte array into a file and save it on users local hard disk
                Dim smimeSource As String = System.IO.Path.GetTempPath & "EcubeEncryptedAttachments\" & (msgAttachments(i + 1).FileName & ".p7m")
                Dim fs2 As New FileStream(smimeSource, FileMode.Create, FileAccess.Write)
                fs2.Write(envelopedData, 0, envelopedData.Length)
                fs2.Close()

                encryptedAttachmentsSource(i) = smimeSource

            Next

            Return encryptedAttachmentsSource

        Catch ex As System.Exception
            MsgBox(ex.Message, , "Something Went Wrong!")
        End Try

    End Function


    'Code to Disable the 'X' button

    Protected Overrides ReadOnly Property CreateParams() As Windows.Forms.CreateParams
        Get
            Dim cp As Windows.Forms.CreateParams = MyBase.CreateParams
            Const CS_NOCLOSE As Integer = &H200
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property
   
    
End Class

