Public Module ModuleBIN
    ''' <summary>
    ''' 十六进制转二进制
    ''' </summary>
    Public Function Hex2Bin(ByVal Str As String) As Byte()
        Dim tmpArray As Byte()
        ReDim tmpArray(Str.Length \ 2 - 1)

        For i As Integer = 0 To tmpArray.Length - 1
            tmpArray(i) = Val($"&H{Str(i * 2)}{Str(i * 2 + 1)}")
        Next i

        Return tmpArray
    End Function

    ''' <summary>
    ''' 二进制转十六进制字符串
    ''' </summary>
    Public Function Bin2Hex(ByVal bytes As Byte()) As String
        Return BitConverter.ToString(bytes).Replace("-", "")
    End Function

    ''' <summary>
    ''' 二进制转十六进制字符串
    ''' </summary>
    Public Function Bin2Hex(ByVal bytes As Byte(), ByVal length As Integer) As String
        Return BitConverter.ToString(bytes,
                                     0,
                                     If(length > bytes.Length, bytes.Length, length)).Replace("-", "")
    End Function

End Module
