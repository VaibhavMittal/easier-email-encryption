<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class DecryptionPasswordDialogBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DisplayDecryptedMessageButton As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.DisplayDecryptedMessageButton = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.DecryptionProgressLabel = New System.Windows.Forms.Label()
        Me.DecryptionProgressBar = New System.Windows.Forms.ProgressBar()
        Me.decryptionStatusLabel = New System.Windows.Forms.Label()
        Me.HintLabel = New System.Windows.Forms.Label()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PasswordLabel
        '
        Me.PasswordLabel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.PasswordLabel.Location = New System.Drawing.Point(320, 59)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(220, 23)
        Me.PasswordLabel.TabIndex = 2
        Me.PasswordLabel.Text = "Enter &Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(428, 59)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(220, 22)
        Me.PasswordTextBox.TabIndex = 1
        '
        'DisplayDecryptedMessageButton
        '
        Me.DisplayDecryptedMessageButton.Location = New System.Drawing.Point(547, 172)
        Me.DisplayDecryptedMessageButton.Name = "DisplayDecryptedMessageButton"
        Me.DisplayDecryptedMessageButton.Size = New System.Drawing.Size(94, 65)
        Me.DisplayDecryptedMessageButton.TabIndex = 2
        Me.DisplayDecryptedMessageButton.Text = "&Display Message"
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(650, 172)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(94, 65)
        Me.Cancel.TabIndex = 3
        Me.Cancel.Text = "&Cancel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label1.Location = New System.Drawing.Point(284, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(306, 17)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Enter password to display the secure message:"
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = Global.FirstOultookAddin.My.Resources.Resources.secure_email
        Me.LogoPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(271, 253)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'DecryptionProgressLabel
        '
        Me.DecryptionProgressLabel.AutoSize = True
        Me.DecryptionProgressLabel.Location = New System.Drawing.Point(453, 103)
        Me.DecryptionProgressLabel.Name = "DecryptionProgressLabel"
        Me.DecryptionProgressLabel.Size = New System.Drawing.Size(137, 17)
        Me.DecryptionProgressLabel.TabIndex = 7
        Me.DecryptionProgressLabel.Text = "Decryption Progress"
        '
        'DecryptionProgressBar
        '
        Me.DecryptionProgressBar.Location = New System.Drawing.Point(336, 123)
        Me.DecryptionProgressBar.Name = "DecryptionProgressBar"
        Me.DecryptionProgressBar.Size = New System.Drawing.Size(370, 23)
        Me.DecryptionProgressBar.TabIndex = 8
        '
        'decryptionStatusLabel
        '
        Me.decryptionStatusLabel.AutoSize = True
        Me.decryptionStatusLabel.Location = New System.Drawing.Point(284, 202)
        Me.decryptionStatusLabel.Name = "decryptionStatusLabel"
        Me.decryptionStatusLabel.Size = New System.Drawing.Size(0, 17)
        Me.decryptionStatusLabel.TabIndex = 9
        '
        'HintLabel
        '
        Me.HintLabel.AutoSize = True
        Me.HintLabel.Location = New System.Drawing.Point(333, 172)
        Me.HintLabel.Name = "HintLabel"
        Me.HintLabel.Size = New System.Drawing.Size(41, 17)
        Me.HintLabel.TabIndex = 10
        Me.HintLabel.Text = "Hint: "
        '
        'DecryptionPasswordDialogBox
        '
        Me.AcceptButton = Me.DisplayDecryptedMessageButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(756, 249)
        Me.Controls.Add(Me.HintLabel)
        Me.Controls.Add(Me.decryptionStatusLabel)
        Me.Controls.Add(Me.DecryptionProgressBar)
        Me.Controls.Add(Me.DecryptionProgressLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.DisplayDecryptedMessageButton)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DecryptionPasswordDialogBox"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Secure Message"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DecryptionProgressLabel As System.Windows.Forms.Label
    Friend WithEvents DecryptionProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents decryptionStatusLabel As System.Windows.Forms.Label
    Friend WithEvents HintLabel As System.Windows.Forms.Label

End Class
