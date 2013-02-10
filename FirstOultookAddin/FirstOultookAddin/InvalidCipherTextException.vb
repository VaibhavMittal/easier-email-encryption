Imports System

Namespace Org.BouncyCastle.Crypto
    '*
' * this exception is thrown whenever we find something we don't expect in a
' * message.
' 

    Public Class InvalidCipherTextException
        Inherits CryptoException
        '*
' * base constructor.
' 

        Public Sub New()
        End Sub

        '*
' * create a InvalidCipherTextException with the given message.
' *
' * @param message the message to be carried with the exception.
' 

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, exception As Exception)
            MyBase.New(message, exception)
        End Sub
    End Class
End Namespace
