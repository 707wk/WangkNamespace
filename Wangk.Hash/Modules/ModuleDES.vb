Public Module ModuleDES
#Region "DES"
    ''' <summary>
    ''' DES加密
    ''' </summary>
    ''' <param name="SourceStr"></param>
    ''' <param name="myKey">8个字符</param>
    ''' <param name="myIV">8个字符</param>
    ''' <returns></returns>
    Public Function EncryptDES(ByVal SourceStr As String,
                               ByVal myKey As String,
                               Optional ByVal myIV As String = "00000000") As String
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider 'DES算法
        Dim inputByteArray As Byte()
        inputByteArray = System.Text.Encoding.Default.GetBytes(SourceStr)
        des.Key = System.Text.Encoding.UTF8.GetBytes(myKey) 'myKey DES用8个字符，TripleDES要24个字符
        des.IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
        Dim ms As New System.IO.MemoryStream
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
        Dim sw As New System.IO.StreamWriter(cs)
        sw.Write(SourceStr)
        sw.Flush()
        cs.FlushFinalBlock()
        ms.Flush()
        Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
    End Function

    ''' <summary>
    ''' DES解密
    ''' </summary>
    ''' <param name="SourceStr"></param>
    ''' <param name="myKey">8个字符</param>
    ''' <param name="myIV">8个字符</param>
    ''' <returns></returns>
    Public Function DecryptDES(ByVal SourceStr As String,
                               ByVal myKey As String,
                               Optional ByVal myIV As String = "00000000") As String
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider 'DES算法
        des.Key = System.Text.Encoding.UTF8.GetBytes(myKey) 'myKey DES用8个字符，TripleDES要24个字符
        des.IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
        Dim buffer As Byte() = Convert.FromBase64String(SourceStr)
        Dim ms As New System.IO.MemoryStream(buffer)
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read)
        Dim sr As New System.IO.StreamReader(cs)
        Return sr.ReadToEnd()
    End Function
#End Region

#Region "TripleDES"
    ''' <summary>
    ''' TripleDES加密
    ''' </summary>
    ''' <param name="SourceStr"></param>
    ''' <param name="myKey">24个字符</param>
    ''' <param name="myIV">24个字符</param>
    ''' <returns></returns>
    Public Function EncryptTripleDES(ByVal SourceStr As String,
                                     Optional ByVal myKey As String = "",
                                     Optional ByVal myIV As String = "00000000000000000000000000000000") As String
        Dim DES As New System.Security.Cryptography.TripleDESCryptoServiceProvider 'TripleDES算法
        Dim inputByteArray As Byte()
        inputByteArray = System.Text.Encoding.Default.GetBytes(SourceStr)
        DES.Key = System.Text.Encoding.UTF8.GetBytes(myKey) 'myKey DES用8个字符，TripleDES要24个字符
        DES.IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
        Dim ms As New System.IO.MemoryStream
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, DES.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
        Dim sw As New System.IO.StreamWriter(cs)
        sw.Write(SourceStr)
        sw.Flush()
        cs.FlushFinalBlock()
        ms.Flush()
        Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
    End Function

    ''' <summary>
    ''' TripleDES解密
    ''' </summary>
    ''' <param name="SourceStr"></param>
    ''' <param name="myKey">24个字符</param>
    ''' <param name="myIV">24个字符</param>
    ''' <returns></returns>
    Public Function DecryptTripleDES(ByVal SourceStr As String,
                                     Optional ByVal myKey As String = "",
                                     Optional ByVal myIV As String = "00000000000000000000000000000000") As String
        Dim DES As New System.Security.Cryptography.TripleDESCryptoServiceProvider 'TripleDES算法
        DES.Key = System.Text.Encoding.UTF8.GetBytes(myKey) 'myKey DES用8个字符，TripleDES要24个字符
        DES.IV = System.Text.Encoding.UTF8.GetBytes(myIV) 'myIV DES用8个字符，TripleDES要24个字符
        Dim buffer As Byte() = Convert.FromBase64String(SourceStr)
        Dim ms As New System.IO.MemoryStream(buffer)
        Dim cs As New System.Security.Cryptography.CryptoStream(ms, DES.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read)
        Dim sr As New System.IO.StreamReader(cs)
        Return sr.ReadToEnd()
    End Function
#End Region

End Module
