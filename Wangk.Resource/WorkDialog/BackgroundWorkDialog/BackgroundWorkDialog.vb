Imports System.Drawing
''' <summary>
''' 后台运行对话框
''' </summary>
Public Class BackgroundWorkDialog
    Implements IDisposable

    ''' <summary>
    ''' 窗口标题
    ''' </summary>
    Public Property Text As String
        Get
            Return ShowDialog.Text
        End Get
        Set
            ShowDialog.Text = Value
        End Set
    End Property

    ''' <summary>
    ''' 窗口图标
    ''' </summary>
    Public Property Icon As Icon
        Get
            Return ShowDialog.Icon
        End Get
        Set
            ShowDialog.Icon = Value
            ShowDialog.ShowIcon = True
        End Set
    End Property

    Private ShowDialog As W51F447639869E1D644FEE98B1DA024D2

    Public Sub New()
        ShowDialog = New W51F447639869E1D644FEE98B1DA024D2 With {
            .ShowIcon = False
        }

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ShowDialog?.Dispose()
    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="backgroundWorkAction">后台事件</param>
    ''' <param name="args">传入的参数</param>
    Public Sub Start(backgroundWorkAction As Action(Of BackgroundWorkEventArgs),
                     Optional args As Object = Nothing)

        ShowDialog.Args = args
        ShowDialog.Start(backgroundWorkAction)
    End Sub

    ''' <summary>
    ''' 操作结果
    ''' </summary>
    Public ReadOnly Property Result As Object
        Get
            Return ShowDialog.Result
        End Get
    End Property

    ''' <summary>
    ''' 发生的错误
    ''' </summary>
    Public ReadOnly Property [Error] As Exception
        Get
            Return ShowDialog.Error
        End Get
    End Property

    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Public ReadOnly Property IsCancel As Boolean
        Get
            Return ShowDialog.IsCancel
        End Get
    End Property

End Class
