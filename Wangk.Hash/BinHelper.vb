Imports System.IO
''' <summary>
''' Bin文件辅助模块
''' </summary>
Public Class BinHelper

    ''' <summary>
    ''' Bin文件转Hex文件
    ''' </summary>
    ''' <param name="BinFilePath">Bin文件路径</param>
    ''' <param name="HexFilePath">保存的Hex文件路径</param>
    ''' <param name="MCUStartAddress">单片机起始地址</param>
    ''' <param name="RowByteCount">每行字节数</param>
    Public Shared Sub Bin2HexFile(BinFilePath As String,
                                  HexFilePath As String,
                                  MCUStartAddress As UInt32,
                                  Optional RowByteCount As Integer = 32)

        ' 头部字节数
        Dim HeadByteCount = 4

        Using binFileStream = File.OpenRead(BinFilePath)
            Using HexStreamWriter = New StreamWriter(HexFilePath, False, Text.Encoding.ASCII)

                Dim BinBytes(1024 - 1) As Byte

                ' 切换起始的高地址
                BinBytes(0) = 2
                BinBytes(3) = 4
                BinBytes(4) = MCUStartAddress >> 24 And &HFF
                BinBytes(5) = MCUStartAddress >> 16 And &HFF
                BinBytes(HeadByteCount + 2) = GetHexCheckCode(BinBytes, HeadByteCount + 2)
                HexStreamWriter.WriteLine($":{Wangk.Hash.ByteHelper.Byte2Hex(BinBytes, HeadByteCount + 2 + 1)}")

                Do
                    Dim readByteCount = binFileStream.Read(BinBytes, HeadByteCount, RowByteCount)
                    If readByteCount = 0 Then
                        Exit Do
                    End If

                    ' Bin数据
                    BinBytes(0) = readByteCount
                    BinBytes(1) = MCUStartAddress >> 8 And &HFF
                    BinBytes(2) = MCUStartAddress And &HFF
                    BinBytes(3) = 0
                    BinBytes(HeadByteCount + readByteCount) = GetHexCheckCode(BinBytes, HeadByteCount + readByteCount)
                    HexStreamWriter.WriteLine($":{Wangk.Hash.ByteHelper.Byte2Hex(BinBytes, HeadByteCount + readByteCount + 1)}")

                    ' 切换高地址
                    If (MCUStartAddress And &HFFFF0000) <> ((MCUStartAddress + RowByteCount) And &HFFFF0000) Then

                        BinBytes(0) = 2
                        BinBytes(1) = 0
                        BinBytes(2) = 0
                        BinBytes(3) = 4
                        BinBytes(4) = (MCUStartAddress + RowByteCount) >> 24 And &HFF
                        BinBytes(5) = (MCUStartAddress + RowByteCount) >> 16 And &HFF
                        BinBytes(HeadByteCount + 2) = GetHexCheckCode(BinBytes, HeadByteCount + 2)
                        HexStreamWriter.WriteLine($":{Wangk.Hash.ByteHelper.Byte2Hex(BinBytes, HeadByteCount + 2 + 1)}")

                    End If

                    MCUStartAddress += RowByteCount
                Loop

                ' 文件结束
                BinBytes(0) = 0
                BinBytes(1) = 0
                BinBytes(2) = 0
                BinBytes(3) = 1
                BinBytes(4) = GetHexCheckCode(BinBytes, HeadByteCount)
                HexStreamWriter.WriteLine($":{Wangk.Hash.ByteHelper.Byte2Hex(BinBytes, HeadByteCount + 1)}")

            End Using
        End Using

    End Sub

    Private Shared Function GetHexCheckCode(BinBytes() As Byte, length As Integer) As Byte

        Dim sum As Integer = 0
        For i001 = 0 To length - 1
            sum = (sum + BinBytes(i001)) Mod &H100
        Next

        Return (&H100 - sum) And &HFF

    End Function

End Class
