''' <summary>
''' 为后台运行提供数据
''' </summary>
Public Interface BackgroundWorkEventArgs
    Inherits IWorkEventArgs

    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Property IsCancel As Boolean

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
