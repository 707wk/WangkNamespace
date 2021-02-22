Imports System.Security.Cryptography
Imports System.Text

Public Module ModuleRSA
#Region "RSA加密"
    ''' <summary>
    ''' RSA加密
    ''' </summary>
    <Obsolete>
    Public Function RSAEncrypt(Data As String, PublicKey As String) As String
        Return RSAHelper.RSAEncrypt(Data, PublicKey)
    End Function
#End Region

#Region "RSA解密"
    ''' <summary>
    ''' RSA解密
    ''' </summary>
    <Obsolete>
    Public Function RSADecrypt(Data As String, PrivateKey As String) As String
        Return RSAHelper.RSADecrypt(Data, PrivateKey)
    End Function
#End Region
End Module
