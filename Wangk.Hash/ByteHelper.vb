''' <summary>
''' Byte辅助模块
''' </summary>
Public Class ByteHelper

    ''' <summary>
    ''' 十六进制字符串转字节数组
    ''' </summary>
    Public Shared Function Hex2Byte(ByVal HexStr As String) As Byte()

        Dim tmpArray(HexStr.Length \ 2 - 1) As Byte

        For i As Integer = 0 To tmpArray.Length - 1
            tmpArray(i) = Convert.ToInt32(HexStr.Substring(i * 2, 2), 16)
        Next i

        Return tmpArray

    End Function

    ''' <summary>
    ''' 字节数组转十六进制字符串
    ''' </summary>
    Public Shared Function Byte2Hex(ByVal bytes As Byte()) As String
        Return BitConverter.ToString(bytes).Replace("-", "")
    End Function

    ''' <summary>
    ''' 字节数组转十六进制字符串
    ''' </summary>
    Public Shared Function Byte2Hex(ByVal bytes As Byte(), ByVal length As Integer) As String

        Return BitConverter.ToString(bytes,
                                     0,
                                     If(length > bytes.Length, bytes.Length, length)).Replace("-", "")

    End Function

End Class
