''' <summary>
''' 终端调试
''' </summary>
<Obsolete>
Public NotInheritable Class ConsoleDebug
    Private Sub New()
    End Sub

    '''' <summary>
    '''' 调用控制台窗口
    '''' </summary>
    Private Declare Function AllocConsole Lib "kernel32" Alias "AllocConsole" () As Boolean

    '''' <summary>
    '''' 释放控制台窗口
    '''' </summary>
    '<DllImport(”kernel32.dll”)>
    'Public Shared Function FreeConsole() As Boolean
    'End Function
    ''' <summary>
    ''' 释放控制台窗口
    ''' </summary>
    Private Declare Function FreeConsole Lib "kernel32" Alias "FreeConsole" () As Boolean

    ''' <summary>
    ''' 打开终端
    ''' </summary>
    Public Shared Sub Open()
        AllocConsole()
    End Sub

    ''' <summary>
    ''' 关闭终端
    ''' </summary>
    Public Shared Sub Close()
        FreeConsole()
    End Sub
End Class
