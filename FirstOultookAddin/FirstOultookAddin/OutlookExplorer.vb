Option Strict On

Imports Outlook = Microsoft.Office.Interop.Outlook
Imports Office = Microsoft.Office.Core

Namespace $safeprojectname$
    ''' <summary>
    ''' This class tracks the state of an Outlook Explorer window for your
    ''' add-in and ensures that what happens in this window is handled correctly.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class OutlookExplorer

#Region "Instance Variables"
        Private WithEvents m_Window As Outlook.Explorer            'wrapped window object
#End Region

#Region "Events"
        Public Event Close As EventHandler
#End Region

#Region "Constructor"
        ''' <summary>
        ''' Create a new instance of the tracking class for a particular
        ''' explorer.
        ''' </summary>
        ''' <param name="Explorer">The new explorer window to monitor</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Explorer As Outlook.Explorer)
            m_Window = Explorer
        End Sub
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Event Handler for the explorer close event.
        ''' </summary>
        Private Sub OutlookExplorerWindow_Close() Handles m_Window.Close

            ' Raise the OutlookExplorer close event
            RaiseEvent Close(Me, EventArgs.Empty)
            m_Window = Nothing
        End Sub
#End Region

#Region "Properties"
        Friend ReadOnly Property Window() As Outlook.Explorer
            Get
                Return m_Window
            End Get
        End Property
#End Region

    End Class
End Namespace
