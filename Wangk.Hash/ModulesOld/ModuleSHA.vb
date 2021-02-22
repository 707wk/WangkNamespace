Public Module ModuleSHA
    ''' <summary>
    ''' 获取字符串512位SHA值
    ''' </summary>
    <Obsolete>
    Public Function GetStrSHA512(ByVal str As String) As String
        Return SHAHelper.GetStrSHA512(str)
    End Function

    ''' <summary>
    ''' 获取文件512位SHA值
    ''' </summary>
    <Obsolete>
    Public Function GetFileSHA512(path As String) As String
        Return SHAHelper.GetFileSHA512(path)
    End Function

End Module
