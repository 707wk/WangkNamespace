Public Class Toast

    Public Shared Sub ShowInfo(parent As DependencyObject,
                               message As String,
                               Optional timeoutInterval As Integer = 1500)

        ShowInfo(Window.GetWindow(parent), message, timeoutInterval)

    End Sub

    Public Shared Async Sub ShowInfo(parent As Window,
                                     message As String,
                                     Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .ShowActivated = False
        }

        If parent Is Nothing Then
            tmpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        Else
            tmpWindow.Owner = parent
        End If

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("NormalBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("NormalImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Sub ShowSuccess(parent As DependencyObject,
                               message As String,
                               Optional timeoutInterval As Integer = 1500)

        ShowSuccess(Window.GetWindow(parent), message, timeoutInterval)

    End Sub

    Public Shared Async Sub ShowSuccess(parent As Window,
                                        message As String,
                                        Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .ShowActivated = False
        }

        If parent Is Nothing Then
            tmpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        Else
            tmpWindow.Owner = parent
        End If

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("SuccessBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("SuccessImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Sub ShowWarning(parent As DependencyObject,
                                  message As String,
                                  Optional timeoutInterval As Integer = 1500)

        ShowWarning(Window.GetWindow(parent), message, timeoutInterval)

    End Sub

    Public Shared Async Sub ShowWarning(parent As Window,
                                        message As String,
                                        Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .ShowActivated = False
        }

        If parent Is Nothing Then
            tmpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        Else
            tmpWindow.Owner = parent
        End If

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("WarningBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("WarningImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Sub ShowError(parent As DependencyObject,
                                message As String,
                                Optional timeoutInterval As Integer = 1500)

        ShowError(Window.GetWindow(parent), message, timeoutInterval)

    End Sub

    Public Shared Async Sub ShowError(parent As Window,
                                  message As String,
                                  Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .ShowActivated = False
        }

        If parent Is Nothing Then
            tmpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        Else
            tmpWindow.Owner = parent
        End If

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("ErrorBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("ErrorImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

End Class
