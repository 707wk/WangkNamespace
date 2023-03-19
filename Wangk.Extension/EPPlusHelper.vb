Imports System.Data
Imports System.Runtime.CompilerServices
Imports OfficeOpenXml

Public Module EPPlusHelper

#Region "将数据导入表格"
    ''' <summary>
    ''' 将数据导入表格
    ''' </summary>
    <Extension()>
    Public Function Add(ByRef worksheets As ExcelWorksheets,
                        Name As String,
                        reader As IDataReader) As ExcelWorksheet

        Dim tmpWorkSheet = worksheets.Add(Name)

        CreateSheetColumns(tmpWorkSheet, reader)

        ' 循环读取
        Dim rowIndex = 1
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

        ' 自动列宽
        tmpWorkSheet.Cells.AutoFitColumns()

        ' 限制最大宽度
        For i001 = 1 To reader.FieldCount
            tmpWorkSheet.Column(i001).Width = Math.Min(tmpWorkSheet.Column(i001).Width, 20)
        Next

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
                                  reader As IDataReader)

        Dim headNameItems As New List(Of String)
        For i001 = 0 To reader.FieldCount - 1
            headNameItems.Add(reader.GetName(i001))
        Next

        For i001 = 1 To headNameItems.Count
            workSheet.Cells(1, i001).Value = headNameItems(i001 - 1)
        Next

        ' 列标题筛选
        workSheet.Cells($"1:1").AutoFilter = True

        ' 设置标题背景色
        workSheet.Cells($"1:1").Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
        workSheet.Cells($"1:1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.YellowGreen)

        ' 首行冻结
        workSheet.View.FreezePanes(2, 1)

        ' 设置列格式
        For i001 = 1 To reader.FieldCount

            Select Case reader.GetFieldType(i001 - 1)
                Case GetType(String)
                    workSheet.Column(i001).Style.Numberformat.Format = "@"

                Case GetType(Int32)
                    workSheet.Column(i001).Style.Numberformat.Format = "#,##0"

                Case GetType(Decimal)
                    If headNameItems(i001 - 1).Contains("%") Then
                        workSheet.Column(i001).Style.Numberformat.Format = "0.0%"
                    Else
                        workSheet.Column(i001).Style.Numberformat.Format = "#,##0.00"
                    End If

                Case GetType(DateTime)
                    workSheet.Column(i001).Style.Numberformat.Format = "yyyy/MM/dd hh:mm:ss;@"

                Case Else
                    workSheet.Column(i001).Style.Numberformat.Format = "@"

            End Select

        Next

    End Sub
#End Region

End Module
