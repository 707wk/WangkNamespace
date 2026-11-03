Imports System.Data
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

''' <summary>EPPlus 导出扩展</summary>
Public Module EPPlusHelper

    Private Const MaxColumnWidth As Double = 20
    Private Const DefaultColumnWidth As Double = 15
    Private Const EmptyCellText As String = "/"
    Private Const DefaultFontName As String = "Calibri"

#Region "将数据导入表格"
    ''' <summary>将数据读取器导入新建工作表</summary>
    ''' <param name="worksheets">工作表集合</param>
    ''' <param name="Name">新建工作表名称</param>
    ''' <param name="reader">数据源</param>
    ''' <param name="descriptions">数据说明行</param>
    ''' <param name="autoSize">是否自动调整列宽</param>
    ''' <param name="fontName">单元格显示字体名称, 为空时按默认字体显示</param>
    ''' <remarks>
    ''' 列标题包含 % 的浮点列按百分比格式显示, 包含 明细 的文本列自动换行.
    ''' 空值单元格写入 / 并居中.
    ''' 数据说明行按数据列合并, 用于横跨表格显示长文本.
    ''' 列宽换算基准固定设为 Calibri, 与 EPPlus 7 的默认主题保持一致.
    ''' 仅当 fontName 与基准字体不同时才额外设置单元格显示字体, 避免重复定义.
    ''' 该基准作用于整个工作簿, 同一工作簿内的其它工作表列宽基准也会随之变化.
    ''' reader 读取完成后不关闭, 由调用方负责.
    ''' </remarks>
    <Extension()>
    Public Function Add(worksheets As ExcelWorksheets,
                        Name As String,
                        reader As IDataReader,
                        Optional descriptions As String() = Nothing,
                        Optional autoSize As Boolean = False,
                        Optional fontName As String = DefaultFontName) As ExcelWorksheet

        If reader Is Nothing Then Throw New ArgumentNullException(NameOf(reader))

        Dim sheet = worksheets.Add(Name)

        ' 列宽换算基准固定为 Calibri, 与 EPPlus 7 的默认主题保持一致
        sheet.Workbook.Styles.Fonts(0).Name = DefaultFontName

        ' 单元格显示字体, 与基准相同时无需重复定义
        If Not String.IsNullOrWhiteSpace(fontName) AndAlso
           Not String.Equals(fontName, DefaultFontName, StringComparison.OrdinalIgnoreCase) Then
            sheet.Cells.Style.Font.Name = fontName
        End If
        Dim rowIndex = 1
        Dim descriptionCount = If(descriptions?.Length, 0)

        ' 数据说明行按数据列合并, 便于长文本横跨表格
        If descriptionCount > 0 Then
            rowIndex = descriptionCount + 1
            For i = 1 To descriptionCount
                sheet.Cells(i, 1).Value = descriptions(i - 1)
                If reader.FieldCount > 0 Then
                    Dim descriptionRange = sheet.Cells(i, 1, i, reader.FieldCount)
                    descriptionRange.Merge = True
                    descriptionRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                    descriptionRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0))
                End If
            Next
        End If

        CreateSheetColumns(sheet, rowIndex, reader)

        ' 填充数据
        While reader.Read()
            rowIndex += 1
            For i = 1 To reader.FieldCount
                Dim cell = sheet.Cells(rowIndex, i)
                Dim value = reader(i - 1)
                If value Is Nothing OrElse value Is DBNull.Value OrElse
                   String.IsNullOrWhiteSpace(value.ToString()) Then
                    cell.Value = EmptyCellText
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                Else
                    cell.Value = value
                End If
            Next
        End While

        ' 自动列宽按整表计算一次, 再限制最大宽度
        If autoSize AndAlso sheet.Dimension IsNot Nothing Then
            sheet.Cells.AutoFitColumns()
        End If

        For i = 1 To reader.FieldCount
            sheet.Column(i).Width = If(autoSize, Math.Min(sheet.Column(i).Width, MaxColumnWidth), DefaultColumnWidth)
        Next

        Return sheet
    End Function
#End Region

#Region "创建表格列"
    ''' <summary>写入列标题并设置表头样式, 筛选与列格式</summary>
    ''' <param name="worksheet">目标工作表</param>
    ''' <param name="startRowIndex">列标题所在行号</param>
    ''' <param name="reader">数据源</param>
    ''' <remarks>
    ''' 表头设置黄绿背景, 居中, 自动换行, 并启用自动筛选.
    ''' 冻结列标题以下的窗格.
    ''' </remarks>
    Private Sub CreateSheetColumns(worksheet As ExcelWorksheet,
                                   startRowIndex As Integer,
                                   reader As IDataReader)

        If reader.FieldCount = 0 Then Return

        For i = 1 To reader.FieldCount
            Dim fieldName = reader.GetName(i - 1)
            worksheet.Cells(startRowIndex, i).Value = fieldName
            SetColumnFormat(worksheet, i, reader.GetFieldType(i - 1), fieldName)
        Next

        ' 表头样式只作用于数据列范围, 避免影响右侧空白列
        Dim headerRange = worksheet.Cells(startRowIndex, 1, startRowIndex, reader.FieldCount)
        headerRange.AutoFilter = True
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid
        headerRange.Style.Fill.BackgroundColor.SetColor(Color.YellowGreen)
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
        headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center
        headerRange.Style.WrapText = True

        worksheet.View.FreezePanes(startRowIndex + 1, 1)

    End Sub

    ''' <summary>按字段类型设置列格式</summary>
    ''' <param name="worksheet">目标工作表</param>
    ''' <param name="colIndex">列号</param>
    ''' <param name="fieldType">字段类型</param>
    ''' <param name="fieldName">字段名称</param>
    ''' <remarks>
    ''' 文本列使用文本格式, 字段名包含 明细 时自动换行.
    ''' 整数列使用千分位格式.
    ''' 浮点数列在字段名包含 % 时使用百分比格式, 否则保留两位小数.
    ''' 日期列使用 yyyy-MM-dd hh:mm:ss 格式.
    ''' </remarks>
    Private Sub SetColumnFormat(worksheet As ExcelWorksheet,
                                colIndex As Integer,
                                fieldType As Type,
                                fieldName As String)

        Select Case fieldType
            Case GetType(String)
                worksheet.Column(colIndex).Style.Numberformat.Format = "@"
                If fieldName.Contains("明细") Then
                    worksheet.Column(colIndex).Style.WrapText = True
                End If

            Case GetType(Int32), GetType(Int64), GetType(Int16),
                 GetType(UInt32), GetType(UInt64), GetType(UInt16),
                 GetType(Byte), GetType(SByte)
                worksheet.Column(colIndex).Style.Numberformat.Format = "#,##0"

            Case GetType(Decimal), GetType(Double), GetType(Single)
                If fieldName.Contains("%") Then
                    worksheet.Column(colIndex).Style.Numberformat.Format = "0.0%"
                Else
                    worksheet.Column(colIndex).Style.Numberformat.Format = "#,##0.00"
                End If

            Case GetType(DateTime)
                worksheet.Column(colIndex).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss;@"

            Case Else
                worksheet.Column(colIndex).Style.Numberformat.Format = "@"

        End Select

    End Sub
#End Region

End Module
