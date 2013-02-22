Imports Microsoft.Office.Tools.Ribbon

Public Class DecryptionRibbon


    Private Sub DecryptionRibbon_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Close
        Try

            decryptionPasswordDialog.Close()
            ' Me.Dispose()

        Catch ex As Exception
            ' Me.Dispose()
        End Try
    End Sub

    Dim decryptionPasswordDialog As DecryptionPasswordDialogBox = Nothing
    Private Sub Ribbon2_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load
        Try

            Dim currentItem As Outlook.MailItem = CType(Globals.ThisAddIn.Application.ActiveExplorer.Selection(1), Outlook.MailItem)

            Dim pa As Outlook.PropertyAccessor
            pa = currentItem.PropertyAccessor

            Dim temp As String = CType(pa.GetProperty("http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/X-PBE-Version"), String)
            If temp.Contains("ECube") Then

                decryptEmailTab.Visible = True

                decryptionPasswordDialog = New DecryptionPasswordDialogBox("DoubleClick")

                decryptionPasswordDialog.Show()
                decryptionPasswordDialog.TopMost = True
                decryptionPasswordDialog.PasswordTextBox.Focus()

            End If

        Catch ex As Exception
            
        End Try

    End Sub

    Private Sub decryptEmailMessage_Click(ByVal sender As System.Object, ByVal e As Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs) Handles decryptEmailMessage.Click

        Dim decryptionPasswordDialog As New DecryptionPasswordDialogBox("DoubleClick")
        decryptionPasswordDialog.Show()


    End Sub
End Class
