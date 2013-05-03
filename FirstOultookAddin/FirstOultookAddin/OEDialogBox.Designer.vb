<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OEDialogBox
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OEDialogBox))
        Me.HintLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.EncryptReply = New System.Windows.Forms.Button()
        Me.DontEncryptReply = New System.Windows.Forms.Button()
        Me.EncryptionProgressLabel = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HintLabel
        '
        Me.HintLabel.AutoSize = True
        Me.HintLabel.Location = New System.Drawing.Point(320, 109)
        Me.HintLabel.Name = "HintLabel"
        Me.HintLabel.Size = New System.Drawing.Size(41, 17)
        Me.HintLabel.TabIndex = 15
        Me.HintLabel.Text = "Hint: "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label1.Location = New System.Drawing.Point(284, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(463, 17)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Enter the same Secret-Phrase you used to decrypt the Parent Message:"
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(428, 74)
        Me.PasswordTextBox.MaxLength = 150
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(220, 22)
        Me.PasswordTextBox.TabIndex = 12
        '
        'PasswordLabel
        '
        Me.PasswordLabel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.PasswordLabel.Location = New System.Drawing.Point(320, 63)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(102, 44)
        Me.PasswordLabel.TabIndex = 13
        Me.PasswordLabel.Text = "&Secret Phrase"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = Global.FirstOultookAddin.My.Resources.Resources.secure_email
        Me.LogoPictureBox.Location = New System.Drawing.Point(0, 1)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(252, 206)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LogoPictureBox.TabIndex = 11
        Me.LogoPictureBox.TabStop = False
        '
        'EncryptReply
        '
        Me.EncryptReply.Location = New System.Drawing.Point(639, 151)
        Me.EncryptReply.Name = "EncryptReply"
        Me.EncryptReply.Size = New System.Drawing.Size(134, 56)
        Me.EncryptReply.TabIndex = 16
        Me.EncryptReply.Text = "Encrypt Message"
        Me.EncryptReply.UseVisualStyleBackColor = True
        '
        'DontEncryptReply
        '
        Me.DontEncryptReply.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DontEncryptReply.Location = New System.Drawing.Point(494, 151)
        Me.DontEncryptReply.Name = "DontEncryptReply"
        Me.DontEncryptReply.Size = New System.Drawing.Size(126, 56)
        Me.DontEncryptReply.TabIndex = 17
        Me.DontEncryptReply.Text = "Don't Encrypt"
        Me.DontEncryptReply.UseVisualStyleBackColor = True
        '
        'EncryptionProgressLabel
        '
        Me.EncryptionProgressLabel.AutoSize = True
        Me.EncryptionProgressLabel.Location = New System.Drawing.Point(472, 54)
        Me.EncryptionProgressLabel.Name = "EncryptionProgressLabel"
        Me.EncryptionProgressLabel.Size = New System.Drawing.Size(119, 17)
        Me.EncryptionProgressLabel.TabIndex = 18
        Me.EncryptionProgressLabel.Text = "Encryption Status"
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(119, 175)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(133, 32)
        Me.RichTextBox1.TabIndex = 19
        Me.RichTextBox1.Text = resources.GetString("RichTextBox1.Text")
        Me.RichTextBox1.Visible = False
        '
        'OEDialogBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DontEncryptReply
        Me.ClientSize = New System.Drawing.Size(785, 210)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.EncryptionProgressLabel)
        Me.Controls.Add(Me.DontEncryptReply)
        Me.Controls.Add(Me.EncryptReply)
        Me.Controls.Add(Me.HintLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OEDialogBox"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Automatic Encryption"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HintLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents EncryptReply As System.Windows.Forms.Button
    Friend WithEvents DontEncryptReply As System.Windows.Forms.Button
    Friend WithEvents EncryptionProgressLabel As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
End Class
