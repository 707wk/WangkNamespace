''' <summary>
''' 请求结果
''' </summary>
Public Class ResultMsg
    ''' <summary>
    ''' 请求处理状态
    ''' 200: 请求处理成功
    ''' 404: 请求处理失败
    ''' </summary>
    Public Code As Integer = 200

    ''' <summary>
    ''' 请求处理消息
    ''' </summary>
    Public Message As String = "操作成功"

    ''' <summary>
    ''' 返回的实体数据
    ''' </summary>
    Public Data As Object

End Class
