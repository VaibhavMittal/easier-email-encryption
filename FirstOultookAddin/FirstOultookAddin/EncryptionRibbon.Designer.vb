Partial Class MyOutlookAddIn
    Inherits Microsoft.Office.Tools.Ribbon.RibbonBase

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub
    'Public Shared quickEncryptionButton As Microsoft.Office.Tools.Ribbon.RibbonButton = Nothing
    'Public Shared highSecurityEncryptionButton As Microsoft.Office.Tools.Ribbon.RibbonButton = Nothing
    Public Shared currentEncryptionRibbon As MyOutlookAddIn = Nothing
    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New(Globals.Factory.GetRibbonFactory())

        'This call is required by the Component Designer.
        InitializeComponent()

        currentEncryptionRibbon = Me

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MyOutlookAddIn))
        Me.encryptEmailTab = Me.Factory.CreateRibbonTab
        Me.encryption = Me.Factory.CreateRibbonGroup
        Me.quickEncryption = Me.Factory.CreateRibbonButton
        Me.highSecurity = Me.Factory.CreateRibbonButton
        Me.encryptEmailTab.SuspendLayout()
        Me.encryption.SuspendLayout()
        '
        'encryptEmailTab
        '
        Me.encryptEmailTab.Groups.Add(Me.encryption)
        Me.encryptEmailTab.Label = "Encrypt Email"
        Me.encryptEmailTab.Name = "encryptEmailTab"
        '
        'encryption
        '
        Me.encryption.Items.Add(Me.quickEncryption)
        Me.encryption.Items.Add(Me.highSecurity)
        Me.encryption.Label = "Encryption"
        Me.encryption.Name = "encryption"
        '
        'quickEncryption
        '
        Me.quickEncryption.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.quickEncryption.Image = CType(resources.GetObject("quickEncryption.Image"), System.Drawing.Image)
        Me.quickEncryption.KeyTip = "Q"
        Me.quickEncryption.Label = "Quick Security"
        Me.quickEncryption.Name = "quickEncryption"
        Me.quickEncryption.ScreenTip = "A quick way to send secure emails."
        Me.quickEncryption.ShowImage = True
        Me.quickEncryption.SuperTip = " Warning: Use a strong password for better security."
        '
        'highSecurity
        '
        Me.highSecurity.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.highSecurity.Image = CType(resources.GetObject("highSecurity.Image"), System.Drawing.Image)
        Me.highSecurity.KeyTip = "H"
        Me.highSecurity.Label = "High Security"
        Me.highSecurity.Name = "highSecurity"
        Me.highSecurity.ScreenTip = "Provides a highly secure message transmission."
        Me.highSecurity.ShowImage = True
        Me.highSecurity.SuperTip = " The process involves several communications from both sides and might seem a lit" & _
            "tle complex."
        '
        'MyOutlookAddIn
        '
        Me.Name = "MyOutlookAddIn"
        Me.RibbonType = "Microsoft.Outlook.Mail.Compose"
        Me.Tabs.Add(Me.encryptEmailTab)
        Me.encryptEmailTab.ResumeLayout(False)
        Me.encryptEmailTab.PerformLayout()
        Me.encryption.ResumeLayout(False)
        Me.encryption.PerformLayout()

    End Sub

    Friend WithEvents encryptEmailTab As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents encryption As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents quickEncryption As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents highSecurity As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property Ribbon1() As MyOutlookAddIn
        Get
            Return Me.GetRibbon(Of MyOutlookAddIn)()
        End Get
    End Property
End Class
