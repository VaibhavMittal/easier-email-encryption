Imports System

Imports FirstOultookAddin.Org.BouncyCastle.Security

Namespace Org.BouncyCastle.Crypto
    Public Interface IWrapper
        ''' <summary>The name of the algorithm this cipher implements.</summary>
        ReadOnly Property AlgorithmName() As String

        Sub Init(forWrapping As Boolean, parameters As ICipherParameters)

        Function Wrap(input As Byte(), inOff As Integer, length As Integer) As Byte()

        Function Unwrap(input As Byte(), inOff As Integer, length As Integer) As Byte()
    End Interface
End Namespace