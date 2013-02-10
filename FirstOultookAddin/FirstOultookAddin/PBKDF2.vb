
Imports System
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Crypto.Digests
Imports Org.BouncyCastle.Crypto.Macs
Imports Org.BouncyCastle.Math
Imports Org.BouncyCastle.Security

Namespace PBKDF2_PKCS5
    Class PBKDF2

        Private ReadOnly hMac As IMac = New HMac(New Sha1Digest())

        Private Sub F(ByVal P As Byte(), ByVal S As Byte(), ByVal c As Integer, ByVal iBuf As Byte(), ByVal outBytes As Byte(), ByVal outOff As Integer)
            Dim state As Byte() = New Byte(hMac.GetMacSize() - 1) {}
            Dim param As ICipherParameters = New KeyParameter(P)

            hMac.Init(param)

            If S IsNot Nothing Then
                hMac.BlockUpdate(S, 0, S.Length)
            End If

            hMac.BlockUpdate(iBuf, 0, iBuf.Length)

            hMac.DoFinal(state, 0)

            Array.Copy(state, 0, outBytes, outOff, state.Length)

            Dim count As Integer = 1
            While count <> c
                hMac.Init(param)
                hMac.BlockUpdate(state, 0, state.Length)
                hMac.DoFinal(state, 0)

                Dim j As Integer = 0
                While j <> state.Length
                    outBytes(outOff + j) = outBytes(outOff + j) Xor state(j)
                    j += 1
                End While
                count += 1
            End While
        End Sub

        Private Sub IntToOctet(ByVal Buffer As Byte(), ByVal i As Integer)
            Buffer(0) = CByte(CUInt(i) >> 24)
            Buffer(1) = CByte(CUInt(i) >> 16)
            Buffer(2) = CByte(CUInt(i) >> 8)
            Buffer(3) = CByte(i)
        End Sub

        ' Use this function to retrieve a derived key.
        ' dkLen is in octets, how much bytes you want when the function to return.
        ' mPassword is the password converted to bytes.
        ' mSalt is the salt converted to bytes
        ' mIterationCount is the how much iterations you want to perform. 


        Public Function GenerateDerivedKey(ByVal dkLen As Integer, ByVal mPassword As Byte(), ByVal mSalt As Byte(), ByVal mIterationCount As Integer) As Byte()
            Dim hLen As Integer = hMac.GetMacSize()
            Dim l As Integer = (dkLen + hLen - 1) / hLen
            Dim iBuf As Byte() = New Byte(3) {}
            Dim outBytes As Byte() = New Byte(l * hLen - 1) {}

            For i As Integer = 1 To l
                IntToOctet(iBuf, i)

                F(mPassword, mSalt, mIterationCount, iBuf, outBytes, (i - 1) * hLen)
            Next

            'By this time outBytes will contain the derived key + more bytes.
            ' According to the PKCS #5 v2.0: Password-Based Cryptography Standard (www.truecrypt.org/docs/pkcs5v2-0.pdf) 
            ' we have to "extract the first dkLen octets to produce a derived key".

            'I am creating a byte array with the size of dkLen and then using
            'Buffer.BlockCopy to copy ONLY the dkLen amount of bytes to it
            ' And finally returning it :D

            Dim output As Byte() = New Byte(dkLen - 1) {}

            Buffer.BlockCopy(outBytes, 0, output, 0, dkLen)

            Return output
        End Function


    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik, @toddanglin
'Facebook: facebook.com/telerik
'=======================================================


