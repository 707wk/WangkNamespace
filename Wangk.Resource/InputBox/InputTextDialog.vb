Public Class InputTextDialog
    Implements IDisposable

    ''' <summary>
    ''' 窗口标题
    ''' </summary>
    Public Property Text As String
        Get
            Return _ShowDialog.Text
        End Get
        Set
            _ShowDialog.Text = Value
        End Set
    End Property

    ''' <summary>
    ''' 窗口图标
    ''' </summary>
    Public Property Icon As Drawing.Icon
        Get
            Return _ShowDialog.Icon
        End Get
        Set
            _ShowDialog.Icon = Value
            _ShowDialog.ShowIcon = True
        End Set
    End Property

    ''' <summary>
    ''' 是否输入密码
    ''' </summary>
    Public Property PasswordChar As Char
        Get
            Return _ShowDialog.TextBox1.PasswordChar
        End Get
        Set(value As Char)
            _ShowDialog.TextBox1.PasswordChar = value
        End Set
    End Property

    ''' <summary>
    ''' 输入的字符串
    ''' </summary>
    Public ReadOnly Property Value As String
        Get
            Return _ShowDialog.TextBox1.Text
        End Get
    End Property

    Private _ShowDialog As Wz2g7FfSJkpDmwBuaSnLKe5FLA5RUSSwy

    Public Sub New()
        _ShowDialog = New Wz2g7FfSJkpDmwBuaSnLKe5FLA5RUSSwy
        _ShowDialog.ShowIcon = False
    End Sub

    Public Function ShowDialog(Optional OKButtonText As String = "OK",
                               Optional CancelButtonText As String = "Cancel") As Windows.Forms.DialogResult
        _ShowDialog.TextBox1.Clear()
        _ShowDialog.Button1.Text = OKButtonText
        _ShowDialog.Button2.Text = CancelButtonText

        Return _ShowDialog.ShowDialog
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If _ShowDialog IsNot Nothing Then
            _ShowDialog.Dispose()
        End If
    End Sub

End Class
