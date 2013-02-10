Imports System

Imports FirstOultookAddin.Org.BouncyCastle.Crypto.InvalidCipherTextException

Imports FirstOultookAddin.Org.BouncyCastle.Crypto.Modes
Imports FirstOultookAddin.Org.BouncyCastle.Crypto.Parameters
Imports FirstOultookAddin.Org.BouncyCastle.Security

Namespace Org.BouncyCastle.Crypto.Engines
    '*
' * an implementation of the RFC 3211 Key Wrap
' * Specification.
' 

    Public Class Rfc3211WrapEngine
        Implements IWrapper

        Private engine As CbcBlockCipher
        Private param As ParametersWithIV
        Private forWrapping As Boolean
        Private rand As SecureRandom

        Public Sub New(ByVal engine As IBlockCipher)
            Me.engine = New CbcBlockCipher(engine)
        End Sub

        Public Sub Init(ByVal forWrapping As Boolean, ByVal param As ICipherParameters) Implements IWrapper.Init
            Me.forWrapping = forWrapping

            If TypeOf param Is ParametersWithRandom Then
                Dim p As ParametersWithRandom = DirectCast(param, ParametersWithRandom)

                Me.rand = p.Random
                Me.param = DirectCast(p.Parameters, ParametersWithIV)
            Else
                If forWrapping Then
                    rand = New SecureRandom()
                End If

                Me.param = DirectCast(param, ParametersWithIV)
            End If
        End Sub

        Public ReadOnly Property AlgorithmName() As String Implements IWrapper.AlgorithmName
            Get
                Return engine.GetUnderlyingCipher().AlgorithmName + "/RFC3211Wrap"
            End Get
        End Property

        Public Function Wrap(ByVal inBytes As Byte(), ByVal inOff As Integer, ByVal inLen As Integer) As Byte() Implements IWrapper.Wrap
            If Not forWrapping Then
                Throw New InvalidOperationException("not set for wrapping")
            End If

            engine.Init(True, param)

            Dim blockSize As Integer = engine.GetBlockSize()
            Dim cekBlock As Byte()

            If inLen + 4 < blockSize * 2 Then
                cekBlock = New Byte(blockSize * 2 - 1) {}
            Else
                cekBlock = New Byte(If((inLen + 4) Mod blockSize = 0, inLen + 4, ((inLen + 4) / blockSize + 1) * blockSize) - 1) {}
            End If

            cekBlock(0) = CByte(inLen)
            cekBlock(1) = CByte(Not inBytes(inOff))
            cekBlock(2) = CByte(Not inBytes(inOff + 1))
            cekBlock(3) = CByte(Not inBytes(inOff + 2))

            Array.Copy(inBytes, inOff, cekBlock, 4, inLen)

            rand.NextBytes(cekBlock, inLen + 4, cekBlock.Length - inLen - 4)

            Dim i As Integer = 0
            While i < cekBlock.Length
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
                i += blockSize
            End While

            Dim i As Integer = 0
            While i < cekBlock.Length
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
                i += blockSize
            End While

            Return cekBlock
        End Function

        Public Function Unwrap(ByVal inBytes As Byte(), ByVal inOff As Integer, ByVal inLen As Integer) As Byte() Implements IWrapper.Unwrap
            If forWrapping Then
                Throw New InvalidOperationException("not set for unwrapping")
            End If

            Dim blockSize As Integer = engine.GetBlockSize()

            If inLen < 2 * blockSize Then
                Throw New InvalidCipherTextException("input too short")
            End If

            Dim cekBlock As Byte() = New Byte(inLen - 1) {}
            Dim iv As Byte() = New Byte(blockSize - 1) {}

            Array.Copy(inBytes, inOff, cekBlock, 0, inLen)
            Array.Copy(inBytes, inOff, iv, 0, iv.Length)

            engine.Init(False, New ParametersWithIV(param.Parameters, iv))

            Dim i As Integer = blockSize
            While i < cekBlock.Length
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
                i += blockSize
            End While

            Array.Copy(cekBlock, cekBlock.Length - iv.Length, iv, 0, iv.Length)

            engine.Init(False, New ParametersWithIV(param.Parameters, iv))

            engine.ProcessBlock(cekBlock, 0, cekBlock, 0)

            engine.Init(False, param)

            Dim i As Integer = 0
            While i < cekBlock.Length
                engine.ProcessBlock(cekBlock, i, cekBlock, i)
                i += blockSize
            End While

            If (cekBlock(0) And &HFF) > cekBlock.Length - 4 Then
                Throw New InvalidCipherTextException("wrapped key corrupted")
            End If

            Dim key As Byte() = New Byte((cekBlock(0) And &HFF) - 1) {}

            Array.Copy(cekBlock, 4, key, 0, cekBlock(0))

            ' Note: Using constant time comparison
            Dim nonEqual As Integer = 0
            Dim i As Integer = 0
            While i <> 3
                Dim check As Byte = CByte(Not cekBlock(1 + i))
                nonEqual = nonEqual Or (check Xor key(i))
                i += 1
            End While

            If nonEqual <> 0 Then
                Throw New InvalidCipherTextException("wrapped key fails checksum")
            End If

            Return key
        End Function



    End Class
End Namespace
