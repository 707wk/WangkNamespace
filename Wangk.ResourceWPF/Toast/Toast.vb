Public Class Toast

    Public Shared Async Sub ShowInfo(parent As Window,
                                 message As String,
                                 Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .Owner = parent,
            .ShowActivated = False
        }
        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("NormalBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("NormalImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Async Sub ShowSuccess(parent As Window,
                                    message As String,
                                    Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .Owner = parent,
            .ShowActivated = False
        }

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("SuccessBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("SuccessImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Async Sub ShowWarning(parent As Window,
                                    message As String,
                                    Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .Owner = parent,
            .ShowActivated = False
        }

        tmpWindow.ToastText.Text = message

        tmpWindow.ToastBorder.Style = tmpWindow.FindResource("WarningBorderStyle")
        tmpWindow.ToastIco.Style = tmpWindow.FindResource("WarningImageStyle")

        tmpWindow.Show()

        Await Task.Run(Sub()
                           Threading.Thread.Sleep(timeoutInterval)
                       End Sub)

        tmpWindow.Close()

    End Sub

    Public Shared Async Sub ShowError(parent As Window,
                                  message As String,
                                  Optional timeoutInterval As Integer = 1500)

        Dim tmpWindow As New ToastWindow With {
            .Owner = parent,
            .ShowActivated = False
        }

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
