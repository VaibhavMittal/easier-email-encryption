Imports System

Namespace Org.BouncyCastle.Crypto
    ''' <remarks>Base interface for a symmetric key block cipher.</remarks>
    Public Interface IBlockCipher
        ''' <summary>The name of the algorithm this cipher implements.</summary>
        ReadOnly Property AlgorithmName() As String

        ''' <summary>Initialise the cipher.</summary>
        ''' <param name="forEncryption">Initialise for encryption if true, for decryption if false.</param>
        ''' <param name="parameters">The key or other data required by the cipher.</param>
        Sub Init(forEncryption As Boolean, parameters As ICipherParameters)

        ''' <returns>The block size for this cipher, in bytes.</returns>
        Function GetBlockSize() As Integer

        ''' <summary>Indicates whether this cipher can handle partial blocks.</summary>
        ReadOnly Property IsPartialBlockOkay() As Boolean

        ''' <summary>Process a block.</summary>
        ''' <param name="inBuf">The input buffer.</param>
        ''' <param name="inOff">The offset into <paramref>inBuf</paramref> that the input block begins.</param>
        ''' <param name="outBuf">The output buffer.</param>
        ''' <param name="outOff">The offset into <paramref>outBuf</paramref> to write the output block.</param>
        ''' <exception cref="DataLengthException">If input block is wrong size, or outBuf too small.</exception>
        ''' <returns>The number of bytes processed and produced.</returns>
        Function ProcessBlock(inBuf As Byte(), inOff As Integer, outBuf As Byte(), outOff As Integer) As Integer

        ''' <summary>
        ''' Reset the cipher to the same state as it was after the last init (if there was one).
        ''' </summary>
        Sub Reset()
    End Interface
End Namespace