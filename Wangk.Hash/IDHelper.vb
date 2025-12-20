''' <summary>
''' ID辅助模块
''' </summary>
Public Class IDHelper

    ''' <summary>
    ''' 创建192位ID, 64位时间戳 + 128位GUID, Base64Url格式的32字节, 区分大小写
    ''' </summary>
    Public Shared ReadOnly Property NewID As String
        Get
            Return Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()).Reverse().Concat(Guid.NewGuid().ToByteArray()).ToArray()).Replace("+", "-").Replace("/", "_")
        End Get
    End Property

End Class
