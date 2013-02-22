
'Imports Microsoft.Office.Interop.Outlook


Public Class ThisAddIn

    Private Sub ThisAddIn_Startup(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Startup
        'MsgBox("The Encryption ad-in is STARTING up...")
    End Sub

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
                decryptButton.TooltipText = "Decrypt the Password Protected Message"
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
End Class
