Imports System.Runtime.InteropServices
''' <summary>
''' 系统睡眠管理
''' </summary>
Public Class SystemSleepManagement

    <DllImport("kernel32.dll")>
    Private Shared Function SetThreadExecutionState(flags As ExecutionFlag) As UInteger

    End Function

    Enum ExecutionFlag As UInteger
        System = &H1UI
        Display = &H2UI
        Continuous = &H80000000UI
    End Enum

    ''' <summary>
    ''' 阻止系统睡眠，直到线程结束恢复睡眠策略
    ''' </summary>
    ''' <param name="includeDisplay">是否阻止关闭显示器</param>
    Public Shared Sub PreventSleep(Optional includeDisplay As Boolean = False)
        If includeDisplay Then
            SetThreadExecutionState(ExecutionFlag.System Or ExecutionFlag.Display Or ExecutionFlag.Continuous)
        Else
            SetThreadExecutionState(ExecutionFlag.System Or ExecutionFlag.Continuous)
        End If
    End Sub

    ''' <summary>
    ''' 恢复系统睡眠策略
    ''' </summary>
    Public Shared Sub RestoreSleep()
        SetThreadExecutionState(ExecutionFlag.Continuous)
    End Sub

    ''' <summary>
    ''' 重置系统睡眠计时器
    ''' </summary>
    ''' <param name="includeDisplay">是否阻止关闭显示器</param>
    Public Shared Sub ResetSleepTimer(Optional includeDisplay As Boolean = False)
        If includeDisplay Then
            SetThreadExecutionState(ExecutionFlag.System Or ExecutionFlag.Display)
        Else
            SetThreadExecutionState(ExecutionFlag.System)
        End If
    End Sub

End Class
