<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class EncryptionPasswordDialogBox
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
    Friend WithEvents ConfirmPasswordLabel As System.Windows.Forms.Label
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ConfirmPasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SendEncryptedMessageButton As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.ConfirmPasswordLabel = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.ConfirmPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.SendEncryptedMessageButton = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.HintLabel = New System.Windows.Forms.Label()
        Me.HintTextBox = New System.Windows.Forms.TextBox()
        Me.NoteLabel = New System.Windows.Forms.Label()
        Me.IntroLabel = New System.Windows.Forms.Label()
        Me.EncryptionProgressBar = New System.Windows.Forms.ProgressBar()
        Me.EncryptionProgressLabel = New System.Windows.Forms.Label()
        Me.encryptionStatusLabel = New System.Windows.Forms.Label()
        Me.ICTimeTest = New System.Windows.Forms.Button()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.ButtonPanelInitial = New System.Windows.Forms.Panel()
        Me.UserChoicePanel = New System.Windows.Forms.Panel()
        Me.doneButton = New System.Windows.Forms.Button()
        Me.userChoiceGroupBox = New System.Windows.Forms.GroupBox()
        Me.saveDecryptedRB = New System.Windows.Forms.RadioButton()
        Me.deleteMessageRB = New System.Windows.Forms.RadioButton()
        Me.saveEncryptedRB = New System.Windows.Forms.RadioButton()
        Me.userChoiceIntroLabel = New System.Windows.Forms.Label()
        Me.TopPanel = New System.Windows.Forms.Panel()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ButtonPanelInitial.SuspendLayout()
        Me.UserChoicePanel.SuspendLayout()
        Me.userChoiceGroupBox.SuspendLayout()
        Me.TopPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'PasswordLabel
        '
        Me.PasswordLabel.AutoSize = True
        Me.PasswordLabel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.PasswordLabel.Location = New System.Drawing.Point(4, 35)
        Me.PasswordLabel.Margin = New System.Windows.Forms.Padding(5)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(136, 17)
        Me.PasswordLabel.TabIndex = 0
        Me.PasswordLabel.Text = "Enter a &Password* : "
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ConfirmPasswordLabel
        '
        Me.ConfirmPasswordLabel.AutoSize = True
        Me.ConfirmPasswordLabel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.ConfirmPasswordLabel.Location = New System.Drawing.Point(3, 71)
        Me.ConfirmPasswordLabel.Margin = New System.Windows.Forms.Padding(5)
        Me.ConfirmPasswordLabel.Name = "ConfirmPasswordLabel"
        Me.ConfirmPasswordLabel.Size = New System.Drawing.Size(134, 17)
        Me.ConfirmPasswordLabel.TabIndex = 2
        Me.ConfirmPasswordLabel.Text = "&Confirm Password* :"
        Me.ConfirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(148, 36)
        Me.PasswordTextBox.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(197, 22)
        Me.PasswordTextBox.TabIndex = 1
        '
        'ConfirmPasswordTextBox
        '
        Me.ConfirmPasswordTextBox.Location = New System.Drawing.Point(148, 77)
        Me.ConfirmPasswordTextBox.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox"
        Me.ConfirmPasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.ConfirmPasswordTextBox.Size = New System.Drawing.Size(197, 22)
        Me.ConfirmPasswordTextBox.TabIndex = 2
        '
        'SendEncryptedMessageButton
        '
        Me.SendEncryptedMessageButton.Location = New System.Drawing.Point(537, 52)
        Me.SendEncryptedMessageButton.Name = "SendEncryptedMessageButton"
        Me.SendEncryptedMessageButton.Size = New System.Drawing.Size(139, 59)
        Me.SendEncryptedMessageButton.TabIndex = 4
        Me.SendEncryptedMessageButton.Text = "&Send Encrypted Message"
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(434, 52)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(88, 54)
        Me.Cancel.TabIndex = 5
        Me.Cancel.Text = "&Cancel"
        '
        'HintLabel
        '
        Me.HintLabel.AutoSize = True
        Me.HintLabel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.HintLabel.Location = New System.Drawing.Point(4, 118)
        Me.HintLabel.Margin = New System.Windows.Forms.Padding(5)
        Me.HintLabel.Name = "HintLabel"
        Me.HintLabel.Size = New System.Drawing.Size(106, 17)
        Me.HintLabel.TabIndex = 6
        Me.HintLabel.Text = "Password &Hint: "
        Me.HintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'HintTextBox
        '
        Me.HintTextBox.Location = New System.Drawing.Point(148, 118)
        Me.HintTextBox.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.HintTextBox.Name = "HintTextBox"
        Me.HintTextBox.Size = New System.Drawing.Size(282, 22)
        Me.HintTextBox.TabIndex = 3
        '
        'NoteLabel
        '
        Me.NoteLabel.Font = New System.Drawing.Font("Arial Rounded MT Bold", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NoteLabel.ForeColor = System.Drawing.Color.SaddleBrown
        Me.NoteLabel.Location = New System.Drawing.Point(11, 47)
        Me.NoteLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.NoteLabel.Name = "NoteLabel"
        Me.NoteLabel.Size = New System.Drawing.Size(396, 48)
        Me.NoteLabel.TabIndex = 8
        Me.NoteLabel.Text = " NOTE: The hint will be sent in plaintext." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please proceed only after composing" & _
            " the whole message."
        '
        'IntroLabel
        '
        Me.IntroLabel.AutoSize = True
        Me.IntroLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IntroLabel.ForeColor = System.Drawing.Color.ForestGreen
        Me.IntroLabel.Location = New System.Drawing.Point(8, 8)
        Me.IntroLabel.Margin = New System.Windows.Forms.Padding(5)
        Me.IntroLabel.Name = "IntroLabel"
        Me.IntroLabel.Size = New System.Drawing.Size(329, 17)
        Me.IntroLabel.TabIndex = 9
        Me.IntroLabel.Text = "Enter the following details to encrypt your message"
        '
        'EncryptionProgressBar
        '
        Me.EncryptionProgressBar.Location = New System.Drawing.Point(334, 191)
        Me.EncryptionProgressBar.Name = "EncryptionProgressBar"
        Me.EncryptionProgressBar.Size = New System.Drawing.Size(430, 30)
        Me.EncryptionProgressBar.TabIndex = 10
        '
        'EncryptionProgressLabel
        '
        Me.EncryptionProgressLabel.AutoSize = True
        Me.EncryptionProgressLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EncryptionProgressLabel.ForeColor = System.Drawing.Color.DarkViolet
        Me.EncryptionProgressLabel.Location = New System.Drawing.Point(472, 171)
        Me.EncryptionProgressLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.EncryptionProgressLabel.Name = "EncryptionProgressLabel"
        Me.EncryptionProgressLabel.Size = New System.Drawing.Size(155, 17)
        Me.EncryptionProgressLabel.TabIndex = 11
        Me.EncryptionProgressLabel.Text = "Encryption Progress"
        '
        'encryptionStatusLabel
        '
        Me.encryptionStatusLabel.AutoSize = True
        Me.encryptionStatusLabel.Font = New System.Drawing.Font("Arial", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.encryptionStatusLabel.Location = New System.Drawing.Point(439, 228)
        Me.encryptionStatusLabel.Name = "encryptionStatusLabel"
        Me.encryptionStatusLabel.Size = New System.Drawing.Size(0, 16)
        Me.encryptionStatusLabel.TabIndex = 12
        '
        'ICTimeTest
        '
        Me.ICTimeTest.Location = New System.Drawing.Point(766, 45)
        Me.ICTimeTest.Name = "ICTimeTest"
        Me.ICTimeTest.Size = New System.Drawing.Size(112, 98)
        Me.ICTimeTest.TabIndex = 13
        Me.ICTimeTest.Text = "iteration counter time test for 128 bit key"
        Me.ICTimeTest.UseVisualStyleBackColor = True
        Me.ICTimeTest.Visible = False
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = Global.FirstOultookAddin.My.Resources.Resources.secure_email
        Me.LogoPictureBox.InitialImage = Global.FirstOultookAddin.My.Resources.Resources.secure_email
        Me.LogoPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(200, 377)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'ButtonPanelInitial
        '
        Me.ButtonPanelInitial.Controls.Add(Me.NoteLabel)
        Me.ButtonPanelInitial.Controls.Add(Me.Cancel)
        Me.ButtonPanelInitial.Controls.Add(Me.SendEncryptedMessageButton)
        Me.ButtonPanelInitial.Location = New System.Drawing.Point(206, 247)
        Me.ButtonPanelInitial.Name = "ButtonPanelInitial"
        Me.ButtonPanelInitial.Size = New System.Drawing.Size(688, 130)
        Me.ButtonPanelInitial.TabIndex = 14
        '
        'UserChoicePanel
        '
        Me.UserChoicePanel.BackColor = System.Drawing.SystemColors.Control
        Me.UserChoicePanel.Controls.Add(Me.doneButton)
        Me.UserChoicePanel.Controls.Add(Me.userChoiceGroupBox)
        Me.UserChoicePanel.Controls.Add(Me.userChoiceIntroLabel)
        Me.UserChoicePanel.Enabled = False
        Me.UserChoicePanel.Location = New System.Drawing.Point(206, 409)
        Me.UserChoicePanel.Name = "UserChoicePanel"
        Me.UserChoicePanel.Size = New System.Drawing.Size(688, 130)
        Me.UserChoicePanel.TabIndex = 15
        Me.UserChoicePanel.Visible = False
        '
        'doneButton
        '
        Me.doneButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.doneButton.Location = New System.Drawing.Point(537, 62)
        Me.doneButton.Name = "doneButton"
        Me.doneButton.Size = New System.Drawing.Size(139, 52)
        Me.doneButton.TabIndex = 5
        Me.doneButton.Text = "&Done"
        Me.doneButton.UseVisualStyleBackColor = False
        '
        'userChoiceGroupBox
        '
        Me.userChoiceGroupBox.Controls.Add(Me.saveDecryptedRB)
        Me.userChoiceGroupBox.Controls.Add(Me.deleteMessageRB)
        Me.userChoiceGroupBox.Controls.Add(Me.saveEncryptedRB)
        Me.userChoiceGroupBox.Location = New System.Drawing.Point(28, 35)
        Me.userChoiceGroupBox.Name = "userChoiceGroupBox"
        Me.userChoiceGroupBox.Size = New System.Drawing.Size(362, 101)
        Me.userChoiceGroupBox.TabIndex = 3
        Me.userChoiceGroupBox.TabStop = False
        '
        'saveDecryptedRB
        '
        Me.saveDecryptedRB.AutoSize = True
        Me.saveDecryptedRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.saveDecryptedRB.Location = New System.Drawing.Point(20, 36)
        Me.saveDecryptedRB.Name = "saveDecryptedRB"
        Me.saveDecryptedRB.Size = New System.Drawing.Size(307, 21)
        Me.saveDecryptedRB.TabIndex = 1
        Me.saveDecryptedRB.TabStop = True
        Me.saveDecryptedRB.Text = "Save the &Decrypted message in Sent folder."
        Me.saveDecryptedRB.UseVisualStyleBackColor = True
        '
        'deleteMessageRB
        '
        Me.deleteMessageRB.AutoSize = True
        Me.deleteMessageRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.deleteMessageRB.Location = New System.Drawing.Point(20, 63)
        Me.deleteMessageRB.Name = "deleteMessageRB"
        Me.deleteMessageRB.Size = New System.Drawing.Size(241, 21)
        Me.deleteMessageRB.TabIndex = 2
        Me.deleteMessageRB.TabStop = True
        Me.deleteMessageRB.Text = "Delete the message permanently."
        Me.deleteMessageRB.UseVisualStyleBackColor = True
        '
        'saveEncryptedRB
        '
        Me.saveEncryptedRB.AutoSize = True
        Me.saveEncryptedRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.saveEncryptedRB.Location = New System.Drawing.Point(20, 9)
        Me.saveEncryptedRB.Name = "saveEncryptedRB"
        Me.saveEncryptedRB.Size = New System.Drawing.Size(306, 21)
        Me.saveEncryptedRB.TabIndex = 0
        Me.saveEncryptedRB.TabStop = True
        Me.saveEncryptedRB.Text = "Save the &Encrypted message in Sent folder."
        Me.saveEncryptedRB.UseVisualStyleBackColor = True
        '
        'userChoiceIntroLabel
        '
        Me.userChoiceIntroLabel.AutoSize = True
        Me.userChoiceIntroLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.userChoiceIntroLabel.ForeColor = System.Drawing.Color.ForestGreen
        Me.userChoiceIntroLabel.Location = New System.Drawing.Point(11, 11)
        Me.userChoiceIntroLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.userChoiceIntroLabel.Name = "userChoiceIntroLabel"
        Me.userChoiceIntroLabel.Size = New System.Drawing.Size(325, 17)
        Me.userChoiceIntroLabel.TabIndex = 2
        Me.userChoiceIntroLabel.Text = "What would you like to do with this Sent Message?"
        '
        'TopPanel
        '
        Me.TopPanel.Controls.Add(Me.IntroLabel)
        Me.TopPanel.Controls.Add(Me.PasswordLabel)
        Me.TopPanel.Controls.Add(Me.ConfirmPasswordLabel)
        Me.TopPanel.Controls.Add(Me.PasswordTextBox)
        Me.TopPanel.Controls.Add(Me.ConfirmPasswordTextBox)
        Me.TopPanel.Controls.Add(Me.HintLabel)
        Me.TopPanel.Controls.Add(Me.HintTextBox)
        Me.TopPanel.Location = New System.Drawing.Point(216, 12)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Padding = New System.Windows.Forms.Padding(5)
        Me.TopPanel.Size = New System.Drawing.Size(544, 156)
        Me.TopPanel.TabIndex = 16
        '
        'BackgroundWorker1
        '
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'EncryptionPasswordDialogBox
        '
        Me.AcceptButton = Me.SendEncryptedMessageButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(894, 565)
        Me.Controls.Add(Me.TopPanel)
        Me.Controls.Add(Me.UserChoicePanel)
        Me.Controls.Add(Me.ButtonPanelInitial)
        Me.Controls.Add(Me.ICTimeTest)
        Me.Controls.Add(Me.encryptionStatusLabel)
        Me.Controls.Add(Me.EncryptionProgressLabel)
        Me.Controls.Add(Me.EncryptionProgressBar)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EncryptionPasswordDialogBox"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Password Based Encryption"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ButtonPanelInitial.ResumeLayout(False)
        Me.UserChoicePanel.ResumeLayout(False)
        Me.UserChoicePanel.PerformLayout()
        Me.userChoiceGroupBox.ResumeLayout(False)
        Me.userChoiceGroupBox.PerformLayout()
        Me.TopPanel.ResumeLayout(False)
        Me.TopPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HintLabel As System.Windows.Forms.Label
    Friend WithEvents HintTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NoteLabel As System.Windows.Forms.Label
    Friend WithEvents IntroLabel As System.Windows.Forms.Label
    Friend WithEvents EncryptionProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents EncryptionProgressLabel As System.Windows.Forms.Label
    Friend WithEvents encryptionStatusLabel As System.Windows.Forms.Label
    Friend WithEvents ICTimeTest As System.Windows.Forms.Button
    Friend WithEvents ButtonPanelInitial As System.Windows.Forms.Panel
    Friend WithEvents UserChoicePanel As System.Windows.Forms.Panel
    Friend WithEvents userChoiceIntroLabel As System.Windows.Forms.Label
    Friend WithEvents userChoiceGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents saveDecryptedRB As System.Windows.Forms.RadioButton
    Friend WithEvents deleteMessageRB As System.Windows.Forms.RadioButton
    Friend WithEvents saveEncryptedRB As System.Windows.Forms.RadioButton
    Friend WithEvents doneButton As System.Windows.Forms.Button
    Friend WithEvents TopPanel As System.Windows.Forms.Panel
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
