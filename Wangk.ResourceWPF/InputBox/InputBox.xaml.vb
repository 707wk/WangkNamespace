Public Class InputBox

    Public Property ShowPrompt As String

    Public Property ShowTitle As String

    Public Property ShowText As String

    Public Overloads Shared Function ShowDialog(prompt As String,
                                                Optional title As String = "",
                                                Optional text As String = "") As String

        Dim tmpWindow As New InputBox With {
            .ShowPrompt = prompt,
            .ShowTitle = title,
            .ShowText = text
        }

        If tmpWindow.ShowDialog() Then
            Return tmpWindow.ShowText
        Else
            Return String.Empty
        End If

    End Function

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        DataContext = Me

        WindowHelper.InitChildWindowStyle(Me)

    End Sub

    Private Sub Window_ContentRendered(sender As Object, e As EventArgs)
        InputTextBox.SelectAll()
    End Sub

    Private Sub okButton_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

End Class
