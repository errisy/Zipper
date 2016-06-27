Imports System.Text.RegularExpressions

Class MainWindow
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        'start to zip the file;
        If Application.ScriptFile IsNot Nothing AndAlso IO.File.Exists(Application.ScriptFile) Then
            Dim sf = Application.ScriptFile
            Dim json As String = IO.File.ReadAllText(sf)
            Dim zs = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Zipper)(json)
            Dim filename = Regex.Replace(sf, "\.zs$", ".zip")
            Dim fi As New IO.FileInfo(filename)
            Dim di = fi.Directory
            Dim igDirs = zs.ignoredir.Select(Of String)(Function(entry) (entry.Replace("/", "\")).ToLower).ToArray()
            Dim igFiles = zs.ignorefile.Select(Of String)(Function(entry) (entry.Replace("/", "\")).ToLower).ToArray()
            Dim inDirs = zs.includedir.Select(Function(entry) entry.Replace("/", "\").ToLower()).ToArray()
            Dim inFiles = zs.includefile.Select(Function(entry) entry.Replace("/", "\").ToLower()).ToArray()
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine("Excluded Directories:")
            For Each __dir In zs.ignoredir
                sb.AppendLine(__dir)
            Next
            sb.AppendLine("Excluded Files:")

            tbMain.Text = String.Join(vbNewLine, igDirs) + vbNewLine + String.Join(vbNewLine, igFiles)


            Dim addresslength As Integer = di.FullName.Length

            'IO.File.WriteAllBytes(filename, New Byte() {Asc("P"), Asc("K"), 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
            If IO.File.Exists(filename) Then IO.File.Delete(filename)
            Dim dirEntries As New HashSet(Of String)(New IgnoreCaseComparer)
            Dim fileEntries As New HashSet(Of String)(New IgnoreCaseComparer)
            Using fs As New IO.FileStream(filename, IO.FileMode.OpenOrCreate)
                Using zip As New System.IO.Compression.ZipArchive(fs, IO.Compression.ZipArchiveMode.Create)
                    Dim LoadDir As Action(Of IO.DirectoryInfo) =
                    Sub(dir As IO.DirectoryInfo)
                        'add each file
                        For Each _file In dir.GetFiles()
                            Dim _filename = _file.FullName.ToLower
                            Dim _truncated = _filename.Substring(addresslength + 1)
                            If _filename = filename.ToLower Then Continue For
                            If _filename = sf.ToLower Then Continue For
                            If igFiles.Contains(_truncated) Then Continue For
                            Dim entry = zip.CreateEntry(_file.FullName.Substring(addresslength + 1))
                            Using zEntry = entry.Open
                                Dim buffer = IO.File.ReadAllBytes(_file.FullName)
                                zEntry.Write(buffer, 0, buffer.Length)
                            End Using
                            fileEntries.Add(_file.FullName)
                        Next
                        For Each _dir In dir.GetDirectories()
                            Dim _truncated = _dir.FullName.Substring(addresslength + 1)
                            If igDirs.Contains(_truncated) Then Continue For
                            LoadDir(_dir)
                            dirEntries.Add(_dir.FullName)
                        Next
                    End Sub
                    LoadDir(di)
                    For Each dirname In inDirs
                        Dim _dinfo As New IO.DirectoryInfo(di.FullName + "\" + dirname)
                        If Not _dinfo.Exists Then Continue For
                        If Not dirEntries.Contains(_dinfo.FullName) Then
                            LoadDir(_dinfo)
                            dirEntries.Add(_dinfo.FullName)
                        End If
                    Next
                    For Each filename In inFiles
                        Dim _finfo As New IO.FileInfo(di.FullName + "\" + filename)
                        If Not _finfo.Exists Then Continue For
                        Dim _filename = _finfo.FullName.ToLower
                        Dim _truncated = _filename.Substring(addresslength + 1)
                        If _filename = filename.ToLower Then Continue For
                        If _filename = sf.ToLower Then Continue For
                        If igFiles.Contains(_truncated) Then Continue For
                        If Not fileEntries.Contains(_finfo.FullName) Then
                            Dim entry = zip.CreateEntry(_finfo.FullName.Substring(addresslength + 1))
                            Using zEntry = entry.Open
                                Dim buffer = IO.File.ReadAllBytes(_finfo.FullName)
                                zEntry.Write(buffer, 0, buffer.Length)
                            End Using
                            fileEntries.Add(_finfo.FullName)
                        End If
                    Next
                End Using
            End Using
        End If
    End Sub
End Class
