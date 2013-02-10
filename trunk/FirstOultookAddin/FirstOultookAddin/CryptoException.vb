Imports System

Namespace Org.BouncyCastle.Crypto
	Public Class CryptoException
		Inherits Exception
		Public Sub New()
		End Sub

		Public Sub New(message As String)
			MyBase.New(message)
		End Sub

		Public Sub New(message As String, exception As Exception)
			MyBase.New(message, exception)
		End Sub
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik, @toddanglin
'Facebook: facebook.com/telerik
'=======================================================
