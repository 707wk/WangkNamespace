Public Module ModuleCRC

    ''' <summary>
    ''' 获取字节数组的CRC16校验[旧]
    ''' </summary>
    <Obsolete>
    Public Function GetCRC16ModbusOld(ByVal array As Byte(), ByVal length As Integer) As UShort
        Return CRCHelper.GetCRC16ModbusOld(array, length)
    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验[旧]
    ''' </summary>
    <Obsolete>
    Public Function GetCRC16ModbusOld(ByVal array As Byte()) As UShort
        Return CRCHelper.GetCRC16ModbusOld(array)
    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验
    ''' </summary>
    <Obsolete>
    Public Function GetCRC16Modbus(ByVal array As Byte(), ByVal length As Integer) As UShort
        Return CRCHelper.GetCRC16Modbus(array, length)
    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验
    ''' </summary>
    <Obsolete>
    Public Function GetCRC16Modbus(ByVal array As Byte()) As UShort
        Return CRCHelper.GetCRC16Modbus(array)
    End Function
End Module
