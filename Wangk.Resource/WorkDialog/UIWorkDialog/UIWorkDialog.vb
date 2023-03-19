Imports System.Drawing
''' <summary>
''' 前台运行对话框
''' </summary>
Public Class UIWorkDialog
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

    Private ShowDialog As WB7CA71228C7CFDE3C7297F7A4667F01D

    Public Sub New()
        ShowDialog = New WB7CA71228C7CFDE3C7297F7A4667F01D
        ShowDialog.ShowIcon = False
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If ShowDialog IsNot Nothing Then
            ShowDialog.Dispose()
        End If
    End Sub

    ''' <summary>
    ''' 开始执行后台事件
    ''' </summary>
    ''' <param name="UIWorkAction">后台事件</param>
    ''' <param name="args">传入的参数</param>
    Public Sub Start(UIWorkAction As Action(Of IUIWorkEventArgs),
                     Optional args As Object = Nothing)
        ShowDialog.Args = args
        ShowDialog.Start(UIWorkAction)
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

End Class
