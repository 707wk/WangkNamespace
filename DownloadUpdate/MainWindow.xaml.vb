Imports System.IO
Imports System.IO.Compression
Imports System.Net.Http

Class MainWindow

    Private DownloadUrl As String

    Private TempFile As String = IO.Path.GetTempFileName

    Private EXEPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location

    Private RunEXEPath As String

    ''' <summary>
    ''' 是否取消
    ''' </summary>
    Private IsCancel As Boolean = False
    ''' <summary>
    ''' 是否下载完毕
    ''' </summary>
    Private IsDownloaded As Boolean = False

    Private Async Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Dim assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location
        Me.Title = $"{My.Application.Info.Title} V{System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion}"

        Dim tmpCommandLineArgs() As String = Environment.GetCommandLineArgs

        If tmpCommandLineArgs.Count > 1 Then
            DownloadUrl = tmpCommandLineArgs(1)
        Else
            End
        End If

        If tmpCommandLineArgs.Count > 2 Then
            RunEXEPath = tmpCommandLineArgs(2)
        End If

        Try
            Await Task.Run(AddressOf DownloadFile)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "下载出错")

        Finally
            IsDownloaded = True
            Me.Close()
        End Try

    End Sub

    Private Sub DownloadFile()

        Me.Dispatcher.Invoke(Sub()
                                 DownloadText.Text = "下载文件..."
                             End Sub)

#Region "下载文件"
        Dim tmpHttpClient As New HttpClient
        Dim tmpHttpResponseMessage As HttpResponseMessage = tmpHttpClient.GetAsync(DownloadUrl,
                                                                                   HttpCompletionOption.ResponseHeadersRead).GetAwaiter.GetResult

        Dim fileSize As Long = tmpHttpResponseMessage.Content.Headers.ContentLength

        Me.Dispatcher.Invoke(Sub()
                                 DownloadProgressBar.Minimum = 0
                                 DownloadProgressBar.Maximum = fileSize

                                 DownloadProgressText.Text = $"{0:n2}/{fileSize / 1000 / 1000:n2}MB"
                             End Sub)

        Using tmpReadFileStream = tmpHttpClient.GetStreamAsync(DownloadUrl).GetAwaiter.GetResult
            Using tmpSaveFileStream = New FileStream(TempFile, FileMode.Create)

                Dim bArr(1024) As Byte
                Dim readByteSize As Integer
                Dim downloadByteSize As Long

                readByteSize = tmpReadFileStream.Read(bArr, 0, bArr.Count)

                While readByteSize > 0

                    If IsCancel Then
                        Exit Sub
                    End If

                    downloadByteSize += readByteSize

                    Me.Dispatcher.Invoke(Sub()
                                             DownloadProgressBar.Value = downloadByteSize
                                             DownloadProgressText.Text = $"{downloadByteSize / 1000 / 1000:n2}/{fileSize / 1000 / 1000:n2}MB"
                                         End Sub)

                    tmpSaveFileStream.Write(bArr, 0, readByteSize)
                    readByteSize = tmpReadFileStream.Read(bArr, 0, bArr.Count)

                End While

            End Using

        End Using
#End Region

        Me.Dispatcher.Invoke(Sub()
                                 DownloadText.Text = "更新文件..."
                             End Sub)

#Region "删除文件/文件夹"
        For Each item In IO.Directory.GetDirectories(IO.Path.GetDirectoryName(EXEPath))
            IO.Directory.Delete(item, True)
        Next

        For Each item In IO.Directory.GetFiles(IO.Path.GetDirectoryName(EXEPath))

            If IO.Path.GetFileName(item) = IO.Path.GetFileName(EXEPath) Then
                Continue For
            End If

            IO.File.Delete(item)
        Next
#End Region

#Region "重命名"
        IO.File.Move(EXEPath,
                     IO.Path.Combine(IO.Path.GetDirectoryName(EXEPath),
                                     $"Old{IO.Path.GetFileName(EXEPath)}"))
#End Region

#Region "解压文件"
        ZipFile.ExtractToDirectory(TempFile, IO.Path.GetDirectoryName(EXEPath))
#End Region

#Region "删除临时文件"
        IO.File.Delete(TempFile)
#End Region

        Threading.Thread.Sleep(1000)

        ' 更新后运行程序
        If Not String.IsNullOrWhiteSpace(RunEXEPath) Then
            Try
                Process.Start(RunEXEPath)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "启动出错")
            End Try

        End If

    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)

        If Not IsDownloaded Then
            e.Cancel = True
            IsCancel = True
        End If

    End Sub

    Private Sub CancelDownload(sender As Object, e As RoutedEventArgs)
        IsCancel = True
    End Sub

End Class
