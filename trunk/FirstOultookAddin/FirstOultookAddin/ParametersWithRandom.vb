Imports System

Imports FirstOultookAddin.Org.BouncyCastle.Security

Namespace Org.BouncyCastle.Crypto.Parameters
    Public Class ParametersWithRandom
        Implements ICipherParameters
        Private ReadOnly m_parameters As ICipherParameters
        Private ReadOnly m_random As SecureRandom

        Public Sub New(parameters As ICipherParameters, random As SecureRandom)
            If parameters Is Nothing Then
                Throw New ArgumentNullException("random")
            End If
            If random Is Nothing Then
                Throw New ArgumentNullException("random")
            End If

            Me.m_parameters = parameters
            Me.m_random = random
        End Sub

        Public Sub New(parameters As ICipherParameters)
            Me.New(parameters, New SecureRandom())
        End Sub

        <Obsolete("Use Random property instead")> _
        Public Function GetRandom() As SecureRandom
            Return Random
        End Function

        Public ReadOnly Property Random() As SecureRandom
            Get
                Return m_random
            End Get
        End Property

        Public ReadOnly Property Parameters() As ICipherParameters
            Get
                Return m_parameters
            End Get
        End Property
    End Class
End Namespace