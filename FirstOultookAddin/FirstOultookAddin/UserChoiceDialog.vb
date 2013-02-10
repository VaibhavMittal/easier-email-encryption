Imports Org.BouncyCastle.Cms

Public Class userChoiceDialog

    Private PBEKey As String

    Sub New(ByVal p1 As String)
        ' TODO: Complete member initialization 
        InitializeComponent()
        PBEKey = p1
    End Sub

    Private Sub doneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles doneButton.Click
        'Use Select to apply the requested function
        If saveEncryptedRB.Checked = True Then
            'Save_Encrypted_Message
            Me.Close()

        ElseIf saveDecryptedRB.Checked = True Then
            'Save_Decrypted_Message
            'Get the message from Sent Box folder decrypt it and set its body as unencrypted message and save it.
            Dim objOutlook As Outlook._Application
            objOutlook = Globals.ThisAddIn.Application
            Dim objNS As Outlook._NameSpace = objOutlook.Session
            Dim objFolder As Outlook.MAPIFolder
            objFolder = objNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

            Dim recentMessage As Outlook.MailItem = CType(objFolder.Items.GetLast(), Outlook.MailItem)

            DecryptionProcess(recentMessage, PBEKey)

            Me.Close()

        ElseIf deleteMessageRB.Checked = True Then
            'Delete_Message
            'Get the message from Sent Box folder and delete it.
            Dim objOutlook2 As Outlook._Application
            objOutlook2 = Globals.ThisAddIn.Application
            Dim objNS2 As Outlook._NameSpace = objOutlook2.Session
            Dim objFolder2 As Outlook.MAPIFolder
            objFolder2 = objNS2.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail)

            Dim recentMessage2 As Outlook.MailItem = CType(objFolder2.Items.GetFirst(), Outlook.MailItem)
            recentMessage2.Delete()

            Me.Close()
        End If

       

        

        
    End Sub

    Private Sub DecryptionProcess(ByRef currentItem As Outlook.MailItem, ByVal PBEKey As String)

        'Retrieve the Current MailMessage Details

        Dim messageBody As String
        Dim messageRecipients As String
        Dim messageCCRecipients As String
        Dim messageBCCRecipients As String
        Dim messageSubject As String
        Dim messageAttachments As Outlook.Attachments

        messageBody = currentItem.Body
        'messageRecipients = currentItem.To
        'messageCCRecipients = currentItem.CC
        'messageBCCRecipients = currentItem.BCC
        'messageSubject = currentItem.Subject
        'messageAttachments = currentItem.Attachments

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Self attempted Byte() to String conversion and vice-versa

        'Decryption Side
        'String to String() conversion using Delimiter
        Dim temp6 As String()
        temp6 = messageBody.Split("#")
        Dim arrayLength As Integer = temp6.Length - 1
        'temp6.SetValue(Nothing, arrayLength)

        'String() to Byte() retrieval conversion
        Dim dataRetrieved As Byte() = New Byte(arrayLength + 1) {}
        Dim k As Integer = 0
        For Each value In temp6
            If k < arrayLength - 1 Then
                dataRetrieved(k) = Byte.Parse(value)
                k = k + 1
            End If
        Next

        'Byte() content display code
        Dim temp2 As String = ""
        For Each value In dataRetrieved
            temp2 = temp2 & value
        Next
        'MsgBox("Retrived Byte array: " & temp2)


        ''''''''''''''''''''''''''''''''''''''''''''''''

        'Decryption Process
        'Decryption Process at Receiver's end

        'New Decryption Code
        Dim encodedData As Byte() = dataRetrieved
        Dim recipientID As New Org.BouncyCastle.Cms.RecipientID()
        Dim decodedEnvelopeData As New CmsEnvelopedData(encodedData)
        Dim recipient As PasswordRecipientInformation = decodedEnvelopeData.GetRecipientInfos().GetFirstRecipient(recipientID)

        Dim key As CmsPbeKey = New Pkcs5Scheme2Utf8PbeKey(PBEKey.ToCharArray(), recipient.KeyDerivationAlgorithm)
        Dim data2 As Byte() = recipient.GetContent(key)

        Dim abc As String = ""
        For Each temp In data2
            abc = abc & temp & " "
        Next

        Dim decryptedMessage As String = ""
        Dim enc2 As New System.Text.UTF8Encoding()
        decryptedMessage = enc2.GetString(data2)

        currentItem.Body = decryptedMessage
        currentItem.Save()

    End Sub

    Private Sub userChoiceDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()



    End Sub
End Class