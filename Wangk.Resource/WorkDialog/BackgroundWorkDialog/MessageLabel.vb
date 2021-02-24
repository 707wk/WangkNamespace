Imports System.Drawing
Imports System.Windows.Forms

Public Class MessageLabel
    Inherits Label

    Private _SourceText As String
    Public Property SourceText As String
        Get
            Return _SourceText
        End Get
        Set(ByVal value As String)
            _SourceText = value
            Timer1_Tick(Nothing, Nothing)
        End Set
    End Property

    Private Timer1 As Timer

    Public Sub New()
        Me.AutoSize = False
        Me.Dock = DockStyle.Fill
        Me.TextAlign = ContentAlignment.MiddleLeft

        Timer1 = New Timer
        Timer1.Interval = 600
        AddHandler Timer1.Tick, AddressOf Timer1_Tick
        Timer1.Start()

    End Sub

    Private StrLen = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs)

        Me.Text = SourceText + " ".PadRight(StrLen + 1, ".")

        If sender IsNot Nothing Then
            StrLen += 1
            StrLen = StrLen Mod 4
        End If

    End Sub

    Private Sub Class2_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Timer1.Stop()
        RemoveHandler Timer1.Tick, AddressOf Timer1_Tick
    End Sub
End Class
