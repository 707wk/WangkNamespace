''' <summary>
''' 软件更新信息
''' </summary>
Public Class AppUpdateInfo
    ''' <summary>
    ''' 版本号(格式: 1.0.0.0)
    ''' </summary>
    Public Version As String
    ''' <summary>
    ''' 更新说明
    ''' </summary>
    Public UpdateInfo() As String
    ''' <summary>
    ''' 下载地址
    ''' </summary>
    Public DownloadPath As String
    ''' <summary>
    ''' 是否必须更新
    ''' </summary>
    Public MustUpdate As Boolean

End Class
