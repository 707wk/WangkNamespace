''' <summary>
''' ID辅助类
''' </summary>
Public Class IDHelper

    ''' <summary>
    ''' 创建192位ID
    ''' </summary>
    Public Shared ReadOnly Property NewID As String
        Get
            Return $"{DateTime.Now:yyyyMMddHHmmssff}{Guid.NewGuid().ToString().Replace("-", "")}"
        End Get
    End Property
End Class
