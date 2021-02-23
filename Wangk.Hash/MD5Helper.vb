Imports System.IO

Public Class MD5Helper

    ''' <summary>
    ''' 获取字符串128位MD5值
    ''' </summary>
    Public Shared Function GetStr128MD5(ByVal str As String) As String

        If String.IsNullOrWhiteSpace(str) Then
            Return Nothing
        End If

        Dim hasher As New Security.Cryptography.MD5CryptoServiceProvider()
        Dim data As Byte() = hasher.ComputeHash(Text.Encoding.Default.GetBytes(str))

        Return BitConverter.ToString(data).Replace("-", "")

    End Function

    ''' <summary>
    ''' 获取文件128位MD5值
    ''' </summary>
    Public Shared Function GetFile128MD5(ByVal filePath As String) As String

        If Not IO.File.Exists(filePath) Then
            Return Nothing
        End If

        Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

            Dim hasher As New Security.Cryptography.MD5CryptoServiceProvider()
            Dim data As Byte() = hasher.ComputeHash(fs)

            Return BitConverter.ToString(data).Replace("-", "")
        End Using

    End Function

End Class
