﻿Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>FolderBrowser 的设计器基类</summary>
Public Class FolderNameEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Dim browser As New FolderBrowserDialog()
        If value IsNot Nothing Then
            browser.DirectoryPath = String.Format("{0}", value)
        End If
        If browser.ShowDialog(Nothing) = DialogResult.OK Then
            Return browser.DirectoryPath
        End If
        Return value
    End Function
End Class

''' <summary>Vista 样式的选择文件对话框的基类</summary>
<Description("提供一个Vista样式的选择文件对话框")>
<Editor(GetType(FolderNameEditor), GetType(UITypeEditor))>
Public Class FolderBrowserDialog
    Inherits Component

    <DllImport("shell32.dll")>
    Private Shared Function SHILCreateFromPath(<MarshalAs(UnmanagedType.LPWStr)> ByVal pszPath As String, ByRef ppIdl As IntPtr, ByRef rgflnOut As UInteger) As Integer
    End Function
    <DllImport("shell32.dll")>
    Private Shared Function SHCreateShellItem(ByVal pidlParent As IntPtr, ByVal psfParent As IntPtr, ByVal pidl As IntPtr, ByRef ppsi As IShellItem) As Integer
    End Function
    <DllImport("user32.dll")>
    Private Shared Function GetActiveWindow() As IntPtr
    End Function
    Private Const ERROR_CANCELLED As UInteger = &H800704C7UI

    ''' <summary>初始化 FolderBrowser 的新实例</summary>
    Public Sub New()
    End Sub

    Private mainFrm As Form
    ''' <summary>初始化 FolderBrowser 的新实例</summary>
    ''' <param name="frm">依附的主窗体</param>
    Public Sub New(ByVal frm As Form)
        If frm IsNot Nothing Then mainFrm = frm
    End Sub

    ''' <summary>依附的主窗体</summary>
    Property MainForm As Form = mainFrm

    ''' <summary>获取在 FolderBrowser 中选择的文件夹路径</summary>
    Public Property DirectoryPath() As String

    ''' <summary>向用户显示 FolderBrowser 的对话框</summary>
    Public Function ShowDialog() As DialogResult
        Return ShowDialog(mainFrm)
    End Function
    ''' <summary>向用户显示 FolderBrowser 的对话框</summary>
    ''' <param name="owner">任何实现 System.Windows.Forms.IWin32Window（表示将拥有模式对话框的顶级窗口）的对象。</param>
    Public Function ShowDialog(ByVal owner As IWin32Window) As DialogResult
        Dim hwndOwner As IntPtr = If(owner IsNot Nothing, owner.Handle, GetActiveWindow())
        Dim dialog As IFileOpenDialog = CType(New FileOpenDialog(), IFileOpenDialog)
        Try
            Dim item As IShellItem
            If Not String.IsNullOrEmpty(DirectoryPath) Then
                Dim idl As IntPtr
                Dim atts As UInteger = 0
                If SHILCreateFromPath(DirectoryPath, idl, atts) = 0 Then
                    If SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, item) = 0 Then
                        dialog.SetFolder(item)
                    End If
                End If
            End If
            dialog.SetOptions(FOS.FOS_PICKFOLDERS Or FOS.FOS_FORCEFILESYSTEM)
            Dim hr As UInteger = dialog.Show(hwndOwner)
            If hr = ERROR_CANCELLED Then
                Return DialogResult.Cancel
            End If

            If hr <> 0 Then
                Return DialogResult.Abort
            End If
            dialog.GetResult(item)
            Dim path As String
            item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, path)
            DirectoryPath = path
            Return DialogResult.OK
        Finally
            Marshal.ReleaseComObject(dialog)
        End Try
    End Function

    <ComImport()>
    <Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")>
    Private Class FileOpenDialog
    End Class

    <ComImport()>
    <Guid("42f85136-db7e-439c-85f1-e4075d135fc8")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IFileOpenDialog
        <PreserveSig()>
        Function Show(<[In]()> ByVal parent As IntPtr) As UInteger ' IModalWindow
        Sub SetFileTypes() ' not fully defined
        Sub SetFileTypeIndex(<[In]()> ByVal iFileType As UInteger)
        Sub GetFileTypeIndex(ByRef piFileType As UInteger)
        Sub Advise() ' not fully defined
        Sub Unadvise()
        Sub SetOptions(<[In]()> ByVal fos As FOS)
        Sub GetOptions(ByRef pfos As FOS)
        Sub SetDefaultFolder(ByVal psi As IShellItem)
        Sub SetFolder(ByVal psi As IShellItem)
        Sub GetFolder(ByRef ppsi As IShellItem)
        Sub GetCurrentSelection(ByRef ppsi As IShellItem)
        Sub SetFileName(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszName As String)
        Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String)
        Sub SetTitle(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszTitle As String)
        Sub SetOkButtonLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszText As String)
        Sub SetFileNameLabel(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszLabel As String)
        Sub GetResult(ByRef ppsi As IShellItem)
        Sub AddPlace(ByVal psi As IShellItem, ByVal alignment As Integer)
        Sub SetDefaultExtension(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszDefaultExtension As String)
        Sub Close(ByVal hr As Integer)
        Sub SetClientGuid()
        ' not fully defined
        Sub ClearClientData()
        Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> ByVal pFilter As IntPtr)
        Sub GetResults(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenum As IntPtr) ' not fully defined
        Sub GetSelectedItems(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsai As IntPtr) ' not fully defined
    End Interface

    <ComImport()>
    <Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellItem
        Sub BindToHandler() ' not fully defined
        Sub GetParent() ' not fully defined
        Sub GetDisplayName(<[In]()> ByVal sigdnName As SIGDN, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
        Sub GetAttributes() ' not fully defined
        Sub Compare() ' not fully defined
    End Interface
    Private Enum SIGDN As UInteger
        SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000UI
        SIGDN_DESKTOPABSOLUTEPARSING = &H80028000UI
        SIGDN_FILESYSPATH = &H80058000UI
        SIGDN_NORMALDISPLAY = 0
        SIGDN_PARENTRELATIVE = &H80080001UI
        SIGDN_PARENTRELATIVEEDITING = &H80031001UI
        SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001UI
        SIGDN_PARENTRELATIVEPARSING = &H80018001UI
        SIGDN_URL = &H80068000UI
    End Enum

    <Flags()>
    Private Enum FOS
        FOS_ALLNONSTORAGEITEMS = &H80
        FOS_ALLOWMULTISELECT = &H200
        FOS_CREATEPROMPT = &H2000
        FOS_DEFAULTNOMINIMODE = &H20000000
        FOS_DONTADDTORECENT = &H2000000
        FOS_FILEMUSTEXIST = &H1000
        FOS_FORCEFILESYSTEM = &H40
        FOS_FORCESHOWHIDDEN = &H10000000
        FOS_HIDEMRUPLACES = &H20000
        FOS_HIDEPINNEDPLACES = &H40000
        FOS_NOCHANGEDIR = 8
        FOS_NODEREFERENCELINKS = &H100000
        FOS_NOREADONLYRETURN = &H8000
        FOS_NOTESTFILECREATE = &H10000
        FOS_NOVALIDATE = &H100
        FOS_OVERWRITEPROMPT = 2
        FOS_PATHMUSTEXIST = &H800
        FOS_PICKFOLDERS = &H20
        FOS_SHAREAWARE = &H4000
        FOS_STRICTFILETYPES = 4
    End Enum
End Class