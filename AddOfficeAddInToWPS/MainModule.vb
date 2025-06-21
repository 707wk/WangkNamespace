Imports System.Reflection
Imports Microsoft.Win32
Imports System.Linq

Module MainModule

    Sub Main()

        Try

#Region "检测安装环境"
            Console.WriteLine("检测安装环境")
            Do
                Threading.Thread.Sleep(1000)

                Dim tmpProcesses = Process.GetProcessesByName("WPS")
                If tmpProcesses IsNot Nothing AndAlso
                    tmpProcesses.Count > 0 Then

                    Console.SetCursorPosition(0, Console.CursorTop)
                    Console.Write("需要退出所有 WPS程序")
                    Continue Do
                End If

                tmpProcesses = Process.GetProcessesByName("Excel")
                If tmpProcesses IsNot Nothing AndAlso
                    tmpProcesses.Count > 0 Then

                    Console.SetCursorPosition(0, Console.CursorTop)
                    Console.Write("需要退出所有 Excel程序")
                    Continue Do
                End If

                Console.WriteLine()
                Exit Do
            Loop
#End Region

#Region "安装插件"
            Dim startupDir = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

            Console.WriteLine("安装插件")

            Dim files = IO.Directory.GetFiles(startupDir, "*.vsto")
            For Each item In files
                Dim tmpProcess = Process.Start($"C:\Program Files\Common Files\microsoft shared\VSTO\10.0\VSTOInstaller.exe", $"/i ""{item}""")
                tmpProcess.WaitForExit()

                Console.WriteLine(tmpProcess.ExitCode)
                If tmpProcess.ExitCode <> 0 Then
                    Exit Sub
                End If
            Next



#End Region

#Region "更新注册表"
            Console.WriteLine("更新注册表")
            Dim tmpRegistryKey = Registry.CurrentUser

            ' HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\Word\Addins
            Dim tmpDictionary As New Dictionary(Of String, String) From {
                {"Software\Microsoft\Office\Excel\Addins", "Software\Kingsoft\Office\ET\AddinsWL"},
                {"Software\Microsoft\Office\PowerPoint\Addins", "Software\Kingsoft\Office\WPP\AddinsWL"},
                {"Software\Microsoft\Office\Word\Addins", "Software\Kingsoft\Office\WPS\AddinsWL"}
            }

            For Each item In tmpDictionary

                Dim addins = tmpRegistryKey.OpenSubKey(item.Key, True)
                Dim wps = tmpRegistryKey.CreateSubKey(item.Value)

                For Each subKeyName In addins.GetSubKeyNames()
                    wps.SetValue(subKeyName, subKeyName, RegistryValueKind.String)
                Next

            Next

            Console.WriteLine("更新成功")
#End Region

            'Console.ReadLine()

        Catch ex As Exception
            Console.WriteLine("更新异常")
            Console.WriteLine(ex.ToString)
            Threading.Thread.Sleep(2000)
        End Try

    End Sub

End Module
