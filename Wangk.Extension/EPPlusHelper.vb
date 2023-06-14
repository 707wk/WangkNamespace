Imports System.Data
Imports System.Runtime.CompilerServices
Imports OfficeOpenXml

Public Module EPPlusHelper

#Region "将数据导入表格"
    ''' <summary>
    ''' 将数据导入表格
    ''' </summary>
    ''' <param name="worksheets">表格集合</param>
    ''' <param name="Name">表格名称</param>
    ''' <param name="reader">数据源</param>
    ''' <param name="descriptions">数据说明</param>
    <Extension()>
    Public Function Add(worksheets As ExcelWorksheets,
                        Name As String,
                        reader As IDataReader,
                        Optional descriptions As String() = Nothing) As ExcelWorksheet

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
        workSheet.Row(1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        workSheet.Row(1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
        workSheet.Row(1).Style.WrapText = True

        ' 首行冻结
        workSheet.View.FreezePanes(startRowIndex + 1, 1)

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

#Region "将BOM导入表格"
    ''' <summary>
    ''' 将BOM导入表格
    ''' </summary>
    ''' <param name="worksheets">表格集合</param>
    ''' <param name="rootNode">BOM根节点</param>
    <Extension()>
    Public Function Add(worksheets As ExcelWorksheets,
                        rootNode As BOMNodeInfo) As ExcelWorksheet

        Dim tmpWorkSheet = worksheets.Add(rootNode.PH)

        Dim levelColumnID = 2
        Dim LevelMaximum = GetBOMMaxLevel(rootNode)
        Dim PHColumnID = levelColumnID + LevelMaximum
        Dim PMColumnID = PHColumnID + 1
        Dim GGColumnID = PHColumnID + 2
        Dim UnitColumnID = PHColumnID + 3
        Dim CountColumnID = PHColumnID + 4
        Dim PriceColumnID = PHColumnID + 5
        Dim LocationColumnID = PHColumnID + 6
        Dim RemarkColumnID = PHColumnID + 7
        Dim BOMModiCountColumnID = PHColumnID + 8

        Dim MaterialFirstRowID = 4

        ' 表头
        tmpWorkSheet.Cells(1, 1).Value = "规格"
        tmpWorkSheet.Cells(1, 2).Value = $"{rootNode.PM} / {rootNode.GG}"
        tmpWorkSheet.Cells(1, 2, 1, BOMModiCountColumnID).Merge = True

        ' 列标题
        tmpWorkSheet.Cells(2, 1).Value = "序号"
        tmpWorkSheet.Cells(2, 1, 3, 1).Merge = True

        tmpWorkSheet.Cells(2, levelColumnID).Value = "阶层"
        tmpWorkSheet.Cells(2, levelColumnID, 2, PHColumnID - 1).Merge = True

        For levelID = 1 To LevelMaximum
            tmpWorkSheet.Cells(3, levelColumnID + levelID - 1).Value = levelID
        Next

        tmpWorkSheet.Cells(2, PHColumnID).Value = "品号"
        tmpWorkSheet.Cells(2, PHColumnID, 3, PHColumnID).Merge = True

        tmpWorkSheet.Cells(2, PMColumnID).Value = "品名"
        tmpWorkSheet.Cells(2, PMColumnID, 3, PMColumnID).Merge = True

        tmpWorkSheet.Cells(2, GGColumnID).Value = "规格"
        tmpWorkSheet.Cells(2, GGColumnID, 3, GGColumnID).Merge = True

        tmpWorkSheet.Cells(2, UnitColumnID).Value = "库存单位"
        tmpWorkSheet.Cells(2, UnitColumnID, 3, UnitColumnID).Merge = True

        tmpWorkSheet.Cells(2, CountColumnID).Value = "数量"
        tmpWorkSheet.Cells(2, CountColumnID, 3, CountColumnID).Merge = True

        tmpWorkSheet.Cells(2, PriceColumnID).Value = "单价"
        tmpWorkSheet.Cells(2, PriceColumnID, 3, PriceColumnID).Merge = True

        tmpWorkSheet.Cells(2, LocationColumnID).Value = "安装位置"
        tmpWorkSheet.Cells(2, LocationColumnID, 3, LocationColumnID).Merge = True

        tmpWorkSheet.Cells(2, RemarkColumnID).Value = "备注"
        tmpWorkSheet.Cells(2, RemarkColumnID, 3, RemarkColumnID).Merge = True

        tmpWorkSheet.Cells(2, BOMModiCountColumnID).Value = "BOM变更次数"
        tmpWorkSheet.Cells(2, BOMModiCountColumnID, 3, BOMModiCountColumnID).Merge = True

        ' 设置标题背景色
        tmpWorkSheet.Cells(1, 1, 3, BOMModiCountColumnID).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
        tmpWorkSheet.Cells(1, 1, 3, BOMModiCountColumnID).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.YellowGreen)

        ' 反向生成节点树
        Dim TreeNodeStack As New Stack(Of BOMNodeInfo)
        TreeNodeStack.Push(rootNode)

        Dim rID = 0
        Do While TreeNodeStack.Count > 0

            Dim tmpNode = TreeNodeStack.Pop

            tmpWorkSheet.Cells(MaterialFirstRowID + rID, 1).Value = rID + 1

            tmpWorkSheet.Cells(MaterialFirstRowID + rID, levelColumnID + tmpNode.Level - 1).Value = "●"

            tmpWorkSheet.Cells(MaterialFirstRowID + rID, PHColumnID).Value = tmpNode.PH
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, PMColumnID).Value = tmpNode.PM
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, GGColumnID).Value = tmpNode.GG
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, UnitColumnID).Value = tmpNode.Unit
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, CountColumnID).Value = tmpNode.Count
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, PriceColumnID).Value = String.Empty
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, LocationColumnID).Value = tmpNode.Location
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, RemarkColumnID).Value = tmpNode.Remark
            tmpWorkSheet.Cells(MaterialFirstRowID + rID, BOMModiCountColumnID).Value = tmpNode.BOMModiCount

            Dim tmpList As New List(Of BOMNodeInfo)
            For Each item In tmpNode.Nodes
                tmpList.Insert(0, item)
            Next

            For Each item In tmpList
                TreeNodeStack.Push(item)
            Next

            rID += 1
        Loop
        Dim MaterialLastRowID = MaterialFirstRowID + rID - 1

        ' 对齐方式
        tmpWorkSheet.Cells(1, 1, 3, BOMModiCountColumnID).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
        tmpWorkSheet.Cells(1, 1, 3, BOMModiCountColumnID).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center

        tmpWorkSheet.Cells("A:A").Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center
        tmpWorkSheet.Cells("A:A").Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center

        ' 单元格值格式
        tmpWorkSheet.Cells(1, PHColumnID, MaterialLastRowID, PHColumnID).Style.Numberformat.Format = "@"
        tmpWorkSheet.Cells(1, CountColumnID, MaterialLastRowID, CountColumnID).Style.Numberformat.Format = "#,##0.00"
        tmpWorkSheet.Cells(1, PriceColumnID, MaterialLastRowID, PriceColumnID).Style.Numberformat.Format = "#,##0.0000"
        tmpWorkSheet.Cells(1, CountColumnID, BOMModiCountColumnID, CountColumnID).Style.Numberformat.Format = "#,##0"

        ' 单元格边框
        tmpWorkSheet.Cells(1, 1, MaterialLastRowID, BOMModiCountColumnID).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        tmpWorkSheet.Cells(1, 1, MaterialLastRowID, BOMModiCountColumnID).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        tmpWorkSheet.Cells(1, 1, MaterialLastRowID, BOMModiCountColumnID).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        tmpWorkSheet.Cells(1, 1, MaterialLastRowID, BOMModiCountColumnID).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin

        ' 显示字体
        tmpWorkSheet.Cells(1, 1, tmpWorkSheet.Dimension.End.Row, tmpWorkSheet.Dimension.End.Column).Style.Font.Name = "宋体"

        ' 自动列宽
        tmpWorkSheet.Cells.AutoFitColumns()
        ' 阶层固定列宽
        For colID = levelColumnID To PHColumnID - 1
            tmpWorkSheet.Column(colID).Width = 3
        Next
        ' BOM变更次数字段
        tmpWorkSheet.Column(BOMModiCountColumnID).Width = 14

        ' 限制最大宽度
        For i001 = 1 To BOMModiCountColumnID
            tmpWorkSheet.Column(i001).Width = Math.Min(tmpWorkSheet.Column(i001).Width, 50)
        Next

        ' 自动行高
        For i001 = 1 To tmpWorkSheet.Dimension.End.Row
            tmpWorkSheet.Row(i001).CustomHeight = True
        Next
        ' 首行两倍高度
        tmpWorkSheet.Row(1).Height *= 2

        Return tmpWorkSheet
    End Function
#End Region


#Region "获取BOM最大阶层数"
    ''' <summary>
    ''' 获取BOM最大阶层数
    ''' </summary>
    Public Function GetBOMMaxLevel(value As BOMNodeInfo) As Integer

        If value.Nodes.Count = 0 Then
            Return value.Level
        End If

        Dim childLevel = 1

        For Each item In value.Nodes
            Dim itemLevel = GetBOMMaxLevel(item)

            If itemLevel > childLevel Then
                childLevel = itemLevel
            End If
        Next

        Return childLevel

    End Function
#End Region

End Module
