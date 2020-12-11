Imports System.Windows.Forms

''' <summary>
''' 输入框
''' </summary>
Public Class Wz2g7FfSJkpDmwBuaSnLKe5FLA5RUSSwy
    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Length = 0 Then
            TextBox1.Focus()
            Exit Sub
        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Clear()
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub InputBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Wz2g7FfSJkpDmwBuaSnLKe5FLA5RUSSwy_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        TextBox1.Focus()
    End Sub
End Class