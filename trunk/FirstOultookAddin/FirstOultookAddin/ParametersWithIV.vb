Imports System

Namespace Org.BouncyCastle.Crypto.Parameters
    Public Class ParametersWithIV
        Implements ICipherParameters
        Private ReadOnly m_parameters As ICipherParameters
        Private ReadOnly iv As Byte()

        Public Sub New(parameters As ICipherParameters, iv As Byte())
            Me.New(parameters, iv, 0, iv.Length)
        End Sub

        Public Sub New(parameters As ICipherParameters, iv As Byte(), ivOff As Integer, ivLen As Integer)
            If parameters Is Nothing Then
                Throw New ArgumentNullException("parameters")
            End If
            If iv Is Nothing Then
                Throw New ArgumentNullException("iv")
            End If

            Me.m_parameters = parameters
            Me.iv = New Byte(ivLen - 1) {}
            Array.Copy(iv, ivOff, Me.iv, 0, ivLen)
        End Sub

        Public Function GetIV() As Byte()
            Return DirectCast(iv.Clone(), Byte())
        End Function

        Public ReadOnly Property Parameters() As ICipherParameters
            Get
                Return m_parameters
            End Get
        End Property
    End Class
End Namespace
