Public Class BackgroundWork

    ''' <summary>
    ''' 窗口标题
    ''' </summary>
    Public Property Title As String
        Get
            Return ShowWindow.Title
        End Get
        Set
            ShowWindow.Title = Value
        End Set
    End Property

    Private ShowWindow As BackgroundWorkWindow

    Public Sub New(parent As Window)
        ShowWindow = New BackgroundWorkWindow
        ShowWindow.Owner = parent
    End Sub

    Public Sub New(parent As DependencyObject)
        ShowWindow = New BackgroundWorkWindow
        ShowWindow.Owner = Window.GetWindow(parent)

        If ShowWindow.Owner Is Nothing Then
            ShowWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        End If

    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    ''' <param name="args">传入的参数</param>
    Public Sub Run(backgroundWorkAction As Action(Of IBackgroundWorkEventArgs),
                   Optional args As Object = Nothing)

        ShowWindow.Args = args
        ShowWindow.Run(backgroundWorkAction)

    End Sub

    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Public ReadOnly Property Result As Object
        Get
            Return ShowWindow.Result
        End Get
    End Property

    ''' <summary>
    ''' 发生的错误
    ''' </summary>
    Public ReadOnly Property [Error] As Exception
        Get
            Return ShowWindow.Error
        End Get
    End Property

    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Public ReadOnly Property IsCancel As Boolean
        Get
            Return ShowWindow.IsCancel
        End Get
    End Property

End Class
