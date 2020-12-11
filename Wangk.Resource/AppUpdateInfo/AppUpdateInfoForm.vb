Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Class AppUpdateInfoForm
    ''' <summary>
    ''' 当前版本号(格式: 1.0.0.0)
    ''' </summary>
    Public OldVersion As String
    ''' <summary>
    ''' 更新检测地址
    ''' </summary>
    Public AppUpdateInfoPath As String

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim request As System.Net.HttpWebRequest = WebRequest.Create(AppUpdateInfoPath)
        request.Method = "GET"
        request.ContentType = "application/json"

        '接收数据
        Using resp As HttpWebResponse = request.GetResponse()
            Using Stream As Stream = resp.GetResponseStream()
                Using reader As StreamReader = New StreamReader(Stream, Encoding.UTF8)
                    e.Result = JsonConvert.DeserializeObject(Of AppUpdateInfo)(reader.ReadToEnd())
                End Using
            End Using
        End Using

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If e.Error Is Nothing Then
            Dim tmp As AppUpdateInfo = e.Result

            Dim oldVersionItems = OldVersion.Split(".")
            Dim versionItems = tmp.Version.Split(".")

            For i001 = 0 To 4 - 1
                If Val(oldVersionItems(i001)) < Val(versionItems(i001)) Then
                    Exit For
                End If

                If i001 = 4 - 1 Then
                    Me.Dispose()
                    Exit Sub
                End If

            Next

            'If String.Compare(OldVersion, tmp.Version, True) >= 0 Then
            '    Me.Dispose()
            '    Exit Sub
            'End If

            OldVersionText.Text = OldVersion
            NewVersionText.Text = tmp.Version
            If tmp.UpdateInfo IsNot Nothing Then
                For i001 = 0 To tmp.UpdateInfo.Count - 1
                    UpdateInfoText.AppendText($"{i001 + 1}. {tmp.UpdateInfo(i001)}{vbCrLf}")
                Next
            End If

            JumpButton.Tag = tmp.DownloadPath

            IgnoreButton.Enabled = Not tmp.MustUpdate
            Me.ControlBox = Not tmp.MustUpdate

            Me.ShowDialog()
        End If

        Me.Dispose()

    End Sub

    Private Sub IgnoreButton_Click(sender As Object, e As EventArgs) Handles IgnoreButton.Click
        Me.Close()
    End Sub

    Private Sub JumpButton_Click(sender As Object, e As EventArgs) Handles JumpButton.Click
        Process.Start(JumpButton.Tag)

        Me.Close()

        Environment.Exit(0)

    End Sub

End Class