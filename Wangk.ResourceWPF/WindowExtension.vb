Imports System.Runtime.CompilerServices
Imports System.Windows.Interop

Module WindowExtension

    ''' <summary>
    ''' 设置WPF窗口的所有者
    ''' </summary>
    <Extension()>
    Public Sub SetOwner(windowInstance As Window, parentIntPtr As IntPtr)

        Dim tmpWindowHelper = New WindowInteropHelper(windowInstance) With {
            .Owner = parentIntPtr
        }

    End Sub

End Module
