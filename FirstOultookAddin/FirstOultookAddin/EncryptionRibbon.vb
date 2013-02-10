Imports Microsoft.Office.Tools.Ribbon
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Office.Interop

Public Class MyOutlookAddIn

  
    Private Sub MyOutlookAddIn_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Close
        'quickEncryptionPasswordDialogBox.Close()
    End Sub

    Private Sub Ribbon1_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load
        'MsgBox("Ribon_Loading...")
        highSecurity.Enabled = False
        quickEncryption.Enabled = True
    End Sub

    Dim quickEncryptionPasswordDialogBox As EncryptionPasswordDialogBox

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs) Handles quickEncryption.Click

        quickEncryptionPasswordDialogBox = New EncryptionPasswordDialogBox()
        quickEncryptionPasswordDialogBox.Show()
        'quickEncryption.Enabled = False

    End Sub

    Private Sub highSecurity_Click(ByVal sender As System.Object, ByVal e As Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs) Handles highSecurity.Click

        Dim highSecurityEncryptionPasswordDialogBox As New EncryptionPasswordDialogBox()
        highSecurityEncryptionPasswordDialogBox.Show()

    End Sub
End Class
