Imports System.Drawing
Imports System.Windows.Forms

Public Class CheckBoxDataGridViewButtonColumn
    Inherits DataGridViewButtonColumn


    Private _ForeColor As Color
    ''' <summary>
    ''' 字体颜色
    ''' </summary>
    Public Property ForeColor As Color
        Get
            Return _ForeColor
        End Get
        Set(ByVal value As Color)
            _ForeColor = value

            _ForeColorBrush = New SolidBrush(Color.White)
            _BackColorBrush = New SolidBrush(_ForeColor)
            _BorderColorPen = New Pen(_ForeColor)

        End Set
    End Property

    Private _ForeColorBrush As SolidBrush
    ''' <summary>
    ''' 字体颜色画刷
    ''' </summary>
    Public ReadOnly Property ForeColorBrush As SolidBrush
        Get
            Return _ForeColorBrush
        End Get
    End Property

    Private _BackColorBrush As SolidBrush
    ''' <summary>
    ''' 背景颜色画刷
    ''' </summary>
    Public ReadOnly Property BackColorBrush As SolidBrush
        Get
            Return _BackColorBrush
        End Get
    End Property

    Private _BorderColorPen As Pen
    ''' <summary>
    ''' 颜色画笔
    ''' </summary>
    Public ReadOnly Property BorderColorPen As Pen
        Get
            Return _BorderColorPen
        End Get
    End Property

    Public ValueStringFormat As StringFormat

    Public Sub New()

        Me.ReadOnly = True
        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DefaultCellStyle.WrapMode = DataGridViewTriState.False
        DefaultCellStyle.Padding = New Padding(2, 3, 2, 3)
        FlatStyle = FlatStyle.Flat

        SortMode = DataGridViewColumnSortMode.Automatic
        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        ValueStringFormat = New StringFormat
        ValueStringFormat.Alignment = StringAlignment.Center
        ValueStringFormat.LineAlignment = StringAlignment.Center

    End Sub

End Class
