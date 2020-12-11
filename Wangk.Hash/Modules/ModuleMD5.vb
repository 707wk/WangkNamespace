Imports System.IO

Public Module ModuleMD5
    ''' <summary>
    ''' 获取字符串128位MD5值
    ''' </summary>
    Public Function GetStr128MD5(ByVal input As String) As String
        Dim hasher As New Security.Cryptography.MD5CryptoServiceProvider()
        Dim data As Byte() = hasher.ComputeHash(Text.Encoding.Default.GetBytes(input))

        Return BitConverter.ToString(data).Replace("-", "")
    End Function

    ''' <summary>
    ''' 获取文件128位MD5值
    ''' </summary>
    Public Function GetFile128MD5(ByVal FilePath As String) As String
        Dim hasher As New Security.Cryptography.MD5CryptoServiceProvider()
        Dim file As New FileStream(FilePath, FileMode.Open)
        Dim data As Byte() = hasher.ComputeHash(file)
        file.Close()

        Return BitConverter.ToString(Data).Replace("-", "")
    End Function
End Module
