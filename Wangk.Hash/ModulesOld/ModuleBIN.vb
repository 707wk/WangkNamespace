Public Module ModuleBIN
    ''' <summary>
    ''' 十六进制转二进制
    ''' </summary>
    <Obsolete>
    Public Function Hex2Bin(ByVal Str As String) As Byte()
        Return BINHelper.Hex2Bin(Str)
    End Function

    ''' <summary>
    ''' 二进制转十六进制字符串
    ''' </summary>
    <Obsolete>
    Public Function Bin2Hex(ByVal bytes As Byte()) As String
        Return BINHelper.Bin2Hex(bytes)
    End Function

    ''' <summary>
    ''' 二进制转十六进制字符串
    ''' </summary>
    <Obsolete>
    Public Function Bin2Hex(ByVal bytes As Byte(), ByVal length As Integer) As String
        Return BINHelper.Bin2Hex(bytes, length)
    End Function

End Module
