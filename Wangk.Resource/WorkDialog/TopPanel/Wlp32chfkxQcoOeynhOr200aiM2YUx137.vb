Imports System.Drawing
Imports System.Windows.Forms

Public Class Wlp32chfkxQcoOeynhOr200aiM2YUx137
    Inherits TableLayoutPanel

    Private Const WS_EX_TRANSPARENT = &H20

    Public Sub New()

        SetStyle(ControlStyles.Opaque, True)

    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using brush As New SolidBrush(Color.FromArgb(5, Color.Black))
            e.Graphics.FillRectangle(brush, Me.ClientRectangle)
        End Using

        'MyBase.OnPaint(e)

    End Sub

#Region "输出提示"
    '   Private Delegate Sub WriteMsgCallback(Msg As String)
    '   ''' <summary>
    '   ''' 输出提示
    '   ''' </summary>
    '   ''' <param name="msg">输出信息</param>
    '   Public Sub Write(msg As String)
    '       If Me.InvokeRequired Then
    '           Me.Invoke(New WriteMsgCallback(AddressOf Write),
    '                     New Object() {msg})
    '           Exit Sub
    '       End If

    '       Dim tmpLable = CType(Me.Controls(0), Label)
    '       tmpLable.Text = $" 
    '   {msg}    
    '"

    '       Me.Refresh()

    '   End Sub
#End Region

End Class
