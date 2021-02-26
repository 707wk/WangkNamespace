Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.WindowsAPICodePack
Imports Microsoft.WindowsAPICodePack.Taskbar
Imports Wangk.Resource

Public Class W51F447639869E1D644FEE98B1DA024D2
    Implements BackgroundWorkEventArgs

    ''' <summary>
    ''' 后台触发事件
    ''' </summary>
    Private BackgroundWorkAction As Action(Of BackgroundWorkEventArgs)

#Region "传递的参数"
    Private _Args As Object
    ''' <summary>
    ''' 传递的参数
    ''' </summary>
    Public Property Args As Object Implements BackgroundWorkEventArgs.Args
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
    Public Property IsCancel As Boolean Implements BackgroundWorkEventArgs.IsCancel
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
    Public Property Result As Object Implements BackgroundWorkEventArgs.Result
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

    Private tmpTaskbarManager As TaskbarManager

    Private Sub W51F447639869E1D644FEE98B1DA024D2_Load(sender As Object, e As EventArgs) Handles Me.Load

        ''自动换行
        'MessageLabel1.MaximumSize = New Drawing.Size(TableLayoutPanel1.Width, 0)
        '窗体透明度
        'Me.Opacity = 0.95
        ProgressBar1.Step = 1

        tmpTaskbarManager = TaskbarManager.Instance

    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    Public Overloads Sub Start(backgroundWorkAction As Action(Of BackgroundWorkEventArgs))
        Me.BackgroundWorkAction = backgroundWorkAction
        Me.Args = Args

        Me.ShowDialog()

    End Sub

    Private Sub W51F447639869E1D644FEE98B1DA024D2_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        MessageLabel1.SourceText = Me.Text
        Me.Refresh()

        tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.Indeterminate)

        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub W51F447639869E1D644FEE98B1DA024D2_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If BackgroundWorker1.IsBusy Then
            IsCancel = True

            e.Cancel = True
            Exit Sub
        End If

    End Sub

#Region "输出提示及进度"
    Private Delegate Sub WriteCallback(msg As String, percentProgress As Integer)
    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    ''' <param name="percentProgress">当前进度 0-99</param>
    Public Sub Write(msg As String, percentProgress As Integer) Implements BackgroundWorkEventArgs.Write
        If Me.InvokeRequired Then
            Me.Invoke(New WriteCallback(AddressOf Write),
                      New Object() {msg, percentProgress})
            Exit Sub
        End If

        Write(msg)

        Write(percentProgress)

    End Sub
#End Region

#Region "输出进度"
    Private Delegate Sub WritePercentProgressCallback(percentProgress As Integer)
    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="percentProgress">当前进度 0-99</param>
    Public Sub Write(percentProgress As Integer) Implements BackgroundWorkEventArgs.Write
        If Me.InvokeRequired Then
            Me.Invoke(New WritePercentProgressCallback(AddressOf Write),
                      New Object() {percentProgress})
            Exit Sub
        End If

        If ProgressBar1.Minimum <= percentProgress AndAlso
            percentProgress <= ProgressBar1.Maximum Then

            ProgressBar1.Value = percentProgress

            tmpTaskbarManager.SetProgressValue(percentProgress, ProgressBar1.Maximum)

            Label1.Text = $"{percentProgress}%"

            ProgressBar1.Update()
            Label1.Update()

        End If
    End Sub
#End Region

#Region "输出提示"
    Private Delegate Sub WriteMsgCallback(msg As String)
    ''' <summary>
    ''' 输出提示
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    Public Sub Write(msg As String) Implements BackgroundWorkEventArgs.Write
        If Me.InvokeRequired Then
            Me.Invoke(New WriteMsgCallback(AddressOf Write),
                      New Object() {msg})
            Exit Sub
        End If

        MessageLabel1.SourceText = msg

        Try
            InfoText.AppendText($"{msg}{vbCrLf}")
            InfoText.Update()
        Catch ex As Exception

        End Try

        MessageLabel1.Update()

    End Sub
#End Region

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.Normal)

        BackgroundWorkAction(Me)

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            _Error = e.Error
            tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.Error)
        End If

        Me.Close()

    End Sub

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        InfoText.Hide()
        Me.Height -= InfoText.Height

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        If BackgroundWorker1.IsBusy Then
            IsCancel = True
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Height += InfoText.Height
        InfoText.Show()
        LinkLabel1.Hide()
    End Sub

    Private Sub W51F447639869E1D644FEE98B1DA024D2_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.NoProgress)
    End Sub

End Class