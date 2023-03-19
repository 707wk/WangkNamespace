Imports System.ComponentModel

Public Class WB7CA71228C7CFDE3C7297F7A4667F01D
    Implements IUIWorkEventArgs

    ''' <summary>
    ''' 前台触发事件
    ''' </summary>
    Private UIWorkAction As Action(Of IUIWorkEventArgs)

#Region "传递的参数"
    Private _Args As Object
    ''' <summary>
    ''' 传递的参数
    ''' </summary>
    Public Property Args As Object Implements IUIWorkEventArgs.Args
        Get
            Return _Args
        End Get
        Set(value As Object)
            _Args = value
        End Set
    End Property
#End Region

#Region "操作结果"
    Private _Result As Object
    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Public Property Result As Object Implements IUIWorkEventArgs.Result
        Get
            Return _Result
        End Get
        Set(value As Object)
            _Result = value
        End Set
    End Property
#End Region

#Region ""
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

    Private Sub WB7CA71228C7CFDE3C7297F7A4667F01D_Load(sender As Object, e As EventArgs) Handles Me.Load

        Timer1.Interval = 10
    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="UIWorkAction">后台事件</param>
    Public Overloads Sub Start(UIWorkAction As Action(Of IUIWorkEventArgs))
        Me.UIWorkAction = UIWorkAction
        Me.Args = Args

        Me.ShowDialog()

    End Sub

    Private Sub WB7CA71228C7CFDE3C7297F7A4667F01D_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        MessageLabel.Text = Me.Text

        Timer1.Start()
    End Sub

    Private Sub WB7CA71228C7CFDE3C7297F7A4667F01D_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Public Delegate Sub WriteCallback(msg As String)
    ''' <summary>
    ''' 输出信息
    ''' </summary>
    ''' <param name="msg">输出信息</param>
    Public Sub Write(msg As String) Implements IUIWorkEventArgs.Write
        If Me.InvokeRequired Then
            Me.Invoke(New WriteCallback(AddressOf Write),
                      New Object() {msg})
            Exit Sub
        End If

        MessageLabel.Text = msg
        MessageLabel.Refresh()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()

        Try

            UIWorkAction(Me)

        Catch ex As Exception
            _Error = ex
        End Try

        Me.Close()
    End Sub

End Class