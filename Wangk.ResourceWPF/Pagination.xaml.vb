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

End Class
