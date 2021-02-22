Imports System.Security.Cryptography
Imports System.Text

Public Class RSAHelper

#Region "RSA加密"
    ''' <summary>
    ''' RSA加密
    ''' </summary>
    ''' <param name="Data">加密字符串</param>
    ''' <param name="PublicKey">公钥</param>
    ''' <returns></returns>
    Public Shared Function RSAEncrypt(Data As String, PublicKey As String) As String
        Dim tmpRSA As New RSACryptoServiceProvider
        '导入公钥
        tmpRSA.FromXmlString(PublicKey)
        '加密
        Dim encryptData As Byte() = tmpRSA.Encrypt(Encoding.UTF8.GetBytes(Data), True)
        '生成Base64字符串
        Return Convert.ToBase64String(encryptData)
    End Function
#End Region

#Region "RSA解密"
    ''' <summary>
    ''' RSA解密
    ''' </summary>
    ''' <param name="Data">解密字符串</param>
    ''' <param name="PrivateKey">私钥</param>
    ''' <returns></returns>
    Public Shared Function RSADecrypt(Data As String, PrivateKey As String) As String
        Dim tmpRSA As New RSACryptoServiceProvider
        '导入私钥
        tmpRSA.FromXmlString(PrivateKey)
        '解密
        Dim decryptData As Byte() = tmpRSA.Decrypt(Convert.FromBase64String(Data), True)
        '转换成字符串
        Return Encoding.UTF8.GetString(decryptData)
    End Function
#End Region

End Class
