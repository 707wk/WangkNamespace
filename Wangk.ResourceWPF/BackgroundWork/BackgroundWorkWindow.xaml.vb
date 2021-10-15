Imports System.Runtime.InteropServices

Public Class BackgroundWorkWindow
    Implements IBackgroundWorkEventArgs

    ''' <summary>
    ''' 后台触发事件
    ''' </summary>
    Private BackgroundWorkAction As Action(Of IBackgroundWorkEventArgs)

#Region "传递的参数"
    Private _Args As Object
    ''' <summary>
    ''' 传递的参数
    ''' </summary>
    Public Property Args As Object Implements IBackgroundWorkEventArgs.Args
        Get
            Return _Args
        End Get
        Set(value As Object)
            _Args = value
        End Set
    End Property
#End Region

#Region "是否取消"
    Private _IsCancel As Boolean = False
    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Public Property IsCancel As Boolean Implements IBackgroundWorkEventArgs.IsCancel
        Get
            Return _IsCancel
        End Get
        Set(value As Boolean)
            _IsCancel = value
        End Set
    End Property
#End Region

#Region "操作结果"
    Private _Result As Object
    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Public Property Result As Object Implements IBackgroundWorkEventArgs.Result
        Get
            Return _Result
        End Get
        Set(value As Object)
            _Result = value
        End Set
    End Property
#End Region

#Region "发生的错误"
    Public _Error As Exception
    ''' <summary>
    ''' 发生的错误
    ''' </summary>
    Public ReadOnly Property [Error] As Object
        Get
            Return _Error
        End Get
    End Property
#End Region

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    Public Overloads Sub Run(backgroundWorkAction As Action(Of IBackgroundWorkEventArgs))
        Me.BackgroundWorkAction = backgroundWorkAction
        Me.Args = Args

        Me.ShowDialog()

    End Sub

    Private IsFirstShow = False
    Private IsRunWorkerCompleted = False
    Private Async Sub Window_IsVisibleChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        If IsFirstShow Then
            Exit Sub
        End If
        IsFirstShow = True

        MessageText.Text = Me.Title

        If Me.Owner.TaskbarItemInfo Is Nothing Then
            Me.Owner.TaskbarItemInfo = New Shell.TaskbarItemInfo
        End If

        Me.Owner.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal
        Me.Owner.TaskbarItemInfo.ProgressValue = 0

        Try
            Await Task.Run(Sub()
                               BackgroundWorkAction(Me)
                           End Sub)
        Catch ex As Exception
            _Error = ex
        End Try

        Me.Owner.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.None

        IsRunWorkerCompleted = True
        Me.Close()

    End Sub

    Public Sub Write(msg As String) Implements IBackgroundWorkEventArgs.Write

        Dispatcher.BeginInvoke(Threading.DispatcherPriority.Normal,
                               Sub()
                                   MessageText.Text = msg
                               End Sub)

    End Sub

    Public Sub Write(msg As String,
                     percentProgress As Integer) Implements IBackgroundWorkEventArgs.Write

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

                                       Me.Owner.TaskbarItemInfo.ProgressValue = percentProgress / 100

                                   End If
                               End Sub)

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        If Not IsRunWorkerCompleted Then
            IsCancel = True
        End If
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)

        If Not IsRunWorkerCompleted Then
            IsCancel = True

            e.Cancel = True
            Exit Sub
        End If

    End Sub

End Class
