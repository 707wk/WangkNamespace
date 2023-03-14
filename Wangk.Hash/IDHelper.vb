''' <summary>
''' ID辅助模块
''' </summary>
Public Class IDHelper

    ''' <summary>
    ''' 创建192位ID
    ''' </summary>
    Public Shared ReadOnly Property NewID As String
        Get
            Return $"{Date.Now:yyyyMMddHHmmssff}{Guid.NewGuid().ToString().Replace("-", "")}"
        End Get
    End Property

End Class
