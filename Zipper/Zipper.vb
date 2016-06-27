Public Class Zipper
    Property ignoredir As String() = New String() {}
    Property ignorefile As String() = New String() {}
    Property includedir As String() = New String() {}
    Property includefile As String() = New String() {}

End Class

Public Class IgnoreCaseComparer
    Implements System.Collections.Generic.IEqualityComparer(Of String)

    Private Function StringEquals(x As String, y As String) As Boolean Implements IEqualityComparer(Of String).Equals
        Return String.Compare(x, y, True)
    End Function

    Private Function StringGetHashCode(obj As String) As Integer Implements IEqualityComparer(Of String).GetHashCode
        Return obj.ToLower.GetHashCode
    End Function

End Class