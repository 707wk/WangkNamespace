Imports System.Data
Imports System.Runtime.CompilerServices
Imports OfficeOpenXml

Public Module EPPlusHelper

#Region "辅助方法"

    ''' <summary>获取 IDataReader 的字段名列表</summary>
    Private Function GetReaderFieldNames(reader As IDataReader) As List(Of String)
        Dim names As New List(Of String)
        For i = 0 To reader.FieldCount - 1
            names.Add(reader.GetName(i))
        Next
        Return names
    End Function

    ''' <summary>设置标题单元格样式（黄绿背景、居中、换行）</summary>
    Private Sub SetHeaderCellStyle(cell As ExcelRangeBase)   ' 改为 ExcelRangeBase
        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.YellowGreen)
        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center
        cell.Style.WrapText = True
    End Sub

    ''' <summary>写入空值单元格（显示 "/" 并居中）</summary>
    Private Sub WriteEmptyCell(cell As ExcelRangeBase)       ' 改为 ExcelRangeBase
        cell.Value = "/"
        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center
    End Sub

    ''' <summary>自动调整列宽并限制最大宽度</summary>
    Private Sub ApplyAutoFitAndMaxWidth(sheet As ExcelWorksheet, fieldCount As Integer, maxWidth As Double)
        sheet.Cells.AutoFitColumns()
        For i = 1 To fieldCount
            sheet.Column(i).Width = Math.Min(sheet.Column(i).Width, maxWidth)
        Next
    End Sub

    ''' <summary>为所有有数据的行设置自定义行高</summary>
    Private Sub SetCustomHeightForAllRows(sheet As ExcelWorksheet)
        If sheet.Dimension Is Nothing Then Return
        For i = 1 To sheet.Dimension.End.Row
            sheet.Row(i).CustomHeight = True
        Next
    End Sub

#End Region

#Region "将数据导入表格"

    <Extension()>
    Public Function Add(worksheets As ExcelWorksheets,
                        Name As String,
                        reader As IDataReader,
                        Optional descriptions As String() = Nothing,
                        Optional autoSize As Boolean = False) As ExcelWorksheet

        Dim sheet = worksheets.Add(Name)
        Dim rowIndex = 1

        ' 写入数据说明
        If descriptions IsNot Nothing AndAlso descriptions.Length > 0 Then
            rowIndex = descriptions.Length + 1
            For i = 1 To descriptions.Length
                sheet.Row(i).Merged = True
                sheet.Cells(i, 1).Value = descriptions(i - 1)
                sheet.Row(i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                sheet.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0))
            Next
        End If

        ' 创建列标题并设置格式
        CreateSheetColumns(sheet, rowIndex, reader)

        ' 填充数据
        While reader.Read()
            rowIndex += 1
            For i = 1 To reader.FieldCount
                Dim value = reader(i - 1)
                Dim cell = sheet.Cells(rowIndex, i)
                If value IsNot DBNull.Value AndAlso Not String.IsNullOrWhiteSpace(value.ToString()) Then
                    cell.Value = value
                Else
                    WriteEmptyCell(cell)
                End If
            Next
        End While

        ' 调整列宽
        If autoSize Then
            ApplyAutoFitAndMaxWidth(sheet, reader.FieldCount, 20)
        Else
            sheet.Columns.Width = 15
        End If

        SetCustomHeightForAllRows(sheet)
        Return sheet
    End Function

#End Region

#Region "创建表格列"

    Private Sub CreateSheetColumns(workSheet As ExcelWorksheet,
                                   startRowIndex As Integer,
                                   reader As IDataReader)

        Dim fieldNames = GetReaderFieldNames(reader)
        For i = 1 To fieldNames.Count
            workSheet.Cells(startRowIndex, i).Value = fieldNames(i - 1)
        Next

        ' 标题行样式与筛选
        Dim headerRange = workSheet.Cells(startRowIndex, 1, startRowIndex, fieldNames.Count)
        headerRange.AutoFilter = True
        For Each cell In headerRange
            SetHeaderCellStyle(cell)
        Next

        ' 冻结首行
        workSheet.View.FreezePanes(startRowIndex + 1, 1)

        ' 设置列格式
        For i = 1 To reader.FieldCount
            Dim fieldType = reader.GetFieldType(i - 1)
            Dim fieldName = fieldNames(i - 1)
            SetColumnFormat(workSheet, i, fieldType, fieldName)
        Next

    End Sub

#End Region

#Region "追加数据"

    <Extension()>
    Public Function Append(worksheet As ExcelWorksheet,
                           reader As IDataReader) As ExcelWorksheet

        ' 验证是否为 Add 创建的工作表（存在自动筛选）
        If worksheet.AutoFilter Is Nothing Then
            Throw New InvalidOperationException("当前工作表没有设置自动筛选，不是由 Add 方法创建。")
        End If

        Dim headerRow = worksheet.AutoFilter.Address.Start.Row

        ' 获取现有列名 -> 列索引（不区分大小写）
        Dim existingCols As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Dim colIdx = 1
        While True
            Dim colName = worksheet.Cells(headerRow, colIdx).Text
            If String.IsNullOrWhiteSpace(colName) Then Exit While
            existingCols(colName) = colIdx
            colIdx += 1
        End While
        Dim maxExistingCol = colIdx - 1

        ' 获取 reader 字段
        Dim readerFields = GetReaderFieldNames(reader)

        ' 匹配列：现有列使用原索引，新列追加到右侧
        Dim finalMap As New Dictionary(Of String, Integer)
        Dim newFields As New List(Of String)
        For Each field In readerFields
            If existingCols.ContainsKey(field) Then
                finalMap(field) = existingCols(field)
            Else
                finalMap(field) = maxExistingCol + newFields.Count + 1
                newFields.Add(field)
            End If
        Next

        ' 添加新列（标题、样式、格式、列宽）
        If newFields.Count > 0 Then
            Dim startNewCol = maxExistingCol + 1
            For idx = 0 To newFields.Count - 1
                Dim fieldName = newFields(idx)
                Dim newColIdx = startNewCol + idx

                ' 写入标题并应用样式
                Dim headerCell = worksheet.Cells(headerRow, newColIdx)
                headerCell.Value = fieldName
                SetHeaderCellStyle(headerCell)

                ' 设置列格式
                Dim fieldType = reader.GetFieldType(Array.IndexOf(readerFields.ToArray(), fieldName))
                SetColumnFormat(worksheet, newColIdx, fieldType, fieldName)

                ' 列宽参考第一列
                Dim widthRef = If(maxExistingCol >= 1, worksheet.Column(1).Width, 15.0)
                worksheet.Column(newColIdx).Width = widthRef
            Next

            ' 更新自动筛选范围
            Dim newMaxCol = maxExistingCol + newFields.Count
            worksheet.Cells(headerRow, 1, headerRow, newMaxCol).AutoFilter = True
        End If

        ' 确定起始数据行
        Dim startDataRow = headerRow + 1
        Dim lastDataRow = headerRow
        If worksheet.Dimension IsNot Nothing Then
            lastDataRow = worksheet.Dimension.End.Row
        End If
        Dim currentRow = If(lastDataRow >= startDataRow, lastDataRow + 1, startDataRow)

        ' 追加数据
        While reader.Read()
            For Each field In readerFields
                Dim col = finalMap(field)
                Dim rawValue = reader(field)
                Dim cell = worksheet.Cells(currentRow, col)

                If rawValue Is DBNull.Value OrElse rawValue Is Nothing OrElse String.IsNullOrWhiteSpace(rawValue.ToString()) Then
                    WriteEmptyCell(cell)
                Else
                    cell.Value = rawValue
                End If
            Next
            worksheet.Row(currentRow).CustomHeight = True
            currentRow += 1
        End While

        Return worksheet
    End Function

#End Region

#Region "列格式设置"

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