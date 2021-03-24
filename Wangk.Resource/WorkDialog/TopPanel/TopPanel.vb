Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms

Public Class TopPanel
    Implements IDisposable

    Private ShowPanel As Wlp32chfkxQcoOeynhOr200aiM2YUx137
    Private ShowControl As Label

    'Public Shared MinimumShowMillisecond As Integer = 200

#Region "是否执行完毕"
    ''' <summary>
    ''' 显示的实例数量
    ''' </summary>
    Private Shared ShowCount As Integer = 0
    ''' <summary>
    ''' 是否执行完毕
    ''' </summary>
    Public Shared ReadOnly Property IsFinished As Boolean
        Get
            Return ShowCount = 0
        End Get
    End Property
#End Region

    Public Sub New()
        ShowControl = New Label
        ShowControl.AutoSize = False
        ShowControl.Size = New Size(400, 60)

        ShowPanel = New Wlp32chfkxQcoOeynhOr200aiM2YUx137
        ShowPanel.Dock = DockStyle.Fill
        ShowPanel.Size = ShowControl.Size
        ShowPanel.Controls.Add(ShowControl)
        'ShowControl.Parent = ShowPanel
        ShowControl.BackColor = Color.FromArgb(71, 71, 71)
        ShowControl.ForeColor = Color.White
        ShowControl.BorderStyle = BorderStyle.FixedSingle
        ShowControl.TextAlign = ContentAlignment.MiddleLeft
        ShowControl.AutoEllipsis = True
        ShowControl.Dock = DockStyle.None
        ShowControl.Anchor = AnchorStyles.None
        'ShowControl.Location = New Point(0, 0)

    End Sub

    Public Sub Show(parent As Control,
                    title As String)

        Interlocked.Increment(ShowCount)

        ShowPanel.Font = parent.Font
        ShowControl.Font = parent.Font

        parent.Controls.Add(ShowPanel)

        'ShowPanel.Refresh()
        'ShowControl.Refresh()
        ShowPanel.Show()
        ShowPanel.BringToFront()
        parent.Refresh()

        ShowControl.Text = $" 
    {title}    
 "

    End Sub

    ''' <summary>
    ''' 输出提示
    ''' </summary>
    Public Sub Write(msg As String)
        ShowPanel.Write(msg)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        Interlocked.Decrement(ShowCount)

        ShowControl?.Dispose()
        ShowPanel?.Dispose()
    End Sub

End Class
