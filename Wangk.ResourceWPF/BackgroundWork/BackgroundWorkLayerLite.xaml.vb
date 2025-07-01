Public Class BackgroundWorkLayerLite
    Implements IBackgroundWorkEventArgs

    ''' <summary>
    ''' 后台触发事件
    ''' </summary>
    Private BackgroundWorkAction As Action(Of IBackgroundWorkEventArgs)

    Public _Error As Exception

    Public Property Args As Object Implements IBackgroundWorkEventArgs.Args

    Public Property Result As Object Implements IBackgroundWorkEventArgs.Result

    Public Property IsCancel As Boolean Implements IBackgroundWorkEventArgs.IsCancel
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Msg As String Implements IBackgroundWorkEventArgs.Msg
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    ''' <summary>
    ''' 发生的错误
    ''' </summary>
    Public ReadOnly Property [Error] As Exception
        Get
            Return _Error
        End Get
    End Property

    Private IsRunWorkerCompleted = True

    Public Property Owner As ContentControl

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    ''' <param name="args">传入的参数</param>
    Public Async Function Run(backgroundWorkAction As Action(Of IBackgroundWorkEventArgs),
                         Optional args As Object = Nothing) As Task

        Do While Not IsRunWorkerCompleted
            Await Task.Delay(100)
        Loop
        IsRunWorkerCompleted = False

        Me.BackgroundWorkAction = backgroundWorkAction
        Me.Args = args
        Me.Result = Nothing
        Me._Error = Nothing

        ' 适配 VSTO
        Dispatcher.Invoke(Threading.DispatcherPriority.Normal,
                               Sub()

                                   Me.Visibility = Visibility.Visible

                               End Sub)

        Try
            Await Task.Run(Sub()
                               backgroundWorkAction(Me)
                           End Sub)
        Catch ex As Exception
            _Error = ex
        End Try

        IsRunWorkerCompleted = True

        ' 适配 VSTO
        Dispatcher.Invoke(Threading.DispatcherPriority.Normal,
                               Sub()

                                   Me.Visibility = Visibility.Collapsed

                               End Sub)

    End Function

    Public Sub Write(msg As String) Implements IBackgroundWorkEventArgs.Write
        Throw New NotImplementedException()
    End Sub

    Public Sub Write(msg As String, percentProgress As Integer) Implements IBackgroundWorkEventArgs.Write
        Throw New NotImplementedException()
    End Sub

    Public Sub Write(percentProgress As Integer) Implements IBackgroundWorkEventArgs.Write
        Throw New NotImplementedException()
    End Sub

End Class