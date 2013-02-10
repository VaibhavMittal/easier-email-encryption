Imports FirstOultookAddin
Imports System
Imports System.IO
Imports System.Security.Cryptography



Public Class AesExample

    Public Shared Sub AESTest()
        Try

            Dim original As String = "Here is some data to encrypt!"

            ' Create a new instance of the AesCryptoServiceProvider
            ' class.  This generates a new key and initialization 
            ' vector (IV).
            Using myAes As New AesCryptoServiceProvider()

                ' Encrypt the string to an array of bytes.
                Dim encrypted As Byte() = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV)

                Dim temp As String = ""
                For Each b As Byte In encrypted
                    temp += b.ToString
                Next



                Dim encoding As New System.Text.UTF8Encoding()
                Dim tempBytes As Byte() = encoding.GetBytes("00000000000000000000000000000000000000000008310364250104243215711311982420821323020768116182126221151140611122511661121187023667")

                ' Decrypt the bytes to a string.
                Dim roundtrip As String = DecryptStringFromBytes_Aes(tempBytes, myAes.Key, myAes.IV)

                'Display the original data and the decrypted data.
                MsgBox("Original:   {0}" & original)
                MsgBox("Encrypted:   {0}" & temp)
                Dim t1 As EncryptionPasswordDialogBox = EncryptionPasswordDialogBox.ActiveForm


                MsgBox("Round Trip: {0}" & roundtrip)
            End Using
        Catch e As Exception
            MsgBox("Error: {0}" & e.Message)
        End Try

    End Sub 'Main

    Shared Function EncryptStringToBytes_Aes(ByVal plainText As String, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()
        ' Check arguments.
        If plainText Is Nothing OrElse plainText.Length <= 0 Then
            Throw New ArgumentNullException("plainText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("IV")
        End If
        Dim encrypted() As Byte
        ' Create an AesCryptoServiceProvider object
        ' with the specified key and IV.
        Using aesAlg As New AesCryptoServiceProvider()

            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for encryption.
            Dim msEncrypt As New MemoryStream()
            Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                Using swEncrypt As New StreamWriter(csEncrypt)
                    'Write all data to the stream.
                    swEncrypt.Write(plainText)
                End Using
                encrypted = msEncrypt.ToArray()

            End Using
        End Using

        ' Return the encrypted bytes from the memory stream.
        Return encrypted

    End Function 'EncryptStringToBytes_Aes

    Shared Function DecryptStringFromBytes_Aes(ByVal cipherText() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As String
        ' Check arguments.
        If cipherText Is Nothing OrElse cipherText.Length <= 0 Then
            Throw New ArgumentNullException("cipherText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        ' Declare the string used to hold
        ' the decrypted text.
        Dim plaintext As String = Nothing

        ' Create an AesCryptoServiceProvider object
        ' with the specified key and IV.
        Using aesAlg As New AesCryptoServiceProvider()

            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for decryption.
            Using msDecrypt As New MemoryStream(cipherText)

                Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

                    Using srDecrypt As New StreamReader(csDecrypt)

                        ' Read the decrypted bytes from the decrypting stream
                        ' and place them in a string.
                        plaintext = srDecrypt.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using
        Return plaintext

    End Function 'DecryptStringFromBytes_Aes 

    
End Class 'AesExample

