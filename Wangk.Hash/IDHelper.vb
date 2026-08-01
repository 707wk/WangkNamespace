''' <summary>
''' ID辅助模块
''' </summary>
Public Class IDHelper

    ''' <summary>
    ''' 创建 RFC 9562 UUID v7 (128位, 48位毫秒时间戳在前), 32位小写十六进制
    ''' 字符串字典序与创建时间序一致, 可直接用于按字符串排序
    ''' </summary>
    Public Shared ReadOnly Property NewID As String
        Get
            Return CreateUuidV7()
        End Get
    End Property

    ''' <summary>
    ''' 生成 UUID v7 (兼容 netstandard2.0, 不依赖 .NET 9 的 Guid.CreateVersion7)
    ''' </summary>
    Private Shared Function CreateUuidV7() As String
        Dim bytes(15) As Byte

        ' 48位毫秒时间戳 (Big-Endian, 高字节在前)
        Dim unixMs As Long = (DateTime.UtcNow.Ticks - 621355968000000000L) \ 10000L
        bytes(0) = CByte((unixMs >> 40) And &HFF)
        bytes(1) = CByte((unixMs >> 32) And &HFF)
        bytes(2) = CByte((unixMs >> 24) And &HFF)
        bytes(3) = CByte((unixMs >> 16) And &HFF)
        bytes(4) = CByte((unixMs >> 8) And &HFF)
        bytes(5) = CByte(unixMs And &HFF)

        ' 随机 10 字节 (版本/变体位在下方修正)
        Using rng As System.Security.Cryptography.RandomNumberGenerator =
            System.Security.Cryptography.RandomNumberGenerator.Create()
            rng.GetBytes(bytes, 6, 10)
        End Using

        bytes(6) = CByte((bytes(6) And &HF) Or &H70)   ' 版本 7
        bytes(8) = CByte((bytes(8) And &H3F) Or &H80)   ' 变体 10

        ' 按字节序输出 hex (netstandard2.0 无 Convert.ToHexString, 手工拼接)
        Dim sb As New System.Text.StringBuilder(32)
        For Each b As Byte In bytes
            sb.Append(b.ToString("x2"))
        Next
        Return sb.ToString()
    End Function

End Class
