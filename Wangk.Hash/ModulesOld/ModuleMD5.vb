Imports System.IO

Public Module ModuleMD5
    ''' <summary>
    ''' 获取字符串128位MD5值
    ''' </summary>
    <Obsolete>
    Public Function GetStr128MD5(ByVal str As String) As String
        Return MD5Helper.GetStr128MD5(str)
    End Function

    ''' <summary>
    ''' 获取文件128位MD5值
    ''' </summary>
    <Obsolete>
    Public Function GetFile128MD5(ByVal filePath As String) As String
        Return MD5Helper.GetFile128MD5(filePath)
    End Function
End Module
