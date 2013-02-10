Partial Class DecryptionRibbon
    Inherits Microsoft.Office.Tools.Ribbon.RibbonBase

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub
    Public Shared currentDecryptionRibbon As DecryptionRibbon = Nothing

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New(Globals.Factory.GetRibbonFactory())

        'This call is required by the Component Designer.
        InitializeComponent()
        currentDecryptionRibbon = Me
    End Sub

    'Component overrides dispose to clean up the component list.
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

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DecryptionRibbon))
        Me.decryptEmailTab = Me.Factory.CreateRibbonTab
        Me.decryptEmail = Me.Factory.CreateRibbonGroup
        Me.decryptEmailMessage = Me.Factory.CreateRibbonButton
        Me.decryptEmailTab.SuspendLayout()
        Me.decryptEmail.SuspendLayout()
        '
        'decryptEmailTab
        '
        Me.decryptEmailTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.decryptEmailTab.Groups.Add(Me.decryptEmail)
        Me.decryptEmailTab.Label = "Decrypt Email"
        Me.decryptEmailTab.Name = "decryptEmailTab"
        '
        'decryptEmail
        '
        Me.decryptEmail.Items.Add(Me.decryptEmailMessage)
        Me.decryptEmail.Label = "Decrypt Email"
        Me.decryptEmail.Name = "decryptEmail"
        '
        'decryptEmailMessage
        '
        Me.decryptEmailMessage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.decryptEmailMessage.Enabled = False
        Me.decryptEmailMessage.Image = CType(resources.GetObject("decryptEmailMessage.Image"), System.Drawing.Image)
        Me.decryptEmailMessage.KeyTip = "D"
        Me.decryptEmailMessage.Label = "Decrypt Email"
        Me.decryptEmailMessage.Name = "decryptEmailMessage"
        Me.decryptEmailMessage.ScreenTip = "Decrypt the Email Message."
        Me.decryptEmailMessage.ShowImage = True
        Me.decryptEmailMessage.SuperTip = "Requires a Password to decrypt."
        '
        'DecryptionRibbon
        '
        Me.Name = "DecryptionRibbon"
        Me.RibbonType = "Microsoft.Outlook.Mail.Read"
        Me.Tabs.Add(Me.decryptEmailTab)
        Me.decryptEmailTab.ResumeLayout(False)
        Me.decryptEmailTab.PerformLayout()
        Me.decryptEmail.ResumeLayout(False)
        Me.decryptEmail.PerformLayout()

    End Sub

    Friend WithEvents decryptEmailTab As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents decryptEmail As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents decryptEmailMessage As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property Ribbon2() As DecryptionRibbon
        Get
            Return Me.GetRibbon(Of DecryptionRibbon)()
        End Get
    End Property
End Class
