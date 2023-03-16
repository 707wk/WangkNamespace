''' <summary>
''' CRC校验辅助模块
''' </summary>
Public Class CRCHelper

    ''' <summary>
    ''' CRC16查找表高字节
    ''' </summary>
    Private Shared ReadOnly CRC16TABLE_HI() As Byte = {
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41,
        &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40}

    ''' <summary>
    ''' CRC16查找表低字节
    ''' </summary>
    Private Shared ReadOnly CRC16TABLE_LO() As Byte = {
        &H0, &HC0, &HC1, &H1, &HC3, &H3, &H2, &HC2, &HC6, &H6, &H7, &HC7, &H5, &HC5, &HC4, &H4,
        &HCC, &HC, &HD, &HCD, &HF, &HCF, &HCE, &HE, &HA, &HCA, &HCB, &HB, &HC9, &H9, &H8, &HC8,
        &HD8, &H18, &H19, &HD9, &H1B, &HDB, &HDA, &H1A, &H1E, &HDE, &HDF, &H1F, &HDD, &H1D, &H1C, &HDC,
        &H14, &HD4, &HD5, &H15, &HD7, &H17, &H16, &HD6, &HD2, &H12, &H13, &HD3, &H11, &HD1, &HD0, &H10,
        &HF0, &H30, &H31, &HF1, &H33, &HF3, &HF2, &H32, &H36, &HF6, &HF7, &H37, &HF5, &H35, &H34, &HF4,
        &H3C, &HFC, &HFD, &H3D, &HFF, &H3F, &H3E, &HFE, &HFA, &H3A, &H3B, &HFB, &H39, &HF9, &HF8, &H38,
        &H28, &HE8, &HE9, &H29, &HEB, &H2B, &H2A, &HEA, &HEE, &H2E, &H2F, &HEF, &H2D, &HED, &HEC, &H2C,
        &HE4, &H24, &H25, &HE5, &H27, &HE7, &HE6, &H26, &H22, &HE2, &HE3, &H23, &HE1, &H21, &H20, &HE0,
        &HA0, &H60, &H61, &HA1, &H63, &HA3, &HA2, &H62, &H66, &HA6, &HA7, &H67, &HA5, &H65, &H64, &HA4,
        &H6C, &HAC, &HAD, &H6D, &HAF, &H6F, &H6E, &HAE, &HAA, &H6A, &H6B, &HAB, &H69, &HA9, &HA8, &H68,
        &H78, &HB8, &HB9, &H79, &HBB, &H7B, &H7A, &HBA, &HBE, &H7E, &H7F, &HBF, &H7D, &HBD, &HBC, &H7C,
        &HB4, &H74, &H75, &HB5, &H77, &HB7, &HB6, &H76, &H72, &HB2, &HB3, &H73, &HB1, &H71, &H70, &HB0,
        &H50, &H90, &H91, &H51, &H93, &H53, &H52, &H92, &H96, &H56, &H57, &H97, &H55, &H95, &H94, &H54,
        &H9C, &H5C, &H5D, &H9D, &H5F, &H9F, &H9E, &H5E, &H5A, &H9A, &H9B, &H5B, &H99, &H59, &H58, &H98,
        &H88, &H48, &H49, &H89, &H4B, &H8B, &H8A, &H4A, &H4E, &H8E, &H8F, &H4F, &H8D, &H4D, &H4C, &H8C,
        &H44, &H84, &H85, &H45, &H87, &H47, &H46, &H86, &H82, &H42, &H43, &H83, &H41, &H81, &H80, &H40}

    ''' <summary>
    ''' 获取字节数组的CRC16校验[旧]
    ''' </summary>
    <Obsolete>
    Public Shared Function GetCRC16ModbusOld(ByVal array As Byte(), ByVal length As Integer) As UShort

        Dim crc As UShort = &HFFFF
        For i As Integer = 0 To length - 1
            crc = crc Xor array(i)
            For j As Integer = 0 To 7 Step 1
                If (crc And &H1) > 0 Then
                    crc = crc >> 1
                    crc = crc Xor &HA001
                Else
                    crc = crc >> 1
                End If
            Next j
        Next i

        Return crc

    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验[旧]
    ''' </summary>
    <Obsolete>
    Public Shared Function GetCRC16ModbusOld(ByVal array As Byte()) As UShort

        Dim crc As UShort = &HFFFF
        For i As Integer = 0 To array.Length - 1
            crc = crc Xor array(i)
            For j As Integer = 0 To 7 Step 1
                If (crc And &H1) > 0 Then
                    crc = crc >> 1
                    crc = crc Xor &HA001
                Else
                    crc = crc >> 1
                End If
            Next j
        Next i

        Return crc

    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验
    ''' </summary>
    Public Shared Function GetCRC16Modbus(ByVal array As Byte(), ByVal length As Integer) As Byte()

        Dim hight As UShort = &HFF
        Dim low As UShort = &HFF

        For i As Integer = 0 To length - 1
            Dim Index As Byte = low Xor array(i)
            low = hight Xor CRC16TABLE_HI(Index)
            hight = CRC16TABLE_LO(Index)
        Next

        Return BitConverter.GetBytes(hight << 8 Or low)

    End Function

    ''' <summary>
    ''' 获取字节数组的CRC16校验
    ''' </summary>
    Public Shared Function GetCRC16Modbus(ByVal array As Byte()) As Byte()

        Return GetCRC16Modbus(array, array.Length)

    End Function

    ''' <summary>
    ''' 校验CRC16校验码
    ''' </summary>
    Public Shared Function CheckCRC16Modbus(ByVal array As Byte(), ByVal length As Integer) As Boolean

        ' 校验回复结果
        Dim CRCCode = Wangk.Hash.CRCHelper.GetCRC16Modbus(array, length - 2)

        If array(length - 2) <> CRCCode(0) OrElse
            array(length - 1) <> CRCCode(1) Then
            ' 校验失败
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 校验CRC16校验码
    ''' </summary>
    Public Shared Function CheckCRC16Modbus(ByVal array As Byte()) As Boolean

        Return CheckCRC16Modbus(array, array.Length)

    End Function

End Class
