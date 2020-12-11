''' <summary>
''' 为运行事件提供数据
''' </summary>
Public Interface WorkEventArgs

    ''' <summary>
    ''' 传递的参数
    ''' </summary>
    ReadOnly Property Args As Object

    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Property Result As Object

    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    Sub Write(msg As String)

End Interface
