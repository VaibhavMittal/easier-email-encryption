Imports System
Imports FirstOultookAddin.Org.BouncyCastle.Crypto.Parameters


Namespace Org.BouncyCastle.Crypto.Modes
    '*
    ' * implements Cipher-Block-Chaining (CBC) mode on top of a simple cipher.
    ' 

    Public Class CbcBlockCipher
        Implements IBlockCipher
        Private IV As Byte(), cbcV As Byte(), cbcNextV As Byte()
        Private blockSize As Integer
        Private cipher As IBlockCipher
        Private encrypting As Boolean

        '*
        ' * Basic constructor.
        ' *
        ' * @param cipher the block cipher to be used as the basis of chaining.
        ' 

        Public Sub New(ByVal cipher As IBlockCipher)
            Me.cipher = cipher
            Me.blockSize = cipher.GetBlockSize()

            Me.IV = New Byte(blockSize - 1) {}
            Me.cbcV = New Byte(blockSize - 1) {}
            Me.cbcNextV = New Byte(blockSize - 1) {}
        End Sub

        '*
        ' * return the underlying block cipher that we are wrapping.
        ' *
        ' * @return the underlying block cipher that we are wrapping.
        ' 

        Public Function GetUnderlyingCipher() As IBlockCipher
            Return cipher
        End Function

        '*
        ' * Initialise the cipher and, possibly, the initialisation vector (IV).
        ' * If an IV isn't passed as part of the parameter, the IV will be all zeros.
        ' *
        ' * @param forEncryption if true the cipher is initialised for
        ' * encryption, if false for decryption.
        ' * @param param the key and other data required by the cipher.
        ' * @exception ArgumentException if the parameters argument is
        ' * inappropriate.
        ' 

        Public Sub Init(ByVal forEncryption As Boolean, ByVal parameters As ICipherParameters)
            Me.encrypting = forEncryption

            If TypeOf parameters Is ParametersWithIV Then
                Dim ivParam As ParametersWithIV = DirectCast(parameters, ParametersWithIV)
                Dim iv__1 As Byte() = ivParam.GetIV()

                If iv__1.Length <> blockSize Then
                    Throw New ArgumentException("initialisation vector must be the same length as block size")
                End If

                Array.Copy(iv__1, 0, IV, 0, iv__1.Length)

                parameters = ivParam.Parameters
            End If

            Reset()

            cipher.Init(encrypting, parameters)
        End Sub

        '*
        ' * return the algorithm name and mode.
        ' *
        ' * @return the name of the underlying algorithm followed by "/CBC".
        ' 

        Public ReadOnly Property AlgorithmName() As String
            Get
                Return cipher.AlgorithmName + "/CBC"
            End Get
        End Property

        Public ReadOnly Property IsPartialBlockOkay() As Boolean
            Get
                Return False
            End Get
        End Property

        '*
        ' * return the block size of the underlying cipher.
        ' *
        ' * @return the block size of the underlying cipher.
        ' 

        Public Function GetBlockSize() As Integer
            Return cipher.GetBlockSize()
        End Function

        '*
        ' * Process one block of input from the array in and write it to
        ' * the out array.
        ' *
        ' * @param in the array containing the input data.
        ' * @param inOff offset into the in array the data starts at.
        ' * @param out the array the output data will be copied into.
        ' * @param outOff the offset into the out array the output will start at.
        ' * @exception DataLengthException if there isn't enough data in in, or
        ' * space in out.
        ' * @exception InvalidOperationException if the cipher isn't initialised.
        ' * @return the number of bytes processed and produced.
        ' 

        Public Function ProcessBlock(ByVal input As Byte(), ByVal inOff As Integer, ByVal output As Byte(), ByVal outOff As Integer) As Integer
            Return If((encrypting), EncryptBlock(input, inOff, output, outOff), DecryptBlock(input, inOff, output, outOff))
        End Function

        '*
        ' * reset the chaining vector back to the IV and reset the underlying
        ' * cipher.
        ' 

        Public Sub Reset()
            Array.Copy(IV, 0, cbcV, 0, IV.Length)
            Array.Clear(cbcNextV, 0, cbcNextV.Length)

            cipher.Reset()
        End Sub

        '*
        ' * Do the appropriate chaining step for CBC mode encryption.
        ' *
        ' * @param in the array containing the data to be encrypted.
        ' * @param inOff offset into the in array the data starts at.
        ' * @param out the array the encrypted data will be copied into.
        ' * @param outOff the offset into the out array the output will start at.
        ' * @exception DataLengthException if there isn't enough data in in, or
        ' * space in out.
        ' * @exception InvalidOperationException if the cipher isn't initialised.
        ' * @return the number of bytes processed and produced.
        ' 

        Private Function EncryptBlock(ByVal input As Byte(), ByVal inOff As Integer, ByVal outBytes As Byte(), ByVal outOff As Integer) As Integer
            If (inOff + blockSize) > input.Length Then
                Throw New DataLengthException("input buffer too short")
            End If

            '
            ' * XOR the cbcV and the input,
            ' * then encrypt the cbcV
            ' 

            For i As Integer = 0 To blockSize - 1
                cbcV(i) = cbcV(i) Xor input(inOff + i)
            Next

            Dim length As Integer = cipher.ProcessBlock(cbcV, 0, outBytes, outOff)

            '
            ' * copy ciphertext to cbcV
            ' 

            Array.Copy(outBytes, outOff, cbcV, 0, cbcV.Length)

            Return length
        End Function

        '*
        ' * Do the appropriate chaining step for CBC mode decryption.
        ' *
        ' * @param in the array containing the data to be decrypted.
        ' * @param inOff offset into the in array the data starts at.
        ' * @param out the array the decrypted data will be copied into.
        ' * @param outOff the offset into the out array the output will start at.
        ' * @exception DataLengthException if there isn't enough data in in, or
        ' * space in out.
        ' * @exception InvalidOperationException if the cipher isn't initialised.
        ' * @return the number of bytes processed and produced.
        ' 

        Private Function DecryptBlock(ByVal input As Byte(), ByVal inOff As Integer, ByVal outBytes As Byte(), ByVal outOff As Integer) As Integer
            If (inOff + blockSize) > input.Length Then
                Throw New DataLengthException("input buffer too short")
            End If

            Array.Copy(input, inOff, cbcNextV, 0, blockSize)

            Dim length As Integer = cipher.ProcessBlock(input, inOff, outBytes, outOff)

            '
            ' * XOR the cbcV and the output
            ' 

            For i As Integer = 0 To blockSize - 1
                outBytes(outOff + i) = outBytes(outOff + i) Xor cbcV(i)
            Next

            '
            ' * swap the back up buffer into next position
            ' 

            Dim tmp As Byte()

            tmp = cbcV
            cbcV = cbcNextV
            cbcNextV = tmp

            Return length
        End Function
    End Class

End Namespace
