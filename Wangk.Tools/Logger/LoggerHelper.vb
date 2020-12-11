Imports Wangk.Tools.Logger
''' <summary>
''' 全局日志管理器
''' </summary>
<Obsolete>
Public NotInheritable Class LoggerHelper
    '单例模式

    Private Sub New()
    End Sub

    ''' <summary>
    ''' 实例
    ''' </summary>
    Private Shared instance As Logger

    ''' <summary>
    ''' 参数
    ''' </summary>
    Public Shared ReadOnly Property Log As Logger
        Get
            If instance Is Nothing Then
                Init()

            End If

            Return instance
        End Get
    End Property

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <param name="path">写入文件夹(默认 ./Logs)</param>
    ''' <param name="saveDaysMax">日志保留天数(默认 30)</param>
    ''' <param name="writelevel">日志的写入级别(默认 Level_DEBUG)</param>
    Public Shared Sub Init(Optional path As String = "./Logs",
                           Optional saveDaysMax As Integer = 30,
                           Optional writeLevel As LogLevel = LogLevel.Level_DEBUG)

        instance = New Logger With {
                    .path = path,
                    .saveDaysMax = saveDaysMax,
                    .writeLevel = writeLevel
                }
        instance.Init()

    End Sub

End Class
