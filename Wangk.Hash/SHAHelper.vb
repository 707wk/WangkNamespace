Imports System.IO
Imports System.Security.Cryptography
''' <summary>
''' 哈希计算辅助模块
''' </summary>
Public Class SHAHelper

    ''' <summary>
    ''' 获取字符串512位SHA值
    ''' </summary>
    Public Shared Function GetStrSHA512(ByVal str As String) As String

        If String.IsNullOrWhiteSpace(str) Then
            Return Nothing
        End If

        Dim hasher As New Security.Cryptography.SHA512CryptoServiceProvider
        Dim data As Byte() = hasher.ComputeHash(Text.Encoding.Default.GetBytes(str))

        Return BitConverter.ToString(data).Replace("-", "")

    End Function

    ''' <summary>
    ''' 获取文件512位SHA值
    ''' </summary>
    Public Shared Function GetFileSHA512(filePath As String) As String

        If Not IO.File.Exists(filePath) Then
            Return Nothing
        End If

        Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

            Dim Sha512 As SHA512Managed = New SHA512Managed()
            Dim bytes = Sha512.ComputeHash(fs)

            Return BitConverter.ToString(bytes).Replace("-", "").ToLower
        End Using

    End Function

End Class
