Imports System.Data
Imports System.Runtime.CompilerServices
Imports OfficeOpenXml

Public Module EPPlusHelper

#Region "将数据导入表格"
    ''' <summary>
    ''' 将数据导入表格, 标题包含 % 则按百分比显示, 包含 明细 则设置换行
    ''' </summary>
    ''' <param name="worksheets">表格集合</param>
    ''' <param name="Name">表格名称</param>
    ''' <param name="reader">数据源</param>
    ''' <param name="descriptions">数据说明</param>
    ''' <param name="autoSize">自动调整行列尺寸</param>
    <Extension()>
    Public Function Add(worksheets As ExcelWorksheets,
                        Name As String,
                        reader As IDataReader,
                        Optional descriptions As String() = Nothing,
                        Optional autoSize As Boolean = False) As ExcelWorksheet

        Dim tmpWorkSheet = worksheets.Add(Name)

        Dim rowIndex = 1

        ' 数据说明
        If descriptions IsNot Nothing AndAlso
            descriptions.Count > 0 Then

            rowIndex = descriptions.Count + 1

            For i001 = 1 To descriptions.Count
                tmpWorkSheet.Row(i001).Merged = True
                tmpWorkSheet.Cells(i001, 1).Value = descriptions(i001 - 1)

                tmpWorkSheet.Row(i001).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                tmpWorkSheet.Row(i001).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0))

            Next

        End If

        CreateSheetColumns(tmpWorkSheet, rowIndex, reader)

        ' 循环读取
        While reader.Read

            ' 填充数据
            rowIndex += 1
            For i001 = 1 To reader.FieldCount
                If Not String.IsNullOrWhiteSpace($"{reader(i001 - 1)}") Then
                    tmpWorkSheet.Cells(rowIndex, i001).Value = reader(i001 - 1)
                Else
                    tmpWorkSheet.Cells(rowIndex, i001).Value = "/"
                    tmpWorkSheet.Cells(rowIndex, i001).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
                End If
            Next

        End While

        If autoSize Then

            ' 自动列宽
            tmpWorkSheet.Cells.AutoFitColumns()

            ' 限制最大宽度
            For i001 = 1 To reader.FieldCount
                tmpWorkSheet.Column(i001).Width = Math.Min(tmpWorkSheet.Column(i001).Width, 20)
            Next

        Else

            ' 固定列宽
            tmpWorkSheet.Columns.Width = 15

        End If

        ' 自动行高
        For i001 = 1 To tmpWorkSheet.Dimension.End.Row
            tmpWorkSheet.Row(i001).CustomHeight = True
        Next

        Return tmpWorkSheet
    End Function
#End Region

#Region "创建表格列"
    ''' <summary>
    ''' 创建表格列
    ''' </summary>
    Private Sub CreateSheetColumns(workSheet As ExcelWorksheet,
                                   startRowIndex As Integer,
                                   reader As IDataReader)

        Dim headNameItems As New List(Of String)
        For i001 = 0 To reader.FieldCount - 1
            headNameItems.Add(reader.GetName(i001))
        Next

        For i001 = 1 To headNameItems.Count
            workSheet.Cells(startRowIndex, i001).Value = headNameItems(i001 - 1)
        Next

        ' 列标题筛选
        workSheet.Cells($"{startRowIndex}:{startRowIndex}").AutoFilter = True

        ' 设置标题背景色
        workSheet.Cells($"{startRowIndex}:{startRowIndex}").Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
        workSheet.Cells($"{startRowIndex}:{startRowIndex}").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.YellowGreen)

        ' 列标题自动换行并居中
        workSheet.Row(startRowIndex).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        workSheet.Row(startRowIndex).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
        workSheet.Row(startRowIndex).Style.WrapText = True

        ' 首行冻结
        workSheet.View.FreezePanes(startRowIndex + 1, 1)

        ' 设置列格式
        For i001 = 1 To reader.FieldCount

            Select Case reader.GetFieldType(i001 - 1)
                Case GetType(String)
                    workSheet.Column(i001).Style.Numberformat.Format = "@"

                    If headNameItems(i001 - 1).Contains("明细") Then
                        workSheet.Column(i001).Style.WrapText = True
                    End If

                Case GetType(Int32)
                    workSheet.Column(i001).Style.Numberformat.Format = "#,##0"

                Case GetType(Decimal)
                    If headNameItems(i001 - 1).Contains("%") Then
                        workSheet.Column(i001).Style.Numberformat.Format = "0.0%"
                    Else
                        workSheet.Column(i001).Style.Numberformat.Format = "#,##0.00"
                    End If

                Case GetType(DateTime)
                    workSheet.Column(i001).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss;@"

                Case Else
                    workSheet.Column(i001).Style.Numberformat.Format = "@"

            End Select

        Next

    End Sub
#End Region

End Module
