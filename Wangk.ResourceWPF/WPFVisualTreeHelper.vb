''' <summary>
''' VisualTreeHelper辅助模块
''' </summary>
Public Class WPFVisualTreeHelper

    ''' <summary>
    ''' 查找子节点
    ''' </summary>
    ''' <typeparam name="ChildT">子节点类型</typeparam>
    ''' <param name="parent">父对象</param>
    ''' <returns>子节点列表</returns>
    Public Shared Function FindVisualChild(Of ChildT As DependencyObject)(parent As DependencyObject) As List(Of ChildT)
        Dim tmpList As New List(Of ChildT)

        For childindex = 0 To VisualTreeHelper.GetChildrenCount(parent) - 1
            Dim childItem As DependencyObject = VisualTreeHelper.GetChild(parent, childindex)

            If TypeOf childItem Is ChildT Then
                tmpList.Add(childItem)
            End If

            tmpList.AddRange(FindVisualChild(Of ChildT)(childItem))

        Next

        Return tmpList
    End Function

    ''' <summary>
    ''' 查找子节点
    ''' </summary>
    ''' <typeparam name="ChildT">子节点类型</typeparam>
    ''' <param name="parent">父对象</param>
    ''' <returns>子节点</returns>
    Public Shared Function GetVisualChild(Of ChildT As DependencyObject)(parent As DependencyObject) As ChildT

        For childindex = 0 To VisualTreeHelper.GetChildrenCount(parent) - 1
            Dim childItem As DependencyObject = VisualTreeHelper.GetChild(parent, childindex)

            If TypeOf childItem Is ChildT Then
                Return childItem
            End If

            Dim tmpChildT = GetVisualChild(Of ChildT)(childItem)
            If tmpChildT IsNot Nothing Then
                Return tmpChildT
            End If

        Next

        Return Nothing
    End Function

End Class
