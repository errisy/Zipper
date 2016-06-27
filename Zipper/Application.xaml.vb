Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        If e.Args.Count > 0 Then
            ScriptFile = e.Args(0)
        Else
            'ScriptFile = "C:\HTTP\ng2ts\zip.zs"
        End If

    End Sub
    Public Shared ScriptFile As String
    ' 应用程序级事件(例如 Startup、Exit 和 DispatcherUnhandledException)
    ' 可以在此文件中进行处理。

End Class
