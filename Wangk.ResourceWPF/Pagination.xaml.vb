Imports System.Data
Imports System.Runtime.InteropServices

Public Class Pagination
    Private _TotalCount As Integer
    Private _PageIndex As Integer = 1
    Private _PageSize As Integer = 1
    Private _PageCount As Integer

    ''' <summary>
    ''' 总记录数
    ''' </summary>
    Public Property TotalCount As Integer
        Get
            Return _TotalCount
        End Get
        Set
            _TotalCount = Value

            TotalCountTextBlock.Text = $"共 {TotalCount:n0} 条记录"

            PageCount = Math.Max(Math.Ceiling(TotalCount / PageSize), 1)

        End Set
    End Property

    ''' <summary>
    ''' 页码
    ''' </summary>
    Public Property PageIndex As Integer
        Get
            Return _PageIndex
        End Get
        Set
            If _PageIndex <> Value Then
                _PageIndex = Value

                PageIndexTextBox.Text = $"{PageIndex}"

                'Debug.WriteLine($"PageIndex: {PageIndex - 1} , PageSize: {PageSize} , TotalCount: {TotalCount} , PageCount: {PageCount}")
                RaiseEvent OnChange(PageIndex - 1, PageSize)
            End If


        End Set
    End Property

    ''' <summary>
    ''' 分页大小
    ''' </summary>
    Public Property PageSize As Integer
        Get
            Return _PageSize
        End Get
        Set
            If _PageSize <> Value Then
                _PageSize = Value

                If PageIndex <> 1 Then
                    'Debug.WriteLine("PageSize PageIndex <> 1")
                    PageIndex = 1
                Else
                    'Debug.WriteLine("PageSize OnChange")
                    RaiseEvent OnChange(PageIndex - 1, PageSize)
                End If

            End If

        End Set
    End Property

    ''' <summary>
    ''' 总页数
    ''' </summary>
    Public Property PageCount As Integer
        Get
            Return _PageCount
        End Get
        Set
            _PageCount = Value

            PageCountTextBox.Text = $"{PageCount}"

        End Set
    End Property

    ''' <summary>
    ''' 页码/分页改变
    ''' </summary>
    ''' <param name="pageIndex">页码, 从1开始</param>
    ''' <param name="pageSize">分页大小</param>
    Public Delegate Sub OnChangeHandle(pageIndex As Integer,
                                       pageSize As Integer)
    ''' <summary>
    ''' 页码/分页改变
    ''' </summary>
    Public Event OnChange As OnChangeHandle

#Region "上一页"
    ''' <summary>
    ''' 上一页
    ''' </summary>
    Private Sub PreviousPageAction(sender As Object, e As RoutedEventArgs)
        'Debug.WriteLine(NameOf(PreviousPageAction))
        If PageIndex <= 1 Then
            Return
        End If

        PageIndex -= 1

    End Sub
#End Region

#Region "下一页"
    ''' <summary>
    ''' 下一页
    ''' </summary>
    Private Sub NextPageAction(sender As Object, e As RoutedEventArgs)
        'Debug.WriteLine(NameOf(NextPageAction))
        If PageIndex >= PageCount Then
            Return
        End If

        PageIndex += 1

    End Sub
#End Region

#Region "分页变化"
    ''' <summary>
    ''' 分页变化
    ''' </summary>
    Private Sub PageSizeChanged(sender As Object, e As SelectionChangedEventArgs)
        'Debug.WriteLine(NameOf(PageSizeChanged))
        PageSize = Val(PageSizeComboBox.SelectedItem)

    End Sub
#End Region

#Region "手动输入页码"
    ''' <summary>
    ''' 手动输入页码
    ''' </summary>
    Private Sub PageIndexTextChanged(sender As Object, e As TextChangedEventArgs)
        'Debug.WriteLine(NameOf(PageIndexTextChanged))
        PageIndex = Math.Max(Math.Min(Val(PageIndexTextBox.Text), PageCount), 1)

    End Sub
#End Region

    Public Property PageSizeList As List(Of String)

    Private Sub Pagination_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DataContext = Me

        PageSizeList = New List(Of String) From {
        "10 条/页",
        "20 条/页",
        "50 条/页",
        "100 条/页"
        }

        TotalCount = 0

        PageSizeComboBox.SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' 重新加载数据
    ''' </summary>
    ''' <param name="onlyCurrentPage">仅重新加载当前页数据</param>
    Public Sub Reload(Optional onlyCurrentPage As Boolean = False)

        If onlyCurrentPage Then
            RaiseEvent OnChange(PageIndex - 1, PageSize)
        Else

            If PageIndex <> 1 Then
                PageIndex = 1
            Else
                RaiseEvent OnChange(PageIndex - 1, PageSize)
            End If

        End If

    End Sub

#Region "修复在 VSTO 侧边栏无法选中下拉列表项的问题"
    <DllImport("user32.dll")>
    Public Shared Function GetAsyncKeyState(virtualKeyCode As Integer) As Short

    End Function
    Private Const VK_LBUTTON As Integer = 1

    Private Async Sub ComboBox_DropDownOpened(ByVal sender As Object, ByVal e As EventArgs)
        Dim combo = CType(sender, System.Windows.Controls.ComboBox)

        If Mouse.Captured Is combo Then

            While combo.IsDropDownOpen AndAlso Mouse.Captured Is combo

                If Mouse.LeftButton = MouseButtonState.Released Then
                    AddHandler combo.LostMouseCapture, AddressOf Combo_LostMouseCapture
                    Return
                End If

                Await Task.Delay(1)
            End While
        End If
    End Sub

    Private Sub Combo_LostMouseCapture(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim combo = CType(sender, ComboBox)
        RemoveHandler combo.LostMouseCapture, AddressOf Combo_LostMouseCapture

        If Mouse.Captured IsNot Nothing Then
            Return
        End If

        Dim esckey = (GetAsyncKeyState(System.Windows.Input.KeyInterop.VirtualKeyFromKey(Key.Escape)) And &H8000) = &H8000
        Dim lbutton = (GetAsyncKeyState(VK_LBUTTON) And &H8000) = &H8000

        If esckey OrElse Not lbutton Then
            Return
        End If

        Dim d = TryCast(e.MouseDevice.Target, DependencyObject)

        While d IsNot Nothing
            Dim comboItem = TryCast(d, ComboBoxItem)

            If comboItem IsNot Nothing Then
                Dim index = combo.ItemContainerGenerator.IndexFromContainer(comboItem)

                If index >= 0 Then

                    If comboItem.IsEnabled AndAlso comboItem.IsHitTestVisible AndAlso comboItem.IsMouseOver Then

                        If combo.SelectedIndex <> index Then
                            combo.SelectedIndex = index
                        End If

                        Return
                    End If
                End If
            End If

            d = VisualTreeHelper.GetParent(d)
        End While
    End Sub
#End Region

End Class
