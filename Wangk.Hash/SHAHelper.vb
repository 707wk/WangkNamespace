Imports System.IO
Imports System.Security.Cryptography

Public Class SHAHelper

    ''' <summary>
    ''' 获取字符串512位SHA值
    ''' </summary>
    Public Shared Function GetStrSHA512(ByVal input As String) As String
        Dim hasher As New Security.Cryptography.SHA512CryptoServiceProvider
        Dim data As Byte() = hasher.ComputeHash(Text.Encoding.Default.GetBytes(input))

        Return BitConverter.ToString(data).Replace("-", "")
    End Function

    ''' <summary>
    ''' 获取文件512位SHA值
    ''' </summary>
    Public Shared Function GetFileSHA512(path As String) As String
        '解决无法读取只读属性文件
        Using Stream As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

            Dim Sha512 As SHA512Managed = New SHA512Managed()
            Dim bytes = Sha512.ComputeHash(Stream)

            Return BitConverter.ToString(bytes).Replace("-", "").ToLower
        End Using
    End Function

End Class
