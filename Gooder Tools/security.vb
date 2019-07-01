Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography
Imports System.Text

Public Class security
    Public Shared Function EncryptSHA256Managed(ByVal ClearString As String) As String
        Dim uEncode As New UnicodeEncoding()
        Dim bytClearString() As Byte = uEncode.GetBytes(ClearString)
        Dim sha As New _
        System.Security.Cryptography.SHA256Managed()
        Dim hash() As Byte = sha.ComputeHash(bytClearString)
        Return Convert.ToBase64String(hash)
    End Function
    Public Shared Function AESEncrypt(ByVal PlainText As String, ByVal Password As String, ByVal salt As String)
        Dim HashAlgorithm As String = "SHA1" 'Can be SHA1 or MD5
        Dim PasswordIterations As String = 2
        Dim InitialVector As String = "CanEncryption123" 'This should be a string of 16 ASCII characters.
        Dim KeySize As Integer = 256 'Can be 128, 192, or 256.

        If (String.IsNullOrEmpty(PlainText)) Then
            Return ""
            Exit Function
        End If
        Dim InitialVectorBytes As Byte() = Encoding.ASCII.GetBytes(InitialVector)
        Dim SaltValueBytes As Byte() = Encoding.ASCII.GetBytes(salt)
        Dim PlainTextBytes As Byte() = Encoding.UTF8.GetBytes(PlainText)
        Dim DerivedPassword As PasswordDeriveBytes = New PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations)
        Dim KeyBytes As Byte() = DerivedPassword.GetBytes(KeySize / 8)
        Dim SymmetricKey As RijndaelManaged = New RijndaelManaged()
        SymmetricKey.Mode = CipherMode.CBC

        Dim CipherTextBytes As Byte() = Nothing
        Using Encryptor As ICryptoTransform = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes)
            Using MemStream As New MemoryStream()
                Using CryptoStream As New CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write)
                    CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length)
                    CryptoStream.FlushFinalBlock()
                    CipherTextBytes = MemStream.ToArray()
                    MemStream.Close()
                    CryptoStream.Close()
                End Using
            End Using
        End Using
        SymmetricKey.Clear()
        Return Convert.ToBase64String(CipherTextBytes)
    End Function
    Public Shared Function AESDecrypt(ByVal CipherText As String, ByVal password As String, ByVal salt As String) As String
        Dim HashAlgorithm As String = "SHA1"
        Dim PasswordIterations As String = 2
        Dim InitialVector As String = "CanEncryption123"
        Dim KeySize As Integer = 256

        If (String.IsNullOrEmpty(CipherText)) Then
            Return ""
        End If
        Dim InitialVectorBytes As Byte() = Encoding.ASCII.GetBytes(InitialVector)
        Dim SaltValueBytes As Byte() = Encoding.ASCII.GetBytes(salt)
        Dim CipherTextBytes As Byte() = Convert.FromBase64String(CipherText)
        Dim DerivedPassword As PasswordDeriveBytes = New PasswordDeriveBytes(password, SaltValueBytes, HashAlgorithm, PasswordIterations)
        Dim KeyBytes As Byte() = DerivedPassword.GetBytes(KeySize / 8)
        Dim SymmetricKey As RijndaelManaged = New RijndaelManaged()
        SymmetricKey.Mode = CipherMode.CBC
        Dim PlainTextBytes As Byte() = New Byte(CipherTextBytes.Length - 1) {}

        Dim ByteCount As Integer = 0

        Using Decryptor As ICryptoTransform = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes)
            Using MemStream As MemoryStream = New MemoryStream(CipherTextBytes)
                Using CryptoStream As CryptoStream = New CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read)
                    ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length)
                    MemStream.Close()
                    CryptoStream.Close()
                End Using
            End Using
        End Using
        SymmetricKey.Clear()
        Return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount)
    End Function

    Public Shared Function generateKeys()
        developerFeatures.logger("d", "The security key was requested")
        Return EncryptSHA256Managed(AESEncrypt(getMacAddress, "secure", EncryptSHA256Managed("ok")))
        'Return "This key is secure"
    End Function

    Public Shared Function getMacAddress()
        Dim nics() As NetworkInterface =
              NetworkInterface.GetAllNetworkInterfaces
        Return nics(0).GetPhysicalAddress.ToString
    End Function
End Class
