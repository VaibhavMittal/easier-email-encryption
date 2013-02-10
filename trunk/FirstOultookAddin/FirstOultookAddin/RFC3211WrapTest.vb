Imports System
Imports System.Text

Imports NUnit.Framework

Imports Org.BouncyCastle.Crypto.Engines
Imports Org.BouncyCastle.Crypto.Modes
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Utilities
Imports Org.BouncyCastle.Utilities.Encoders
Imports Org.BouncyCastle.Utilities.Test

Namespace Org.BouncyCastle.Crypto.Tests
    '*
    '    * Wrap Test based on RFC3211 test vectors
    '    

    <TestFixture()> _
    Public Class Rfc3211WrapTest
        Inherits SimpleTest
        ' Note: These test data assume the Rfc3211WrapEngine will call SecureRandom.NextBytes

        Private r1 As SecureRandom = FixedSecureRandom.From(New Byte() {&HC4, &H36, &HF5, &H41})

        Private r2 As SecureRandom = FixedSecureRandom.From(New Byte() {&HFA, &H6, &HA, &H45})

        Public Overrides ReadOnly Property Name() As String
            Get
                Return "RFC3211Wrap"
            End Get
        End Property

        Private Sub doWrapTest(ByVal id As Integer, ByVal engine As IBlockCipher, ByVal kek As Byte(), ByVal iv As Byte(), ByVal rand As SecureRandom, ByVal inBytes As Byte(), _
            ByVal outBytes As Byte())
            Dim wrapper As IWrapper = New Rfc3211WrapEngine(engine)

            wrapper.Init(True, New ParametersWithRandom(New ParametersWithIV(New KeyParameter(kek), iv), rand))

            Dim cText As Byte() = wrapper.Wrap(inBytes, 0, inBytes.Length)
            If Not AreEqual(cText, outBytes) Then
                Fail("failed Wrap test " + id + " expected " + Encoding.ASCII.GetString(Hex.Encode(outBytes)) + " got " + Encoding.ASCII.GetString(Hex.Encode(cText)))
            End If

            wrapper.Init(False, New ParametersWithIV(New KeyParameter(kek), iv))

            Dim pText As Byte() = wrapper.Unwrap(outBytes, 0, outBytes.Length)
            If Not AreEqual(pText, inBytes) Then
                Fail("rfailed Unwrap test " + id + " expected " + Encoding.ASCII.GetString(Hex.Encode(inBytes)) + " got " + Encoding.ASCII.GetString(Hex.Encode(pText)))
            End If
        End Sub

        Private Sub doTestCorruption()
            Dim kek As Byte() = Hex.Decode("D1DAA78615F287E6")
            Dim iv As Byte() = Hex.Decode("EFE598EF21B33D6D")

            Dim wrapper As IWrapper = New Rfc3211WrapEngine(New DesEngine())

            wrapper.Init(False, New ParametersWithIV(New KeyParameter(kek), iv))

            Dim block As Byte() = Hex.Decode("ff739D838C627C897323A2F8C436F541")
            encryptBlock(kek, iv, block)

            Try
                wrapper.Unwrap(block, 0, block.Length)

                Fail("bad length not detected")
            Catch e As InvalidCipherTextException
                If Not e.Message.Equals("wrapped key corrupted") Then
                    Fail("wrong exception on length")
                End If
            End Try

            block = Hex.Decode("08639D838C627C897323A2F8C436F541")
            doTestChecksum(kek, iv, block, wrapper)

            block = Hex.Decode("08736D838C627C897323A2F8C436F541")
            doTestChecksum(kek, iv, block, wrapper)

            block = Hex.Decode("08739D638C627C897323A2F8C436F541")
            doTestChecksum(kek, iv, block, wrapper)
        End Sub

        Private Sub doTestChecksum(ByVal kek As Byte(), ByVal iv As Byte(), ByVal block As Byte(), ByVal wrapper As IWrapper)
            encryptBlock(kek, iv, block)

            Try
                wrapper.Unwrap(block, 0, block.Length)

                Fail("bad checksum not detected")
            Catch e As InvalidCipherTextException
                If Not e.Message.Equals("wrapped key fails checksum") Then
                    Fail("wrong exception")
                End If
            End Try
        End Sub

        Private Sub encryptBlock(ByVal key As Byte(), ByVal iv As Byte(), ByVal cekBlock As Byte())
            Dim engine As IBlockCipher = New CbcBlockCipher(New DesEngine())

            engine.Init(True, New ParametersWithIV(New KeyParameter(key), iv))

            For i As Integer = 0 To cekBlock.Length - 1 Step 8
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
            Next

            For i As Integer = 0 To cekBlock.Length - 1 Step 8
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
            Next
        End Sub

        Public Overrides Sub PerformTest()
            doWrapTest(1, New DesEngine(), Hex.Decode("D1DAA78615F287E6"), Hex.Decode("EFE598EF21B33D6D"), r1, Hex.Decode("8C627C897323A2F8"), _
                Hex.Decode("B81B2565EE373CA6DEDCA26A178B0C10"))
            doWrapTest(2, New DesEdeEngine(), Hex.Decode("6A8970BF68C92CAEA84A8DF28510858607126380CC47AB2D"), Hex.Decode("BAF1CA7931213C4E"), r2, Hex.Decode("8C637D887223A2F965B566EB014B0FA5D52300A3F7EA40FFFC577203C71BAF3B"), _
                Hex.Decode("C03C514ABDB9E2C5AAC038572B5E24553876B377AAFB82ECA5A9D73F8AB143D9EC74E6CAD7DB260C"))

            doTestCorruption()

            Dim wrapper As IWrapper = New Rfc3211WrapEngine(New DesEngine())
            Dim parameters As New ParametersWithIV(New KeyParameter(New Byte(15) {}), New Byte(15) {})
            Dim buf As Byte() = New Byte(15) {}

            Try
                wrapper.Init(True, parameters)

                wrapper.Unwrap(buf, 0, buf.Length)

                Fail("failed Unwrap state test.")
                ' expected
            Catch generatedExceptionName As InvalidOperationException
            Catch e As InvalidCipherTextException
                Fail("unexpected exception: " + e, e)
            End Try

            Try
                wrapper.Init(False, parameters)

                wrapper.Wrap(buf, 0, buf.Length)

                Fail("failed Unwrap state test.")
                ' expected
            Catch generatedExceptionName As InvalidOperationException
            End Try

            '
            ' short test
            '
            Try
                wrapper.Init(False, parameters)

                wrapper.Unwrap(buf, 0, buf.Length / 2)

                Fail("failed Unwrap short test.")
                ' expected
            Catch generatedExceptionName As InvalidCipherTextException
            End Try
        End Sub

        Public Shared Sub Main(ByVal args As String())
            RunTest(New Rfc3211WrapTest())
        End Sub

        <Test()> _
        Public Sub TestFunction()
            Dim resultText As String = Perform().ToString()


        End Sub
    End Class
End Namespace