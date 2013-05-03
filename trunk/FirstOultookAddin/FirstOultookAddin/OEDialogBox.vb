Imports System.IO
Imports Org.BouncyCastle.Cms

Public Class OEDialogBox
    'Global Variables
    Dim parentMsg As Outlook.MailItem = Nothing
    Dim Hint As String = ""
    Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveInspector.CurrentItem, Outlook.MailItem)
    Dim originalMessage As String = currentItem.Body.ToString
    'Dim originalMsgID As String = currentItem.EntryID
    Dim originalMsgDate As Date = currentItem.TaskCompletedDate
    Dim originalMsgTo As String = currentItem.To
    Dim originalMsgSubject As String = currentItem.Subject
    Dim originalMsgAttachments As Outlook.Attachments = currentItem.Attachments
    Dim originalAttachmentsSource As String() = New String(currentItem.Attachments.Count - 1) {}

    Sub New(ByVal parentMsg As Outlook.MailItem)

        InitializeComponent()
        Me.parentMsg = parentMsg
    End Sub

    Private Sub OEDialogBox_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            'Cleanup
            'Set all global variables to nothing

            originalMessage = Nothing
            currentItem = Nothing
            originalMsgDate = Nothing
            originalMsgTo = Nothing
            originalMsgSubject = Nothing

        Catch ex As System.Exception
            'MsgBox(ex.Message, , "Something Went Wrong!")
        End Try
    End Sub

    Private Sub OEDialogBox_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
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


    Private Sub OEDialogBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.ecubeicon

        Dim pa As Microsoft.Office.Interop.Outlook.PropertyAccessor
        pa = parentMsg.PropertyAccessor

        Dim hint As String = CType(pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Hint"), String)
        HintLabel.Text = "Hint: " & hint    'Set Hint value on OEDialogBox as in the parent message header
        Me.Hint = hint 'Set value of global hint variable

        Me.Focus()
        PasswordTextBox.Focus()
    End Sub


    Private Sub EncryptReply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EncryptReply.Click

        'Check whether the supplied Password is same as of Parent Message or not
        If checkUserPassword() = False Then
            PasswordTextBox.SelectAll()
            PasswordTextBox.Focus()
            Exit Sub
        End If

        'If above holds true then Encrypt the Reply using supplied Password
        Try

            'Retrieve the Current MailMessage Details

            Dim messageBody As String
            Dim messageRecipients As String
            Dim messageCCRecipients As String
            Dim messageBCCRecipients As String
            Dim messageSubject As String
            Dim messageAttachments As Outlook.Attachments

            'Set messageBodyFormat variable according to the selected BodyFormat 
            Dim msgBodyFormat As Outlook.OlBodyFormat = currentItem.BodyFormat
            If msgBodyFormat = Outlook.OlBodyFormat.olFormatHTML Then
                messageBody = currentItem.HTMLBody                  'TML formatted message
            ElseIf msgBodyFormat = Outlook.OlBodyFormat.olFormatPlain Then
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


            'Adding Custom Message Headers
            Dim pa As Outlook.PropertyAccessor
            pa = currentItem.PropertyAccessor
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version", "ECube-1.0")
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Hint", Hint.ToString)
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-Encryption-Type", "Quick")
            'pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-Encryption-Status", "AES-Encrypted")
            pa.SetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/Content-Description", "S/MIME Encrypted Message")


            ''Set progress bar to 50%
            'EncryptionProgressBar.Value = 75
            'EncryptionProgressBar.Refresh()
            'encryptionStatusLabel.Text = "Message Headers Added"
            'encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
            '                                                 EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

            'encryptionStatusLabel.Refresh()
            'System.Threading.Thread.Sleep(500)

            'Convert MessageBody (as String) to Byte Array using UTF-8 Encoding

            Dim messageBodyData() As Byte
            messageBodyData = System.Text.UTF8Encoding.UTF8.GetBytes(messageBody)    'Microsoft Outlook 2007 uses Wester European (UTF-8) Encoding Scheme to compose New Messages

            'hence the msg body text is converted to UTF-8 encoded bytes not ASCII.
            'A simple rule Read the text in the encoding it is present (eg: UTF-8 in Outlook) and Retrieve or Write the text in the same Encoding scheme to get the correct output.

            'Encrypt the messageBodyData
            Dim encodedData As Byte() = PasswordBasedEncryption(CmsEnvelopedDataGenerator.DesEde3Cbc, messageBodyData, PasswordTextBox.Text.ToString)

            currentItem.Body = RichTextBox1.Text

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

            ''Set progress bar to 75%
            'EncryptionProgressBar.Value = 100
            'EncryptionProgressBar.Refresh()
            'encryptionStatusLabel.ForeColor = Drawing.Color.Green
            'encryptionStatusLabel.Font = New System.Drawing.Font("Arial", 9, Drawing.FontStyle.Bold)
            'encryptionStatusLabel.Text = "Message Successfully Encrypted!"
            'encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
            '                                                 EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)

            'encryptionStatusLabel.Refresh()
            'System.Threading.Thread.Sleep(500)

            'Send the current Password Encrypted Message
            'currentItem.Send()
            Me.Hide()

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

            EncryptionProgressLabel.ForeColor = Drawing.Color.Green
            EncryptionProgressLabel.Text = "Encryption Successfull! Sending Message.."
            EncryptionProgressLabel.Refresh()
            System.Threading.Thread.Sleep(2000)

            ''Set progress bar to 100%
            'EncryptionProgressBar.Value = EncryptionProgressBar.Maximum

            'encryptionStatusLabel.Text = "Encryption Successfull! Sending Message.."
            'encryptionStatusLabel.Location = New Drawing.Point(((Me.Size.Width - LogoPictureBox.Width - encryptionStatusLabel.Width) / 2) + LogoPictureBox.Width, _
            '                                                 EncryptionProgressBar.Location.Y + EncryptionProgressBar.Height)
            'EncryptionProgressBar.Refresh()
            'encryptionStatusLabel.Refresh()
            ''System.Threading.Thread.Sleep(2000)

            'Display the UserChoicePanel and hide the previous panel
            'ButtonPanelInitial.Hide()
            'ButtonPanelInitial.Enabled = False
            'TopPanel.Enabled = False

            'UserChoicePanel.Visible = True
            'UserChoicePanel.Enabled = True

            'UserChoicePanel.Show()
            'UserChoicePanel.Focus()

            'Me.CancelButton = doneButton

            Me.Close()
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Catch ex As System.Exception
            'MsgBox(ex.ToString)
            EncryptionProgressLabel.ForeColor = Drawing.Color.Red
            EncryptionProgressLabel.Text = "Failed to encrypt the message! Please try again."
        End Try

    End Sub

    Private Sub DontEncryptReply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DontEncryptReply.Click
        Me.Close()

    End Sub

    Private Function checkUserPassword() As Boolean

        'Save and Read the encoded Data from the smime.p7m attachment
        Dim encodedData As Byte() = Nothing
        EncryptionProgressLabel.ForeColor = Drawing.SystemColors.ControlText
        EncryptionProgressLabel.Text = ""

        Try
            My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath) 'Create a temp directory to store encrypted (received) message and attachments

            Dim saveAttachment As Outlook.Attachment = parentMsg.Attachments("smime.p7m")
            saveAttachment.SaveAsFile(System.IO.Path.GetTempPath & "check.eee")

            If My.Computer.FileSystem.FileExists(System.IO.Path.GetTempPath & "check.eee") Then

                Dim checkSource As String = System.IO.Path.GetTempPath & "check.eee"
                Dim fs As New IO.FileStream(checkSource, FileMode.Open, FileAccess.Read)
                Dim tempBuffer As Byte() = New Byte() {0}
                ReDim tempBuffer(fs.Length - 1)

                fs.Read(tempBuffer, 0, fs.Length)
                encodedData = tempBuffer

                fs.Close()

            Else

            End If

        Catch ex As System.IO.IOException
            MsgBox(ex.Message.ToString)
            EncryptionProgressLabel.ForeColor = Drawing.Color.Red
            EncryptionProgressLabel.Text = ex.Message.ToString
        Catch ex2 As System.Exception
            MsgBox(ex2.Message.ToString)
            EncryptionProgressLabel.ForeColor = Drawing.Color.Red
            EncryptionProgressLabel.Text = ex2.Message.ToString
        End Try

        'Decrypting the Message Body [encodedData()]
        PasswordBasedDecryption(encodedData, PasswordTextBox.Text.ToString)

        If EncryptionProgressLabel.ForeColor = Drawing.Color.Red Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function PasswordBasedDecryption(ByVal encodedData As Byte(), ByRef PBEKey As String) As Byte() 'Returns Decoded data as Byte()
        Try

            'Dim encodedData As Byte() = encodedData1
            Dim recipientID As New RecipientID()
            Dim decodedEnvelopeData As New CmsEnvelopedData(encodedData)
            Dim recipient As PasswordRecipientInformation = decodedEnvelopeData.GetRecipientInfos().GetFirstRecipient(recipientID)

            Dim key As CmsPbeKey = New Pkcs5Scheme2Utf8PbeKey(PBEKey.ToCharArray(), recipient.KeyDerivationAlgorithm) 'Decryption is happening here!
            Dim decodedData As Byte() = New Byte(recipient.GetContent(key).Length - 1) {}
            decodedData = recipient.GetContent(key)

            Return decodedData


        Catch ex As System.Exception
            If ex.Message.Contains("key corrupt") Then
                EncryptionProgressLabel.ForeColor = Drawing.Color.Red
                EncryptionProgressLabel.Text = "Incorrect Secret. Please Try again!"
                Return New Byte(0) {0}
            Else
                EncryptionProgressLabel.ForeColor = Drawing.Color.Red
                EncryptionProgressLabel.Text = ex.Message.ToString
                Return New Byte(0) {0}
            End If

        End Try
    End Function
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

    'Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    '    Try
    '        'User Choice Code running in the background
    '        'MsgBox("BG started...")

    '        Dim objOutlook1 As Outlook.Application
    '        objOutlook1 = Globals.ThisAddIn.Application
    '        Dim objNS1 As Outlook.NameSpace = objOutlook1.Session
    '        Dim objFolder1 As Outlook.MAPIFolder
    '        objFolder1 = objNS1.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

    '        'Keeps checking whether the message has been saved or not after every 2 seconds.
    '        While (Not isSentMessageSaved())
    '            System.Threading.Thread.Sleep(2000)
    '        End While

    '        'Function to perform on various choice selections
    '        If saveEncryptedRB.Checked = True Then
    '            'Save_Encrypted_Message
    '            Exit Sub

    '        ElseIf saveDecryptedRB.Checked = True Then
    '            If isSentMessageSaved() Then
    '                'Save_Decrypted_Message
    '                'Get the message from Sent Box folder decrypt it and set its body as unencrypted message and save it.
    '                Dim objOutlook As Outlook._Application
    '                objOutlook = Globals.ThisAddIn.Application
    '                Dim objNS As Outlook._NameSpace = objOutlook.Session
    '                Dim objFolder As Outlook.MAPIFolder
    '                objFolder = objNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

    '                Dim recentlySentMessage As Outlook.MailItem = CType(objFolder.Items.GetLast(), Outlook.MailItem)
    '                recentlySentMessage.HTMLBody = originalMessage
    '                recentlySentMessage.Save()

    '                'Save Decrypted Attachments
    '                If originalMsgAttachments.Count > 0 Then
    '                    'Delete all encrypted attachments
    '                    Dim k As Integer = recentlySentMessage.Attachments.Count
    '                    For j As Integer = 1 To k
    '                        recentlySentMessage.Attachments(1).Delete()
    '                    Next

    '                    'Now Attach the original unencrypted attachment(s)
    '                    For Each attachmentSource As String In originalAttachmentsSource

    '                        Dim msgAttachs As Outlook.Attachments = recentlySentMessage.Attachments
    '                        Dim msgAttach As Outlook.Attachment
    '                        msgAttach = msgAttachs.Add(attachmentSource.ToString)
    '                    Next
    '                End If

    '                Exit Sub

    '            End If

    '        ElseIf deleteMessageRB.Checked = True Then
    '            'Delete_Message
    '            'Get the message from Sent Box folder and delete it.

    '            If isSentMessageSaved() Then

    '                Dim objOutlook2 As Outlook._Application
    '                objOutlook2 = Globals.ThisAddIn.Application
    '                Dim objNS2 As Outlook._NameSpace = objOutlook2.Session
    '                Dim objFolder2 As Outlook.MAPIFolder
    '                objFolder2 = objNS2.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

    '                Dim recentMessage2 As Outlook.MailItem = CType(objFolder2.Items.GetLast(), Outlook.MailItem)
    '                recentMessage2.Delete()

    '                Exit Sub


    '            End If

    '        End If

    '    Catch ex As System.Exception
    '        MsgBox(ex.Message, , "Something Went Wrong!")
    '    End Try
    'End Sub


    'Public Function isSentMessageSaved() As Boolean
    '    Try

    '        Dim objOutlook1 As Outlook.Application
    '        objOutlook1 = Globals.ThisAddIn.Application
    '        Dim objNS1 As Outlook.NameSpace = objOutlook1.Session
    '        Dim objFolder1 As Outlook.MAPIFolder
    '        objFolder1 = objNS1.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

    '        Dim checkRecentMessage As Outlook.MailItem = CType(objFolder1.Items.GetLast(), Outlook.MailItem)
    '        'Dim currentMsgID As String = CType(checkRecentMessage.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x1035001E").ToString(), String)
    '        'Dim currentMsgID As String = checkRecentMessage.EntryID

    '        Dim savedMsgDate As Date = checkRecentMessage.TaskCompletedDate
    '        Dim savedMsgTo As String = checkRecentMessage.To
    '        savedMsgTo = savedMsgTo.Remove(0, 1)
    '        Dim l As Integer = savedMsgTo.Length
    '        savedMsgTo = savedMsgTo.Substring(0, l - 1)
    '        Dim savedMsgSubject As String = checkRecentMessage.Subject

    '        Dim isMessageSavedInSentBox As Boolean = False

    '        If (savedMsgDate.Equals(originalMsgDate)) Then
    '            If (savedMsgTo = originalMsgTo) Then
    '                If (savedMsgSubject = originalMsgSubject) Then
    '                    isMessageSavedInSentBox = True
    '                End If
    '            End If
    '        End If
    '        Return isMessageSavedInSentBox

    '    Catch ex As System.Exception
    '        MsgBox(ex.Message, , "Something Went Wrong!")

    '    End Try

    'End Function


    'Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    '    If Not BackgroundWorker1.IsBusy Then
    '        Me.Close()
    '    End If
    'End Sub


    'Code to Disable the 'X' button

    Protected Overrides ReadOnly Property CreateParams() As Windows.Forms.CreateParams
        Get
            Dim cp As Windows.Forms.CreateParams = MyBase.CreateParams
            Const CS_NOCLOSE As Integer = &H200
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property

    Private Sub PasswordTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles PasswordTextBox.KeyPress

        If e.KeyChar = CType(ChrW(System.Windows.Forms.Keys.Enter), Char) Then
            EncryptReply_Click(sender, e)
        End If
    End Sub

End Class