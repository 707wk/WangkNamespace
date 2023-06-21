Public Class BackgroundWorkLayer
    Implements IBackgroundWorkEventArgs

    ''' <summary>
    ''' 后台触发事件
    ''' </summary>
    Private BackgroundWorkAction As Action(Of IBackgroundWorkEventArgs)

    Private _Msg As String
    Public _Error As Exception

    Public Property Args As Object Implements IBackgroundWorkEventArgs.Args

    Public Property Result As Object Implements IBackgroundWorkEventArgs.Result

    Public Property IsCancel As Boolean Implements IBackgroundWorkEventArgs.IsCancel

    Public ReadOnly Property Msg As String Implements IBackgroundWorkEventArgs.Msg
        Get
            Return _Msg
        End Get
    End Property

    ''' <summary>
    ''' 发生的错误
    ''' </summary>
    Public ReadOnly Property [Error] As Object
        Get
            Return _Error
        End Get
    End Property

    Private IsRunWorkerCompleted = False

    Public Property Owner As Window

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Public Sub New(parent As Window)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Owner = parent

    End Sub

    Public Sub New(parent As DependencyObject)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Owner = Window.GetWindow(parent)

    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    ''' <param name="args">传入的参数</param>
    Public Async Function Run(backgroundWorkAction As Action(Of IBackgroundWorkEventArgs),
                         Optional args As Object = Nothing) As Task

        Me.BackgroundWorkAction = backgroundWorkAction
        Me.Args = args

        Dim originalContent As UIElement = Owner.Content
        Owner.Content = Nothing

        ContentGrid.Children.Insert(0, originalContent)
        Owner.Content = Me

        Try
            Await Task.Run(Sub()
                               backgroundWorkAction(Me)
                           End Sub)
        Catch ex As Exception
            _Error = ex
        End Try

        IsRunWorkerCompleted = True

        ContentGrid.Children.RemoveAt(0)
        Owner.Content = originalContent

    End Function

    Public Sub Write(msg As String) Implements IBackgroundWorkEventArgs.Write

        _Msg = msg

        Dispatcher.BeginInvoke(Threading.DispatcherPriority.Normal,
                               Sub()
                                   MessageText.Text = msg
                               End Sub)

    End Sub

    Public Sub Write(msg As String, percentProgress As Integer) Implements IBackgroundWorkEventArgs.Write

        Write(msg)

        Write(percentProgress)

    End Sub

    Public Sub Write(percentProgress As Integer) Implements IBackgroundWorkEventArgs.Write

        Dispatcher.BeginInvoke(Threading.DispatcherPriority.Normal,
                               Sub()
                                   If 0 <= percentProgress AndAlso
                                   percentProgress <= 100 Then

                                       MessageProgressBar.IsIndeterminate = False
                                       MessageProgressText.Visibility = Visibility.Visible

                                       MessageProgressBar.Value = percentProgress

                                       MessageProgressText.Text = $"{percentProgress}%"

                                   End If
                               End Sub)

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        If Not IsRunWorkerCompleted Then
            IsCancel = True
        End If
    End Sub

End Class
