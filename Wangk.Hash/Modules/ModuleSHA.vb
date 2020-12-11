Public Module ModuleSHA
    ''' <summary>
    ''' 获取字符串512位SHA值
    ''' </summary>
    Public Function GetStrSHA512(ByVal input As String) As String
        Dim hasher As New Security.Cryptography.SHA512CryptoServiceProvider
        Dim data As Byte() = hasher.ComputeHash(Text.Encoding.Default.GetBytes(input))

        Return BitConverter.ToString(data).Replace("-", "")
    End Function
End Module
