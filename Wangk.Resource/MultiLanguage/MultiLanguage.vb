Imports System.Collections.Concurrent

Public Class MultiLanguage
#Region "语言选择"
    ''' <summary>
    ''' 语言选择
    ''' </summary>
    Public Enum LANG
        ''' <summary>
        ''' 简体中文
        ''' </summary>
        ZH_CN
        ''' <summary>
        ''' 英文
        ''' </summary>
        EN
    End Enum
#End Region

    '''' <summary>
    '''' 语言包路径
    '''' </summary>
    'Public path As String = "./Lang"

    ''' <summary>
    ''' 语言包索引
    ''' </summary>
    Private LanguageDictionary As New ConcurrentDictionary(Of String, String)

    Public Sub New()

    End Sub

#Region "初始化"
    ''' <summary>
    ''' 初始化
    ''' </summary>
    Public Function Init(ByVal Lang As Wangk.Resource.MultiLanguage.LANG,
                         ByVal AppName As String) As Boolean

        Try
            Dim IOsR As IO.StreamReader = New IO.StreamReader($"./Lang/{AppName}.{[Enum].GetName(GetType(MultiLanguage.LANG), Lang)}.resources")
            Do
                Dim tmpstr As String = IOsR.ReadLine
                '判断数据合法性
                If tmpstr Is Nothing Then
                    Exit Do
                End If

                '判断数据个数
                Dim tmpstr2() As String = tmpstr.Split("_")
                If tmpstr2.Length < 2 Then
                    Continue Do
                End If

                LanguageDictionary.TryAdd(tmpstr2(0), tmpstr2(1))
            Loop
        Catch ex As Exception
            Debug.WriteLine($"语言包初始化异常:{ex.Message}")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "设置显示语言"
    '    ''' <summary>
    '    ''' 设置显示语言
    '    ''' </summary>
    '    Public Sub SetLanguage(CTL As Control)
    '        'Static TmpCTL

    '#Region "单控件"
    '        If TypeOf CTL Is Button Then
    '            Dim TmpCTL = CType(CTL, Button)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '        ElseIf TypeOf CTL Is CheckBox Then
    '            Dim TmpCTL = CType(CTL, CheckBox)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '        ElseIf TypeOf CTL Is Label Then
    '            Dim TmpCTL = CType(CTL, Label)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '        ElseIf TypeOf CTL Is RadioButton Then
    '            Dim TmpCTL = CType(CTL, RadioButton)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)
    '#End Region
    '#Region "表格"

    '#End Region
    '#Region "容器"
    '        ElseIf TypeOf CTL Is FlowLayoutPanel Then
    '            Dim TmpCTL = CType(CTL, FlowLayoutPanel)

    '            For Each i001 As Control In TmpCTL.Controls
    '                SetLanguage(i001)
    '            Next

    '        ElseIf TypeOf CTL Is GroupBox Then
    '            Dim TmpCTL = CType(CTL, GroupBox)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '            For Each i001 As Control In TmpCTL.Controls
    '                SetLanguage(i001)
    '            Next

    '        ElseIf TypeOf CTL Is Panel Then
    '            Dim TmpCTL = CType(CTL, Panel)

    '            For Each i001 As Control In TmpCTL.Controls
    '                SetLanguage(i001)
    '            Next

    '        ElseIf TypeOf CTL Is SplitContainer Then
    '            Dim TmpCTL = CType(CTL, SplitContainer)

    '            For Each i001 As Control In TmpCTL.Panel1.Controls
    '                SetLanguage(i001)
    '            Next
    '            For Each i002 As Control In TmpCTL.Panel2.Controls
    '                SetLanguage(i002)
    '            Next

    '        ElseIf TypeOf CTL Is TabControl Then
    '            Dim TmpCTL = CType(CTL, TabControl)

    '            For Each i001 As TabPage In TmpCTL.TabPages
    '                SetLanguage(i001)
    '            Next

    '        ElseIf TypeOf CTL Is TabPage Then
    '            Dim TmpCTL = CType(CTL, TabPage)
    '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '            For Each i001 As Control In TmpCTL.Controls
    '                SetLanguage(i001)
    '            Next

    '#End Region
    '#Region "菜单和工具栏"

    '#End Region

    '        'ElseIf TypeOf CTL Is Label Then
    '        '    '文本标签
    '        '    TmpCTL = CType(CTL, Label)
    '        '    TmpCTL.Text = GetLang(TmpCTL.Text)
    '        'ElseIf TypeOf CTL Is CheckBox Then
    '        '    '复选框
    '        '    TmpCTL = CType(CTL, CheckBox)
    '        '    TmpCTL.Text = GetLang(TmpCTL.Text)
    '        'ElseIf TypeOf CTL Is MenuStrip Then
    '        '    '菜单栏
    '        '    TmpCTL = CType(CTL, MenuStrip)
    '        '    For Each i As ToolStripMenuItem In TmpCTL.Items
    '        '        i.Text = GetLang(i.Text)

    '        '        For Each j In i.DropDownItems
    '        '            SetLanguage(j)
    '        '        Next
    '        '    Next
    '        'ElseIf TypeOf CTL Is ToolStrip OrElse
    '        '    TypeOf CTL Is StatusStrip Then
    '        '    '工具栏
    '        '    '状态栏
    '        '    TmpCTL = CType(CTL, ToolStrip)
    '        '    For Each i In TmpCTL.Items
    '        '        If TypeOf i Is ToolStripButton Then
    '        '            TmpCTL = CType(i, ToolStripButton)
    '        '            TmpCTL.Text = GetLang(TmpCTL.Text)
    '        '        ElseIf TypeOf i Is ToolStripLabel Then
    '        '            TmpCTL = CType(i, ToolStripLabel)
    '        '            TmpCTL.Text = GetLang(TmpCTL.Text)
    '        '        ElseIf TypeOf i Is ToolStripSplitButton Then
    '        '            TmpCTL = CType(i, ToolStripSplitButton)
    '        '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '        '            For Each j In TmpCTL.DropDownItems
    '        '                j.Text = GetLang(j.Text)
    '        '            Next
    '        '        ElseIf TypeOf i Is ToolStripDropDownButton Then
    '        '            TmpCTL = CType(i, ToolStripDropDownButton)
    '        '            TmpCTL.Text = GetLang(TmpCTL.Text)

    '        '            For Each j In TmpCTL.DropDownItems
    '        '                j.Text = GetLang(j.Text)
    '        '            Next
    '        '        End If
    '        '    Next
    '        'ElseIf TypeOf CTL Is DataGridView Then
    '        '    '数据表格
    '        '    TmpCTL = CType(CTL, DataGridView)
    '        '    For Each i In TmpCTL.Columns
    '        '        i.HeaderText = GetLang(i.HeaderText)
    '        '    Next
    '        'ElseIf TypeOf CTL Is TabControl Then
    '        '    '选项卡
    '        '    TmpCTL = CType(CTL, TabControl)
    '        '    For Each i As TabPage In TmpCTL.TabPages
    '        '        i.Text = GetLang(i.Text)

    '        '        For Each j In i.Controls
    '        '            SetLanguage(j)
    '        '        Next
    '        '    Next
    '        'ElseIf TypeOf CTL Is ListView Then
    '        '    '列表
    '        '    TmpCTL = CType(CTL, ListView)
    '        '    For Each i As ColumnHeader In TmpCTL.Columns
    '        '        i.Text = GetLang(i.Text)
    '        '    Next
    '        'ElseIf TypeOf CTL Is GroupBox Then
    '        '    '分组框
    '        '    TmpCTL = CType(CTL, GroupBox)
    '        '    TmpCTL.text = GetLang(TmpCTL.text)
    '        '    For Each i In TmpCTL.Controls
    '        '        SetLanguage(i)
    '        '    Next
    '        'End If
    '    End Sub
#End Region

#Region "获取显示文本"
    ''' <summary>
    ''' 获取显示文本
    ''' </summary>
    Public Function GetS(ByVal KeyStr As String) As String
        Try
            Dim tmpStr As String = Nothing
            If LanguageDictionary.TryGetValue(KeyStr, tmpStr) Then
                Return tmpStr
            End If
            Return KeyStr
        Catch ex As Exception
            Return KeyStr
        End Try
    End Function
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
