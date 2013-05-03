Imports System.Windows.Forms
Imports System.IO

'Imports Microsoft.Office.Interop.Outlook


Public Class ThisAddIn

    Private Sub ThisAddIn_Startup(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Startup
        'MsgBox("The Encryption ad-in is STARTING up...")

        'Dim xyz As Outlook.ApplicationEvents_11_Event
        'AddHandler xyz, AddressOf MyItem_SendEventHandler
    End Sub
    Private Sub ItemSend() Handles Application.ItemSend
        Try
            'MsgBox("Hahahah")
            Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveInspector.CurrentItem, Outlook.MailItem)
            'currentItem.Body = "CHANGED"


            'Do all the Checking here



            'Checking Conditions to apply Oppurtunistic Encryption or not

            'Conditions: Check if the Email is encrypted with ECube addin or not

            ' Finding the Parent MailItem 
            Dim parentMsg As Outlook.MailItem
            parentMsg = FindParentMessage(currentItem)
            If Not parentMsg Is Nothing Then

                'parentMsg.Display()
                'Check whether msg is encryptred with ECube or not
            Else
                '                currentItem.Send()
                Exit Sub
                'MsgBox("Error Message: Parent Message cannot be found!")
            End If

            Dim pa As Outlook.PropertyAccessor
            pa = parentMsg.PropertyAccessor

            'Dim currentItemHeader As String = pa.GetProperty("http://schemas.microsoft.com/mapi/id/PR_TRANSPORT_MESSAGE_HEADERS")
            Dim addinName As String = pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version")
            Dim encryptionType As String = pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-Encryption-Type")
            If addinName.Contains("ECube") And encryptionType.Contains("Quick") Then
                Dim OEDialog As New OEDialogBox(parentMsg)
                OEDialog.ShowDialog()

                'Dim OEQuery As DialogResult = OEDialog.ShowDialog()

                'If OEQuery = DialogResult.Yes Then
                '    MessageBox.Show("OE stuff comes here...") 'All Oppurtunistic Encryption functionality goes here

                '    'Encrypt the reply message with the same key.
                'ElseIf OEQuery = DialogResult.No Then
                '    Exit Sub
                'End If

                ' Step 3: Get the encryption key and encrypt the current reply msg



                'Dim inReplyToMsgIDValue As String = _
                'pa.GetProperty("http://schemas.microsoft.com/mapi/proptag/PR_INTERNET_MESSAGE_ID") 'or use string/in-reply-to



            Else
                Exit Sub  'If NOT then continue with the normal "Send" functionality of Outlook
            End If


            'MsgBox("OE works!...Oh Yes")

            ' Display a dialog box here asking to Encrypt or Dont Encrypt the reply message.
            'Dim OEQuery As DialogResult = MessageBox.Show("Do u want to Encrypt this reply?", "Encrypt Reply", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            




        Catch ex As Exception

        End Try
    End Sub

   

    Function FindParentMessage(ByVal msg As Outlook.MailItem) As Outlook.MailItem
        Dim strFind As String
        Dim strIndex As String
        Dim fld As Outlook.MAPIFolder
        Dim itms As Outlook.Items
        Dim itm As Outlook.MailItem
        On Error Resume Next
        strIndex = Left(msg.ConversationIndex, _
                        Len(msg.ConversationIndex) - 10)
        fld = Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)
        strFind = "[ConversationTopic] = " & _
                  Chr(34) & msg.ConversationTopic & Chr(34)
        itms = fld.Items.Restrict(strFind)

        For Each itm In itms
            If itm.ConversationIndex = strIndex Then

                Return itm
                'FindParentMessage = itm
                Exit For
            End If
        Next
        fld = Nothing
        itms = Nothing
        itm = Nothing


    End Function

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
        'MsgBox("The Encryption ad-in is SHUTTING Down...")
    End Sub

    Dim decryptButton As Office.CommandBarButton


    Private Sub Application_ItemContextMenuDisplay(ByVal CommandBar As Microsoft.Office.Core.CommandBar, ByVal Selection As Microsoft.Office.Interop.Outlook.Selection) Handles Application.ItemContextMenuDisplay
        Dim item As OutlookItem

        Try
            If Selection.Count > 1 Then Exit Sub
            item = New OutlookItem(Selection(1))

            If item.Class <> Outlook.OlObjectClass.olMail Then Exit Sub

            decryptButton = CommandBar.FindControl(Tag:="DecryptContextMenuButton")

            If decryptButton Is Nothing Then

                decryptButton = CommandBar.Controls.Add(Office.MsoControlType.msoControlButton)
                decryptButton.Caption = "Decrypt Message"
                decryptButton.Tag = "DecryptContextMenuButton"
                decryptButton.TooltipText = "Decrypt the Protected Message"
                'decryptButton.BeginGroup = True

            End If

            'Check if message is encrypted with our add-in (ECube)
            Dim selectedItem As Outlook.MailItem = Application.ActiveExplorer.Selection(1)
            Dim pa As Outlook.PropertyAccessor = selectedItem.PropertyAccessor

            Dim addinVersion As String = CType(pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version"), String)
            If addinVersion.Contains("ECube") Then
                decryptButton.Enabled = True
            Else
                decryptButton.Enabled = False
            End If

            AddHandler decryptButton.Click, AddressOf decryptButton_Click

        Catch ex As Exception
            decryptButton.Enabled = False
        Finally

        End Try
    End Sub

    Private Sub Application_ContextMenuClose(ByVal ContextMenu As Microsoft.Office.Interop.Outlook.OlContextMenu) Handles Application.ContextMenuClose
        Try
            RemoveHandler decryptButton.Click, AddressOf decryptButton_Click
        Catch e As Exception
        End Try
    End Sub

    Private Sub decryptButton_Click(ByVal ctrl As Microsoft.Office.Core.CommandBarButton, ByRef CancelDefault As Boolean)
        Try
            'Dim selectedItem As Outlook.MailItem = Application.ActiveExplorer.Selection(1)

            Dim decryptionPasswordDialog As New DecryptionPasswordDialogBox("RightClickMenu")
            decryptionPasswordDialog.TopMost = True
            decryptionPasswordDialog.Show()
            Dim selectedItem As Outlook.MailItem = Application.ActiveExplorer.Selection(1)
            'selectedItem.Display()
        Catch ex As Exception

        Finally
            RemoveHandler decryptButton.Click, AddressOf decryptButton_Click
        End Try
    End Sub
    Private WithEvents m_Mail As Outlook.MailItem               ' wrapped MailItem

    Public Sub m_Mail_Open(ByRef Cancel As Boolean) Handles m_Mail.Open
        Dim decryptionPasswordDialog As New DecryptionPasswordDialogBox("DoubleClick")
        decryptionPasswordDialog.Show()
    End Sub

    '  'Custom EventHandling
    '  ' Define the Click event to use the delegate store.
    '  Public Custom Event AnyName As EventHandler
    '      AddHandler(ByVal value As EventHandler)
    '          ' Add the delegate to the Component's EventHandlerList Collection
    '          .EAddHandler("AnyNameEvent", value)
    '      End AddHandler

    '      RemoveHandler(ByVal value As EventHandler)
    '          ' Remove the delegate from the Component's EventHandlerList Collection
    '          Me.Events.RemoveHandler("AnyNameEvent", value)
    '      End RemoveHandler

    '      RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)
    '          ' Raise the event.
    '          CType(Me.Events("AnyNameEvent"), EventHandler).Invoke(sender, e)
    '      End RaiseEvent
    '  End Event

    '  ' Write the method to call the Event, and then use it as you want.
    '  Protected Sub OnAnyName(ByVal e As EventArgs)
    '      Dim anyNameHandler As EventHandler = _
    'CType(Me.Events("AnyNameEvent"), EventHandler)
    '      If (anyNameHandler IsNot Nothing) Then
    '          anyNameHandler.Invoke(Me, e)
    '      End If
    '  End Sub


    'Public Delegate Sub ApplicationEvents_11_ItemSendEventHandler(ByVal Item As Object, ByRef Cancel As Boolean)

    'Public Sub MyItem_SendEventHandler(ByVal Item As Object, ByRef Cancel As Boolean)
    '    MsgBox("HAHAHAH")
    '    'Check if the Email is encrypted with ECube addin or not
    '    Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveInspector.CurrentItem, Outlook.MailItem)


    '    Dim pa As Outlook.PropertyAccessor
    '    pa = currentItem.PropertyAccessor

    '    Dim temp As String = CType(pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version"), String)
    '    If temp.Contains("ECube") Then 'If YES then encrypt the Reply as well

    '        MsgBox("OE works!")
    '        'All Oppurtunistic Encryption functionality goes here

    '        'Encrypt the reply message with the same key.
    '    Else
    '        Exit Sub 'If NOT then continue with the normal "Send" functionality of Outlook
    '    End If
    'End Sub

End Class
