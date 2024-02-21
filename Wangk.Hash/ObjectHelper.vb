Public Class ObjectHelper

    Public Shared Function ToDic(obj As Object) As Dictionary(Of String, String)
        Dim tmpMap = New Dictionary(Of String, String)

        If obj Is Nothing Then Return tmpMap

        Dim t = obj.GetType
        Dim propertyList = t.GetProperties

        For Each item In propertyList

            Dim key = item.Name
            Dim value As Object = item.GetValue(obj)

            If value IsNot Nothing Then
                tmpMap.Add(key, value)
            End If

        Next

        Return tmpMap
    End Function

End Class
