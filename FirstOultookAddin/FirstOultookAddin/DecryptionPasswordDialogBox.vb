Imports Org.BouncyCastle.Cms
Imports System.Collections
Imports System.IO

Public Class DecryptionPasswordDialogBox

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    'Global variables
    ' currently Selected Item as currentItem
    Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveExplorer.Selection(1), Outlook.MailItem)
    Dim dialogCallOrigin As String = ""
    Dim isDecrypted As Boolean = False

    Sub New(ByVal dialogCallOrigin As String)
        ' TODO: Complete member initialization 
        InitializeComponent()
        Me.dialogCallOrigin = dialogCallOrigin
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayDecryptedMessageButton.Click
        Try
            decryptionStatusLabel.Text = ""

            'Validations
            'Password Text Box validations
            If isPasswordValid() = False Then
                PasswordTextBox.Focus()
                Exit Sub
            End If

            'Correct Password Validations
            If checkUserPassword() = False Then
                PasswordTextBox.SelectAll()
                PasswordTextBox.Focus()
                Exit Sub
            End If

            Dim password As String = PasswordTextBox.Text.Trim(" ")
            Dim PBEKey As String = password
            password = Nothing

            'Retrieve the Current MailMessage Details

            Dim messageBody As String
            Dim messageRecipients As String
            Dim messageCCRecipients As String
            Dim messageBCCRecipients As String
            Dim messageSubject As String
            Dim messageAttachments As Outlook.Attachments

            messageBody = currentItem.Body
            messageRecipients = currentItem.To
            messageCCRecipients = currentItem.CC
            messageBCCRecipients = currentItem.BCC
            messageSubject = currentItem.Subject
            messageAttachments = currentItem.Attachments

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''Self attempted Byte() to String conversion and vice-versa

            ''Decryption Side
            ''String to String() conversion using Delimiter
            'Dim temp6 As String()
            'temp6 = messageBody.Split("#")
            'Dim arrayLength As Integer = temp6.Length - 1
            ''temp6.SetValue(Nothing, arrayLength)

            ''String() to Byte() retrieval conversion
            'Dim dataRetrieved As Byte() = New Byte(arrayLength + 1) {}
            'Dim k As Integer = 0
            'For Each value In temp6
            '    If k < arrayLength - 1 Then
            '        dataRetrieved(k) = Byte.Parse(value)
            '        k = k + 1
            '    End If
            'Next

            ''Byte() content display code
            'Dim temp2 As String = ""
            'For Each value In dataRetrieved
            '    temp2 = temp2 & value
            'Next
            '    'MsgBox("Retrived Byte array: " & temp2)


            ''''''''''''''''''''''''''''''''''''''''''''''''

            'Save and Read the encoded Data from the smime.p7m attachment
            Dim encodedData As Byte() = Nothing

            Try
                My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath & "EcubeDecryptAttachments") 'Create a temp directory to store encrypted (received) message and attachments

                Dim saveAttachment As Outlook.Attachment = currentItem.Attachments("smime.p7m")
                saveAttachment.SaveAsFile(System.IO.Path.GetTempPath & "EcubeDecryptAttachments\" & (currentItem.Attachments("smime.p7m").FileName))

                If My.Computer.FileSystem.FileExists(System.IO.Path.GetTempPath & "EcubeDecryptAttachments\smime.p7m") Then

                    Dim smimeSource As String = System.IO.Path.GetTempPath & "EcubeDecryptAttachments\smime.p7m"
                    Dim fs As New IO.FileStream(smimeSource, FileMode.Open, FileAccess.Read)
                    Dim tempBuffer As Byte() = New Byte() {0}
                    ReDim tempBuffer(fs.Length - 1)

                    fs.Read(tempBuffer, 0, fs.Length)
                    encodedData = tempBuffer

                    fs.Close()

                Else

                End If

            Catch ex As System.IO.IOException
                MsgBox(ex.Message.ToString)
                decryptionStatusLabel.ForeColor = Drawing.Color.Red
                decryptionStatusLabel.Text = ex.Message.ToString
            Catch ex2 As Exception
                MsgBox(ex2.Message.ToString)
                decryptionStatusLabel.ForeColor = Drawing.Color.Red
                decryptionStatusLabel.Text = ex2.Message.ToString
            End Try

            'Decrypting the Message Body [encodedData()]
            Dim decodedData As Byte() = PasswordBasedDecryption(encodedData, PBEKey)

            Dim decryptedMessage As String = ""
            Dim utf8Object As New System.Text.UTF8Encoding()
            decryptedMessage = utf8Object.GetString(decodedData)

            'Get the current InspectorWindow object
            Dim currentItemNewInspectorWindow As Outlook.MailItem = Nothing
            If (dialogCallOrigin = "DoubleClick") Then
                currentItemNewInspectorWindow = CType(Globals.ThisAddIn.Application.ActiveInspector.CurrentItem, Outlook.MailItem)
                'Set the decrypted message in current InspectorWindow
                Dim msgBodyFormat As Outlook.OlBodyFormat = currentItem.BodyFormat
                If msgBodyFormat = Outlook.OlBodyFormat.olFormatHTML Then
                    currentItemNewInspectorWindow.HTMLBody = decryptedMessage                 'HTML formatted message
                ElseIf msgBodyFormat = Outlook.OlBodyFormat.olFormatPlain Then
                    currentItemNewInspectorWindow.Body = decryptedMessage                    'PlainText message
                Else
                    currentItemNewInspectorWindow.HTMLBody = decryptedMessage 'For Rich-Text and Unspecified formats and any others that might come in future
                End If

            ElseIf (dialogCallOrigin = "RightClickMenu") Then

                Dim selectedItem As Outlook.MailItem = Globals.ThisAddIn.Application.ActiveExplorer.Selection(1)

                'Set the decrypted message in current InspectorWindow
                Dim msgBodyFormat As Outlook.OlBodyFormat = selectedItem.BodyFormat
                If msgBodyFormat = Outlook.OlBodyFormat.olFormatHTML Then
                    selectedItem.HTMLBody = decryptedMessage                 'HTML formatted message
                ElseIf msgBodyFormat = Outlook.OlBodyFormat.olFormatPlain Then
                    selectedItem.Body = decryptedMessage                    'PlainText message
                Else
                    selectedItem.HTMLBody = decryptedMessage 'For Rich-Text and Unspecified formats and any others that might come in future
                End If
            End If

            'Decrypting the Attachments
            Dim decryptedAttachmentsSource As String() = DecryptAttachments(PBEKey, messageAttachments)

            'Remove the current Encrypted attachments
            Dim currentMailItem As Outlook.MailItem = Nothing
            If (dialogCallOrigin = "RightClickMenu") Then
                currentMailItem = currentItem
            ElseIf (dialogCallOrigin = "DoubleClick") Then
                currentMailItem = currentItemNewInspectorWindow
            End If
            Dim k As Integer = currentMailItem.Attachments.Count
            For j As Integer = 1 To k
                currentMailItem.Attachments(1).Delete()
            Next

            'Attach the decrypted attachment(s)
            For Each attachmentSource As String In decryptedAttachmentsSource

                'Dim attachLen As String = currentItem.Body.Length
                Dim msgAttachs As Outlook.Attachments = currentItem.Attachments
                Dim msgAttach As Outlook.Attachment
                'msgAttach = msgAttachs.Add(attachmentSource.ToString, , attachLen + 1, smimeDisplayName)
                If attachmentSource.Equals(System.IO.Path.GetTempPath & "EcubeDecryptedAttachments\smime") Then
                    Continue For
                End If
                msgAttach = msgAttachs.Add(attachmentSource.ToString)
            Next

            isDecrypted = True 'Message Successfully Decrypted

            '' Clean up
            'delete all the Folders (and files inside them) created by this form (including all the Attachment stuff)

            If My.Computer.FileSystem.DirectoryExists(System.IO.Path.GetTempPath & "EcubeDecryptAttachments") Then
                My.Computer.FileSystem.DeleteDirectory(System.IO.Path.GetTempPath & "EcubeDecryptAttachments", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            If My.Computer.FileSystem.DirectoryExists(System.IO.Path.GetTempPath & "EcubeDecryptedAttachments") Then
                My.Computer.FileSystem.DeleteDirectory(System.IO.Path.GetTempPath & "EcubeDecryptedAttachments", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            PBEKey = Nothing

            messageBody = Nothing
            messageRecipients = Nothing
            messageCCRecipients = Nothing
            messageBCCRecipients = Nothing
            messageSubject = Nothing
            messageAttachments = Nothing

            decodedData = Nothing
            decryptedMessage = Nothing

            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            decryptionStatusLabel.ForeColor = Drawing.Color.Red
            decryptionStatusLabel.Text = ex.Message.ToString
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        'GUI of the Decrypt Email Message button in Decryption Ribbon
        If isDecrypted = False Then
            'Globals.Ribbons.Ribbon2.decryptEmailMessage.Enabled = True
            DecryptionRibbon.currentDecryptionRibbon.decryptEmailMessage.Enabled = True

        ElseIf isDecrypted = True Then
            'Globals.Ribbons.Ribbon2.decryptEmailMessage.Enabled = False
            DecryptionRibbon.currentDecryptionRibbon.decryptEmailMessage.Enabled = False

        End If

        Me.Close()
        'Me.Dispose()    'Just Experimenting

    End Sub

    Dim encWithECube As Boolean = False

    Private Sub DecryptionPasswordDialogBox_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        currentItem = Nothing
    End Sub


    Private Sub DecryptionPasswordDialogBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Retrieving Message Header
        'Dim currentItem As Outlook.MailItem
        'currentItem = CType(Globals.ThisAddIn.Application.ActiveExplorer.Selection(1), Outlook.MailItem)

        Me.Icon = My.Resources.ecubeicon

        DecryptionRibbon.currentDecryptionRibbon.decryptEmailMessage.Enabled = False


        Dim pa As Microsoft.Office.Interop.Outlook.PropertyAccessor
        pa = currentItem.PropertyAccessor

        Dim hint As String = CType(pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Hint"), String)
        HintLabel.Text = "Hint: " & hint                    'Set Hint value on DecryptionDialogBox as in the message header
        Me.Text = """" & currentItem.Subject.ToString & """" & " sent by " & currentItem.SenderEmailAddress.ToString & " | Quick Security" 'Set DialogBox Title to Message Subject

        Me.Focus()
        PasswordTextBox.Focus()

    End Sub

    Public Function saveAttachments(ByVal path As String) As String()

        Dim originalAttachmentsSource As String() = New String(currentItem.Attachments.Count - 1) {}

        'inBoxItems = inBoxItems.Restrict("[Unread] = true")

        Try
            If currentItem IsNot Nothing Then
                If currentItem.Attachments.Count > 0 Then

                    For i As Integer = 1 To currentItem.Attachments.Count
                        'Dim saveAttachment As Outlook.Attachment = currentItem.Attachments(i)
                        currentItem.Attachments(i).SaveAsFile(path & "\" & (currentItem.Attachments(i).FileName))
                        originalAttachmentsSource(i - 1) = path & "\" & (currentItem.Attachments(i).FileName)

                    Next i

                End If
            End If

            Return originalAttachmentsSource

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Private Function PasswordBasedDecryption(ByVal encodedData As Byte(), ByRef PBEKey As String) As Byte() 'Returns Decoded data as Byte()
        Try

        'Dim encodedData As Byte() = encodedData1
        Dim recipientID As New RecipientID()
        Dim decodedEnvelopeData As New CmsEnvelopedData(encodedData)
        Dim recipient As PasswordRecipientInformation = decodedEnvelopeData.GetRecipientInfos().GetFirstRecipient(recipientID)

        Dim key As CmsPbeKey = New Pkcs5Scheme2Utf8PbeKey(PBEKey.ToCharArray(), recipient.KeyDerivationAlgorithm)
        Dim decodedData As Byte() = New Byte(recipient.GetContent(key).Length - 1) {}
        decodedData = recipient.GetContent(key)

            Return decodedData


        Catch ex As Exception
            If ex.Message.Contains("key corrupt") Then
                decryptionStatusLabel.ForeColor = Drawing.Color.Red
                decryptionStatusLabel.Text = "Password Incorrect. Please Try again!"
                Return New Byte(0) {0}
            Else
                decryptionStatusLabel.ForeColor = Drawing.Color.Red
                decryptionStatusLabel.Text = ex.Message.ToString
                Return New Byte(0) {0}
            End If

        End Try
    End Function


    Private Function DecryptAttachments(ByRef PBEKey As String, ByVal msgAttachments As Outlook.Attachments) As String()

        My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath & "EcubeDecryptedAttachments")

        Dim originalAttachmentsSource As String() = saveAttachments(System.IO.Path.GetTempPath & "EcubeDecryptAttachments")
        Dim decryptedAttachmentsSource As String() = New String(msgAttachments.Count - 2) {}

        For i As Integer = 0 To msgAttachments.Count - 1
            'Dont decrypt smime.p7m attachment
            If originalAttachmentsSource(i).Equals(System.IO.Path.GetTempPath & "EcubeDecryptAttachments\smime.p7m") Then
                Continue For
            End If

            'Convert saved attachments to byte() {byteData()}
            Dim byteData As Byte() = Nothing
            Dim fs As New System.IO.FileStream(originalAttachmentsSource(i), FileMode.Open, FileAccess.Read)
            Dim tempBuffer As Byte() = New Byte(fs.Length - 1) {}

            fs.Read(tempBuffer, 0, fs.Length)
            byteData = tempBuffer
            fs.Close()

            'Decrypt the byteData()
            Dim decryptedData As Byte() = PasswordBasedDecryption(byteData, PBEKey)

            'Write the decryptedData() byte array into a file and save it on users local hard disk
            Dim smimeSource As String = System.IO.Path.GetTempPath & "EcubeDecryptedAttachments\" & (msgAttachments(i + 1).FileName)
            smimeSource = smimeSource.Substring(0, smimeSource.Length - 4)

            Dim fs2 As New FileStream(smimeSource, FileMode.Create, FileAccess.Write)
            fs2.Write(decryptedData, 0, decryptedData.Length)
            fs2.Close()

            If i <> 0 Then
                decryptedAttachmentsSource(i - 1) = smimeSource
            ElseIf i = 0 Then
                decryptedAttachmentsSource(i) = smimeSource
            End If
        Next

        Return decryptedAttachmentsSource
    End Function


    Private Function checkUserPassword() As Boolean

        'Save and Read the encoded Data from the smime.p7m attachment
        Dim encodedData As Byte() = Nothing
        decryptionStatusLabel.ForeColor = Drawing.SystemColors.ControlText
        decryptionStatusLabel.Text = ""

        Try
            My.Computer.FileSystem.CreateDirectory(System.IO.Path.GetTempPath) 'Create a temp directory to store encrypted (received) message and attachments

            Dim saveAttachment As Outlook.Attachment = currentItem.Attachments("smime.p7m")
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
            decryptionStatusLabel.ForeColor = Drawing.Color.Red
            decryptionStatusLabel.Text = ex.Message.ToString
        Catch ex2 As Exception
            MsgBox(ex2.Message.ToString)
            decryptionStatusLabel.ForeColor = Drawing.Color.Red
            decryptionStatusLabel.Text = ex2.Message.ToString
        End Try

        'Decrypting the Message Body [encodedData()]
        PasswordBasedDecryption(encodedData, PasswordTextBox.Text.ToString)

        If decryptionStatusLabel.ForeColor = Drawing.Color.Red Then
            Return False
        Else
            Return True
        End If

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

    Public Function isPasswordValid() As Boolean

        ' User Password

        If String.IsNullOrEmpty(PasswordTextBox.Text) Then
            PasswordTextBox.BackColor = Drawing.Color.Red
            MsgBox("Password cannot be left Empty.", MsgBoxStyle.Exclamation, "No Password")

            'tt1.Show(tt1.GetToolTip(PasswordTextBox), PasswordTextBox, 4000)
            Return False
            Exit Function

        Else
            Return True
        End If

    End Function
End Class


