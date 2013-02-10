<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class userChoiceDialog
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
        Me.rememberChoiceCheckBox = New System.Windows.Forms.CheckBox()
        Me.userChoiceIntroLabel = New System.Windows.Forms.Label()
        Me.userChoiceGroupBox = New System.Windows.Forms.GroupBox()
        Me.saveEncryptedRB = New System.Windows.Forms.RadioButton()
        Me.deleteMessageRB = New System.Windows.Forms.RadioButton()
        Me.saveDecryptedRB = New System.Windows.Forms.RadioButton()
        Me.doneButton = New System.Windows.Forms.Button()
        Me.userChoiceGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'rememberChoiceCheckBox
        '
        Me.rememberChoiceCheckBox.AutoSize = True
        Me.rememberChoiceCheckBox.ForeColor = System.Drawing.SystemColors.InfoText
        Me.rememberChoiceCheckBox.Location = New System.Drawing.Point(22, 235)
        Me.rememberChoiceCheckBox.Name = "rememberChoiceCheckBox"
        Me.rememberChoiceCheckBox.Size = New System.Drawing.Size(265, 21)
        Me.rememberChoiceCheckBox.TabIndex = 3
        Me.rememberChoiceCheckBox.Text = "Do the &same with all future messages"
        Me.rememberChoiceCheckBox.UseVisualStyleBackColor = True
        '
        'userChoiceIntroLabel
        '
        Me.userChoiceIntroLabel.AutoSize = True
        Me.userChoiceIntroLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.userChoiceIntroLabel.ForeColor = System.Drawing.Color.ForestGreen
        Me.userChoiceIntroLabel.Location = New System.Drawing.Point(16, 29)
        Me.userChoiceIntroLabel.Name = "userChoiceIntroLabel"
        Me.userChoiceIntroLabel.Size = New System.Drawing.Size(387, 20)
        Me.userChoiceIntroLabel.TabIndex = 1
        Me.userChoiceIntroLabel.Text = "What would you like to do with this Sent Message?"
        '
        'userChoiceGroupBox
        '
        Me.userChoiceGroupBox.Controls.Add(Me.saveDecryptedRB)
        Me.userChoiceGroupBox.Controls.Add(Me.deleteMessageRB)
        Me.userChoiceGroupBox.Controls.Add(Me.saveEncryptedRB)
        Me.userChoiceGroupBox.Location = New System.Drawing.Point(40, 60)
        Me.userChoiceGroupBox.Name = "userChoiceGroupBox"
        Me.userChoiceGroupBox.Size = New System.Drawing.Size(536, 169)
        Me.userChoiceGroupBox.TabIndex = 2
        Me.userChoiceGroupBox.TabStop = False
        '
        'saveEncryptedRB
        '
        Me.saveEncryptedRB.AutoSize = True
        Me.saveEncryptedRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.saveEncryptedRB.Location = New System.Drawing.Point(6, 21)
        Me.saveEncryptedRB.Name = "saveEncryptedRB"
        Me.saveEncryptedRB.Size = New System.Drawing.Size(306, 21)
        Me.saveEncryptedRB.TabIndex = 0
        Me.saveEncryptedRB.TabStop = True
        Me.saveEncryptedRB.Text = "Save the &Encrypted message in Sent folder."
        Me.saveEncryptedRB.UseVisualStyleBackColor = True
        '
        'deleteMessageRB
        '
        Me.deleteMessageRB.AutoSize = True
        Me.deleteMessageRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.deleteMessageRB.Location = New System.Drawing.Point(6, 119)
        Me.deleteMessageRB.Name = "deleteMessageRB"
        Me.deleteMessageRB.Size = New System.Drawing.Size(241, 21)
        Me.deleteMessageRB.TabIndex = 2
        Me.deleteMessageRB.TabStop = True
        Me.deleteMessageRB.Text = "Delete the message permanently."
        Me.deleteMessageRB.UseVisualStyleBackColor = True
        '
        'saveDecryptedRB
        '
        Me.saveDecryptedRB.AutoSize = True
        Me.saveDecryptedRB.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.saveDecryptedRB.Location = New System.Drawing.Point(6, 70)
        Me.saveDecryptedRB.Name = "saveDecryptedRB"
        Me.saveDecryptedRB.Size = New System.Drawing.Size(307, 21)
        Me.saveDecryptedRB.TabIndex = 1
        Me.saveDecryptedRB.TabStop = True
        Me.saveDecryptedRB.Text = "Save the &Decrypted message in Sent folder."
        Me.saveDecryptedRB.UseVisualStyleBackColor = True
        '
        'doneButton
        '
        Me.doneButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.doneButton.Location = New System.Drawing.Point(432, 235)
        Me.doneButton.Name = "doneButton"
        Me.doneButton.Size = New System.Drawing.Size(144, 41)
        Me.doneButton.TabIndex = 4
        Me.doneButton.Text = "&Done"
        Me.doneButton.UseVisualStyleBackColor = False
        '
        'userChoiceDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(605, 288)
        Me.Controls.Add(Me.doneButton)
        Me.Controls.Add(Me.userChoiceGroupBox)
        Me.Controls.Add(Me.userChoiceIntroLabel)
        Me.Controls.Add(Me.rememberChoiceCheckBox)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "userChoiceDialog"
        Me.Text = "Secured Message Sent Successfully"
        Me.TopMost = True
        Me.userChoiceGroupBox.ResumeLayout(False)
        Me.userChoiceGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rememberChoiceCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents userChoiceIntroLabel As System.Windows.Forms.Label
    Friend WithEvents userChoiceGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents saveDecryptedRB As System.Windows.Forms.RadioButton
    Friend WithEvents deleteMessageRB As System.Windows.Forms.RadioButton
    Friend WithEvents saveEncryptedRB As System.Windows.Forms.RadioButton
    Friend WithEvents doneButton As System.Windows.Forms.Button
End Class
