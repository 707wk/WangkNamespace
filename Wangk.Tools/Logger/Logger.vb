''' <summary>
''' 日志辅助类
''' 参考LogThis类
''' </summary>
<Obsolete>
Public Class Logger

    ''' <summary>
    ''' 日志等级
    ''' </summary>
    Public Enum LogLevel
        ''' <summary>
        ''' 在开发过程中有用的信息，不会在生产环境中显示
        ''' </summary>
        Level_DEBUG
        ''' <summary>
        ''' 调试生产问题时需要的信息
        ''' </summary>
        Level_INFO
        ''' <summary>
        ''' 虽然发生错误事件, 但仍然不影响系统的继续运行
        ''' </summary>
        Level_WARN
        ''' <summary>
        ''' 严重的错误事件将会导致应用程序的退出
        ''' </summary>
        Level_ERROR
    End Enum

    ''' <summary>
    ''' 写入文件夹
    ''' </summary>
    Public path As String
    ''' <summary>
    ''' 日志保留天数
    ''' </summary>
    Public saveDaysMax As Integer
    ''' <summary>
    ''' 日志的写入级别
    ''' </summary>
    Public writeLevel As LogLevel

    ''' <summary>
    ''' 写入缓存(线程安全)
    ''' </summary>
    Private logsCache As New Concurrent.ConcurrentQueue(Of String)
    ''' <summary>
    ''' 写日志线程实例
    ''' </summary>
    Private workThread As Threading.Thread

    ''' <summary>
    ''' 删除日志线程实例
    ''' </summary>
    Private DeleteThread As Threading.Thread

    ''' <summary>
    ''' 写入日志通知
    ''' </summary>
    Private writeEvent As New Threading.AutoResetEvent(False)

    ''' <summary>
    ''' 构造
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' 初始化
    ''' </summary>
    Public Function Init() As Boolean
        Try
            System.IO.Directory.CreateDirectory(path)

            workThread = New Threading.Thread(AddressOf WriteThread) With {
                .IsBackground = True
            }
            workThread.Start()

            DeleteThread = New Threading.Thread(AddressOf DeleteOldLogs) With {
                .IsBackground = True
            }
            DeleteThread.Start()

        Catch ex As Exception
            Debug.WriteLine($"日志初始化异常:{ex.Message}")
            Return False
        End Try

        Return True
    End Function

#Region "写入线程"
    ''' <summary>
    ''' 写入线程
    ''' </summary>
    Private Sub WriteThread()
        Dim tmpStreamWriter As IO.StreamWriter
        Dim tmpStr As String = ""

        Do
            Do While logsCache.Count
                logsCache.TryDequeue(tmpStr)

                Try
                    tmpStreamWriter = New IO.StreamWriter($"{path}/{Format(Now(), "yyyyMMdd")}.txt", True)
                    tmpStreamWriter.WriteLine(tmpStr)
                    tmpStreamWriter.Close()
                Catch ex As Exception
                    Debug.WriteLine($"日志写入异常:{ex.Message}")
                End Try

            Loop

            '等待写入日志事件
            writeEvent.WaitOne()
        Loop
    End Sub
#End Region

#Region "添加日志"
    ''' <summary>
    ''' 添加日志
    ''' </summary>
    ''' <param name="Context">标题</param>
    ''' <param name="Message">日志内容</param>
    ''' <param name="level">日志等级</param>
    Public Sub LogThis(Context As String,
                       Message As String,
                       level As LogLevel)

        If writeLevel > level Then
            Exit Sub
        End If

        Dim tmpStr As String =
$"------------------------------------------------------------------------------
Times  :  {Now().ToString("yyyy/MM/dd HH:mm:ss.fff")}
level  :  {level.ToString}
Context:  {Context}
Message:  {Message}
"
        logsCache.Enqueue(tmpStr)

        '触发写入日志事件
        writeEvent.Set()
    End Sub
#End Region

    ''' <summary>
    ''' 添加日志
    ''' </summary>
    ''' <param name="Message">日志内容</param>
    Public Sub LogThis(Message As String)
        logsCache.Enqueue(Now().ToString("yyyy/MM/dd HH:mm:ss.fff> ") & Message)

        '触发写入日志事件
        writeEvent.Set()
    End Sub

#Region "删除旧log文件"
    ''' <summary>
    ''' 删除旧log文件
    ''' </summary>
    Private Sub DeleteOldLogs()
        Do
            Try
                Dim nowtime As DateTime = DateTime.Now
                Dim files As String() = IO.Directory.GetFiles(path)
                For Each file In files
                    Dim f As IO.FileInfo = New IO.FileInfo(file)
                    Dim t As TimeSpan = nowtime - f.LastWriteTime
                    If (t.Days >= saveDaysMax) Then
                        f.Delete()
                    End If
                Next
            Catch ex As Exception
            End Try
            Debug.WriteLine("清除旧log文件")

            Threading.Thread.Sleep(60 * 60 * 1000)
        Loop

    End Sub
#End Region

    ''' <summary>
    ''' 析构
    ''' </summary>
    Protected Overrides Sub Finalize()
        workThread?.Abort()

        DeleteThread?.Abort()

        MyBase.Finalize()
    End Sub
End Class
