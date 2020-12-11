''' <summary>
''' 机器码
''' </summary>
Public Module MachineCode
#Region "获取CPU批号"
    ''' <summary>
    ''' 获取CPU批号
    ''' </summary>
    Public Function GetCPUID() As String
        Dim tmpStr As String = ""

        Try
            Dim myCpu As Management.ManagementClass = New Management.ManagementClass("win32_Processor")
            Dim myCpuConnection As Management.ManagementObjectCollection = myCpu.GetInstances()
            For Each myObject As Management.ManagementObject In myCpuConnection
                tmpStr = myObject.Properties("Processorid").Value.ToString()
            Next
        Catch ex As Exception
        End Try

        Return tmpStr
    End Function
#End Region

#Region "获取硬盘信息"
    ''' <summary>
    ''' 获取硬盘信息
    ''' </summary>
    Public Function GetDiskDriveID() As String
        Dim tmpStr As String = ""

        Try
            Dim myCpu As Management.ManagementClass = New Management.ManagementClass("Win32_DiskDrive")
            Dim myCpuConnection As Management.ManagementObjectCollection = myCpu.GetInstances()
            For Each myObject As Management.ManagementObject In myCpuConnection
                tmpStr = myObject.Properties("Model").Value.ToString()
            Next
        Catch ex As Exception
        End Try

        Return tmpStr
    End Function
#End Region

#Region "获取网卡MAC"
    ''' <summary>
    ''' 获取网卡MAC
    ''' </summary>
    Public Function GetNetworkMAC() As String
        Dim tmpStr As String = ""

        Try
            Dim myCpu As Management.ManagementClass = New Management.ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim myCpuConnection As Management.ManagementObjectCollection = myCpu.GetInstances()
            For Each myObject As Management.ManagementObject In myCpuConnection
                If myObject("IPEnabled") = True Then
                    tmpStr = myObject("MacAddress").ToString()
                End If
            Next
        Catch ex As Exception
        End Try

        Return tmpStr
    End Function
#End Region
End Module
