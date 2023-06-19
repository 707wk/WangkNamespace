Imports System.Runtime.InteropServices
Imports System.Windows.Interop

Public Class WindowHelper

    <DllImport("user32.dll")>
    Private Shared Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer

    End Function

    <DllImport("user32.dll")>
    Private Shared Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer

    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetActiveWindow() As IntPtr

    End Function

    Private Const GWL_STYLE = -16
    'Private Const WS_MAXIMIZEBOX = &H10000
    Private Const WS_MINIMIZEBOX = &H20000
    Private Const WS_MINIMIZE = &H20000000L

    ''' <summary>
    ''' 初始化子窗口样式
    ''' </summary>
    Public Shared Sub InitChildWindowStyle(win As Window)

        ' 禁用最小化按钮
        Dim hwnd = New WindowInteropHelper(win).Handle
        Dim value = GetWindowLong(hwnd, GWL_STYLE)
        SetWindowLong(hwnd, GWL_STYLE, value And Not WS_MINIMIZEBOX)

        ' 不显示在任务栏
        win.ShowInTaskbar = False

    End Sub

    ''' <summary>
    ''' 窗口是否最小化
    ''' </summary>
    Public Shared Function IsWindowMini(hWnd As IntPtr) As Boolean
        Dim value = GetWindowLong(hWnd, GWL_STYLE)

        Return (value And WS_MINIMIZE) = WS_MINIMIZE

    End Function

End Class
