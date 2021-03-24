Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' 带多选的DataGridView控件
''' </summary>
Public Class CheckBoxDataGridView
    Inherits DataGridView

    Friend WithEvents ColumnCheckBox As CheckBox
    Public Sub New()
        With Me
            .DoubleBuffered = True
            '.BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            '.RowHeadersVisible = True
            '.AllowUserToResizeRows = True
            '.AllowUserToOrderColumns = True
            '.AllowUserToResizeColumns = True
            .AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control
            .GridColor = Color.FromArgb(&HE5, &HE5, &HE5)
            '.CellBorderStyle = DataGridViewCellBorderStyle.None
            .ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
            .RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False

            '.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
            '.RowHeadersWidth = 60
            .RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowTemplate.MinimumHeight = 30

        End With

        ColumnCheckBox = New CheckBox
        With ColumnCheckBox
            .BackColor = SystemColors.Window
            .Text = ""
            .AutoSize = True
        End With

        Me.Controls.Add(ColumnCheckBox)

        SetColumnCheckBoxLocation()

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles ColumnCheckBox.CheckedChanged
        For Each item As DataGridViewRow In Me.Rows
            item.Cells(0).Value = ColumnCheckBox.Checked
        Next

    End Sub

    Private Sub Class1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles Me.ColumnWidthChanged
        SetColumnCheckBoxLocation()
    End Sub

    Private Sub Class1_ColumnHeadersHeightChanged(sender As Object, e As EventArgs) Handles Me.ColumnHeadersHeightChanged
        SetColumnCheckBoxLocation()
    End Sub

    Private Sub Class1_RowHeadersWidthChanged(sender As Object, e As EventArgs) Handles Me.RowHeadersWidthChanged
        SetColumnCheckBoxLocation()
    End Sub

    Private Sub SetColumnCheckBoxLocation()
        With ColumnCheckBox
            If Columns.Count = 0 Then
                ColumnCheckBox.Hide()
                Exit Sub
            End If

            If Columns(0).GetType Is GetType(DataGridViewCheckBoxColumn) Then
                ColumnCheckBox.Show()
                .Location = New Point(If(Me.RowHeadersVisible, Me.RowHeadersWidth, 0) + (Me.Columns(0).Width - ColumnCheckBox.Width) / 2 + 0.5, (Me.ColumnHeadersHeight - ColumnCheckBox.Height) \ 2)
            Else
                ColumnCheckBox.Hide()
            End If
        End With
    End Sub

    Private Sub Class1_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles Me.ColumnAdded
        If (TypeOf Columns(0) IsNot DataGridViewCheckBoxColumn) AndAlso
            Columns(0).Name <> "CheckBoxColumn707wk" Then

            Me.Columns.Insert(0, New DataGridViewCheckBoxColumn With {
                              .Name = "CheckBoxColumn707wk",
                              .[ReadOnly] = True,
                              .HeaderText = "",
                              .MinimumWidth = 48,
                              .Width = .MinimumWidth,
                              .Frozen = True,
                              .SortMode = DataGridViewColumnSortMode.Automatic})
            ColumnCheckBox.Checked = False
            SetColumnCheckBoxLocation()
        End If

    End Sub

    Private Sub Class1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.CellMouseClick
        If e.RowIndex < 0 OrElse
            e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If TypeOf Me.Columns(e.ColumnIndex) Is DataGridViewCheckBoxColumn Then
            Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Me.Rows(e.RowIndex).Cells(e.ColumnIndex).EditedFormattedValue
        End If

    End Sub

    Private Sub CheckBoxDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Me.CellFormatting

        If TypeOf Me.Columns(e.ColumnIndex) Is DataGridViewCheckBoxColumn Then
            '勾选后突出单元格颜色
            If Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = True Then
                Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.FromArgb(127, 187, 66)
            Else
                Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Nothing
            End If
        End If

    End Sub

    Private Sub CheckBoxDataGridView_ColumnStateChanged(sender As Object, e As DataGridViewColumnStateChangedEventArgs) Handles Me.ColumnStateChanged
        SetColumnCheckBoxLocation()
    End Sub

    Private Sub CheckBoxDataGridView_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles Me.RowsRemoved
        If Me.RowHeadersVisible Then
            For rowID = e.RowIndex To Me.Rows.GetLastRow(DataGridViewElementStates.Displayed)
                Me.Rows(rowID).HeaderCell.Value = $"{rowID + 1}"
            Next
        End If

        For rowID = Me.Rows.Count - 1 To 0 Step -1
            If Me.Rows(rowID).Cells(0).EditedFormattedValue Then
                Exit Sub
            End If
        Next

        ColumnCheckBox.Checked = False

    End Sub

    Private Sub CheckBoxDataGridView_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Me.RowsAdded
        If Me.RowHeadersVisible Then
            For rowID = e.RowIndex To Me.Rows.GetLastRow(DataGridViewElementStates.Displayed)
                Me.Rows(rowID).HeaderCell.Value = $"{rowID + 1}"
            Next
        End If

    End Sub

    Private Sub CheckBoxDataGridView_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles Me.RowStateChanged
        If Me.RowHeadersVisible Then
            e.Row.HeaderCell.Value = $"{e.Row.Index + 1}"
        End If

    End Sub

#Region "拖拽操作"
    Private FirstDragDropRowID As Integer = -1
    Private NowDragDropRowID As Integer = -1

    Private Sub CheckBoxDataGridView_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub CheckBoxDataGridView_CellMouseMove(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.CellMouseMove
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        If Me.AllowDrop AndAlso
            e.Clicks < 2 AndAlso
            e.Button = MouseButtons.Left AndAlso
            e.ColumnIndex = -1 Then

            FirstDragDropRowID = e.RowIndex
            Me.DoDragDrop(Me.Rows(e.RowIndex), DragDropEffects.Move)

        End If
    End Sub

    Private Sub CheckBoxDataGridView_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop

        Dim tmp = NowDragDropRowID
        NowDragDropRowID = -1
        Me.InvalidateRow(tmp)

        Dim clientPoint = Me.PointToClient(New Point(e.X, e.Y))
        Dim tmpHit = Me.HitTest(clientPoint.X, clientPoint.Y)

        If tmpHit.RowIndex < 0 OrElse
            tmpHit.RowIndex = FirstDragDropRowID Then
            Exit Sub
        End If

        Me.InvalidateRow(tmpHit.RowIndex)

        Dim tmpRow = Me.Rows(FirstDragDropRowID)
        Me.Rows.RemoveAt(FirstDragDropRowID)

        If FirstDragDropRowID < tmpHit.RowIndex Then
            FirstDragDropRowID = tmpHit.RowIndex - 1
            Me.Rows.Insert(FirstDragDropRowID, tmpRow)
        Else
            FirstDragDropRowID = tmpHit.RowIndex
            Me.Rows.Insert(FirstDragDropRowID, tmpRow)
        End If

        Me.Rows(FirstDragDropRowID).Selected = True
        Me.CurrentCell = Me.Rows(FirstDragDropRowID).Cells(0)

        FirstDragDropRowID = -1
        NowDragDropRowID = -1

    End Sub

    Private Sub CheckBoxDataGridView_DragOver(sender As Object, e As DragEventArgs) Handles Me.DragOver
        Dim clientPoint = Me.PointToClient(New Point(e.X, e.Y))
        Dim tmpHit = Me.HitTest(clientPoint.X, clientPoint.Y)
        If NowDragDropRowID = tmpHit.RowIndex Then
            Exit Sub
        End If

        If FirstDragDropRowID <> NowDragDropRowID AndAlso
            NowDragDropRowID > -1 Then

            Dim tmp = NowDragDropRowID
            NowDragDropRowID = -1
            Me.InvalidateRow(tmp)
        End If

        NowDragDropRowID = tmpHit.RowIndex

        If FirstDragDropRowID = NowDragDropRowID Then
            e.Effect = DragDropEffects.None
            Exit Sub
        Else
            e.Effect = DragDropEffects.Move
        End If

        Me.InvalidateRow(NowDragDropRowID)

    End Sub

    Private Sub CheckBoxDataGridView_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles Me.RowPostPaint
        If e.RowIndex = NowDragDropRowID AndAlso
            e.RowIndex <> FirstDragDropRowID Then

            e.Graphics.FillRectangle(
                Brushes.DarkGray,
                e.RowBounds.X,
                e.RowBounds.Y,
                Me.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + If(Me.RowHeadersVisible, Me.RowHeadersWidth, 0),
                2)
        End If

    End Sub
#End Region

    Private Sub CheckBoxDataGridView_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles Me.CellPainting

        If e.RowIndex < 0 OrElse
            e.ColumnIndex < 0 Then
            Exit Sub
        End If

        If TypeOf Me.Columns(e.ColumnIndex) IsNot CheckBoxDataGridViewButtonColumn Then
            Exit Sub
        End If

        Dim tmpCheckBoxDataGridViewButtonColumn As CheckBoxDataGridViewButtonColumn = Me.Columns(e.ColumnIndex)

        If e.RowIndex Mod 2 Then
            e.Graphics.FillRectangle(New SolidBrush(Me.AlternatingRowsDefaultCellStyle.BackColor), e.CellBounds)
        Else
            e.Graphics.FillRectangle(New SolidBrush(Me.DefaultCellStyle.BackColor), e.CellBounds)
        End If

        If e.Value IsNot Nothing Then
            Dim tmpFontSize = e.Graphics.MeasureString("品号", Me.Font)

            '背景描边
            'e.Graphics.DrawRectangle(tmpCheckBoxDataGridViewButtonColumn.BorderColorPen,
            '                         New Rectangle(e.CellBounds.X + 2,
            '                                       e.CellBounds.Y + (e.CellBounds.Height - tmpFontSize.Height - 4) / 2,
            '                                       e.CellBounds.Width - 4,
            '                                       tmpFontSize.Height + 4))

            '背景填充
            e.Graphics.FillRectangle(tmpCheckBoxDataGridViewButtonColumn.BackColorBrush,
                                     New Rectangle(e.CellBounds.X + 2,
                                                   e.CellBounds.Y + (e.CellBounds.Height - tmpFontSize.Height - 6) / 2,
                                                   e.CellBounds.Width - 4,
                                                   tmpFontSize.Height + 6))
            e.Graphics.DrawString(e.Value,
                                  Me.Font,
                                  tmpCheckBoxDataGridViewButtonColumn.ForeColorBrush,
                                  e.CellBounds,
                                  tmpCheckBoxDataGridViewButtonColumn.ValueStringFormat)
        End If

        e.Handled = True

    End Sub

    ''' <summary>
    ''' 获取或设置一个值，该值指示是否显示包含行标题的列
    ''' 重新声明以解决表头多选框位置不动的问题
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property RowHeadersVisible As Boolean
        Get
            Return MyBase.RowHeadersVisible
        End Get
        Set(ByVal value As Boolean)
            MyBase.RowHeadersVisible = value
            SetColumnCheckBoxLocation()
        End Set
    End Property

End Class
