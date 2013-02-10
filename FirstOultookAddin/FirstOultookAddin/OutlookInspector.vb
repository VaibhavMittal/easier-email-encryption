Option Strict On

Imports Outlook = Microsoft.Office.Interop.Outlook
Imports Office = Microsoft.Office.Core


''' <summary>
''' This class tracks the state of an Outlook Inspector window for your
''' add-in and ensures that what happens in this window is handled correctly.
''' </summary>
''' <remarks></remarks>
Friend Class OutlookInspector

#Region "Instance Variables"
    Private WithEvents m_Window As Outlook.Inspector            'wrapped window object
    ' Use these instance variables to handle item-level events
    Private WithEvents m_Mail As Outlook.MailItem               ' wrapped MailItem
    Private WithEvents m_Appointment As Outlook.AppointmentItem ' wrapped AppointmentItem
    Private WithEvents m_Contact As Outlook.ContactItem         ' wrapped ContactItem
    Private WithEvents m_Task As Outlook.TaskItem               ' wrapped TaskItem
    ' Define other class-level item instance variables as needed
#End Region

#Region "Events"
    Public Event Close As EventHandler
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Create a new instance of the tracking class for a particular
    ''' inspector.
    ''' </summary>
    ''' <param name="Inspector">The new inspector window to monitor</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Inspector As Outlook.Inspector)
        m_Window = Inspector

        ' Hookup item-level events as needed
        ' For example, the following code hooks up PropertyChange
        ' event for a ContactItem
        'Dim olItem As OutlookItem = New OutlookItem(Inspector.CurrentItem)
        'If olItem.Class = Outlook.OlObjectClass.olContact Then
        '    m_Contact = CType(olItem.InnerObject, Outlook.ContactItem)
        'End If

    End Sub
#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Event Handler for the inspector close event.
    ''' </summary>
    Private Sub OutlookInspectorWindow_Close() Handles m_Window.Close

        ' Raise the OutlookInspector close event
        RaiseEvent Close(Me, EventArgs.Empty)

        m_Window = Nothing
    End Sub

    'Private Sub m_Contact_PropertyChange(ByVal Name As String) _
    '    Handles m_Contact.PropertyChange
    '    ' Implement PropertyChange here
    'End Sub
#End Region

#Region "Properties"
    Friend ReadOnly Property Window() As Outlook.Inspector
        Get
            Return m_Window
        End Get
    End Property
#End Region

    Private Sub m_Mail_Open(ByRef Cancel As Boolean) Handles m_Mail.Open
        Dim decryptionPasswordDialog As New DecryptionPasswordDialogBox("DoubleClick")
        decryptionPasswordDialog.Show()
    End Sub
End Class

