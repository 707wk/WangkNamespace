''' <summary>
''' Byte辅助模块
''' </summary>
Public Class ByteHelper

    ''' <summary>
    ''' 十六进制字符串转字节数组
    ''' </summary>
    Public Shared Function Hex2Byte(ByVal HexStr As String) As Byte()

        HexStr = HexStr.Replace(" ", "")

        Dim tmpArray(HexStr.Length \ 2 - 1) As Byte

        For i As Integer = 0 To tmpArray.Length - 1
            tmpArray(i) = Convert.ToInt32(HexStr.Substring(i * 2, 2), 16)
        Next i

        Return tmpArray

    End Function

    ''' <summary>
    ''' 十六进制字符串转字节数组, 最后2字节为CRC校验码
    ''' </summary>
    Public Shared Function Hex2ByteByCRC16Modbus(ByVal HexStr As String) As Byte()

        HexStr = HexStr.Replace(" ", "")

        Dim tmpArray(HexStr.Length \ 2 - 1) As Byte

        For i As Integer = 0 To tmpArray.Length - 1
            tmpArray(i) = Convert.ToInt32(HexStr.Substring(i * 2, 2), 16)
        Next i

        Dim CRCCode = CRCHelper.GetCRC16Modbus(tmpArray, tmpArray.Length - 2)

        tmpArray(tmpArray.Length - 2) = CRCCode(0)
        tmpArray(tmpArray.Length - 1) = CRCCode(1)

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

    ''' <summary>
    ''' 获取字节的位状态
    ''' </summary>
    ''' <param name="byteData">字节数据</param>
    ''' <param name="index">位序号</param>
    Public Shared Function GetBitValue(byteData As Byte, index As Integer) As Boolean

        Return byteData >> (index Mod 8) And &H1

    End Function

End Class
