''' <summary>
''' BOM节点信息
''' </summary>
Public Class BOMNodeInfo

    ''' <summary>
    ''' 品号
    ''' </summary>
    Public Property PH As String

    ''' <summary>
    ''' 品名
    ''' </summary>
    Public Property PM As String

    ''' <summary>
    ''' 规格
    ''' </summary>
    Public Property GG As String

    ''' <summary>
    ''' 库存单位
    ''' </summary>
    Public Property Unit As String

    ''' <summary>
    ''' 阶层编号
    ''' </summary>
    Public Property Level As Integer

    ''' <summary>
    ''' 子节点集合
    ''' </summary>
    Public Property Nodes As New List(Of BOMNodeInfo)

    ''' <summary>
    ''' 数量
    ''' </summary>
    Public Property Count As Decimal

    ''' <summary>
    ''' 安装位置
    ''' </summary>
    Public Property Location As String

    ''' <summary>
    ''' 备注
    ''' </summary>
    Public Property Remark As String

    ''' <summary>
    ''' BOM变更次数
    ''' </summary>
    Public Property BOMModiCount As Integer

End Class
