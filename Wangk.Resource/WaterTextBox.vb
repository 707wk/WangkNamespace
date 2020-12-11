Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' 带水印文本输入框
''' </summary>
Public Class WaterTextBox
    Inherits TextBox

    Private Const EM_SETCUEBANNER = &H1501

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=False)>
    Private Shared Function SendMessage(
                                      hWnd As IntPtr,
                                      Msg As UInt32,
                                      wParam As UInt32,
                                      lParam As String) As IntPtr
    End Function

    Private tooltipStr As String
    Public Property Tooltip As String
        Get
            Return tooltipStr
        End Get
        Set(ByVal value As String)
            SendMessage(Handle, EM_SETCUEBANNER, 0, value)

            tooltipStr = value

        End Set
    End Property
End Class
