Imports System.Runtime.CompilerServices

Public Class CodeHelper

#Region "获取代码位置"
    ''' <summary>
    ''' 获取代码位置
    ''' </summary>
    Public Shared Function GetLocation(<CallerFilePath> Optional callerFilePath As String = Nothing,
                                       <CallerLineNumber> Optional callerLineNumber As String = Nothing) As String

        Return $"{IO.Path.GetFileNameWithoutExtension(callerFilePath)}-{callerLineNumber}"

    End Function
#End Region

End Class
