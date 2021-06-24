Public Interface IBackgroundWorkEventArgs

    ''' <summary>
    ''' 传递的参数
    ''' </summary>
    Property Args As Object

    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Property Result As Object

    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Property IsCancel As Boolean

    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    Overloads Sub Write(msg As String)

    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    ''' <param name="percentProgress">当前进度 0-100</param>
    Overloads Sub Write(msg As String, percentProgress As Integer)

    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="percentProgress">当前进度 0-100</param>
    Overloads Sub Write(percentProgress As Integer)

End Interface
