﻿Imports System.Configuration
Imports System.IO
Imports System.IO.Compression
Imports System.Net
Imports System.Net.Http

Class MainWindow

    Private DownloadUrl As String

    Private TempFile As String = Path.Combine(Path.GetTempPath(), $"{Date.Now:yyyyMMddHHmmssff}{Guid.NewGuid().ToString().Replace("-", "")}")

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

        ' win7系统使用
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or
            SecurityProtocolType.Tls Or
            SecurityProtocolType.Tls11 Or
            SecurityProtocolType.Tls12

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
            Debug.WriteLine(ex)
            MsgBox($"程序下载失败,请到程序下载页面手动下载最新版本", MsgBoxStyle.Question, "下载出错")

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
        Dim tmpHttpResponseMessage As HttpResponseMessage = tmpHttpClient.GetAsync(DownloadUrl, HttpCompletionOption.ResponseHeadersRead).GetAwaiter.GetResult

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
        ' 获取忽略文件夹
        Dim IgnoreFolderItems As New HashSet(Of String)
        Dim IgnoreFoldersStr = ConfigurationManager.AppSettings.Get("IgnoreFolders")
        For Each item In IgnoreFoldersStr.Split(";")
            If String.IsNullOrWhiteSpace(item) Then
                Continue For
            End If

            If IgnoreFolderItems.Contains(item.ToLower) Then
                Continue For
            End If

            IgnoreFolderItems.Add(item.ToLower)

        Next

        ' 删除文件夹
        For Each item In IO.Directory.GetDirectories(IO.Path.GetDirectoryName(EXEPath))
            ' 跳过忽略的文件夹
            If IgnoreFolderItems.Contains(IO.Path.GetFileName(item).ToLower) Then
                Continue For
            End If

            IO.Directory.Delete(item, True)
        Next

        ' 删除文件
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
