Imports System.Text.RegularExpressions
Imports System.Globalization
Imports LevDan.Exif
Imports System.Threading
Imports System.Text
Imports System.Drawing.Imaging

Public Class Form1

    Private Sub Form1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop, Path.DragDrop
        Dim theFiles() As String = CType(e.Data.GetData("FileDrop", True), String())
        If (theFiles.Length > 0) Then
            Dim di As New IO.DirectoryInfo(theFiles(0))
            If di.Exists Then
                Path.Text = theFiles(0)
            End If
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Path.Text = My.Settings.LastPath
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        RefreshTimeSpanOnTextBox()
        Log("Software started. Ready to work")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim a As New FolderBrowserDialog
        a.SelectedPath = Path.Text
        If (a.ShowDialog = Windows.Forms.DialogResult.OK) Then
            Path.Text = a.SelectedPath
            My.Settings.LastPath = Path.Text
            My.Settings.Save()
        End If
    End Sub

    Public Enum LogType
        NEUTRAL
        ERR
        SUCCESS
    End Enum

    Public Delegate Sub LogDelegate(ByRef str As String, ByVal type As LogType, ByVal newLine As Boolean, ByVal percent As Integer)

    Public Sub Log(ByRef str As String, Optional ByVal type As LogType = LogType.NEUTRAL, Optional ByVal newLine As Boolean = True, Optional ByVal percent As Integer = -1)
        If percent >= 0 Then
            ProgressBar1.Value = Math.Min(ProgressBar1.Maximum, percent)
            Label2.Text = Math.Round(percent / ProgressBar1.Maximum * 100, 1) & "%"
        End If
        If (str IsNot Nothing) Then
            RichLog.SelectionStart = RichLog.Text.Length
            RichLog.SelectedText = str & IIf(newLine, Environment.NewLine, "")
            RichLog.SelectionLength = 0
            If type = LogType.ERR Or type = LogType.SUCCESS Then
                RichLog.SelectionStart = RichLog.Text.Length - str.Length - 1
                RichLog.SelectionLength = str.Length
                If type = LogType.SUCCESS Then
                    RichLog.SelectionColor = Color.Green
                Else
                    RichLog.SelectionColor = Color.Red
                End If
            End If

            Dim line As Integer = RichLog.Lines.Length - IIf(newLine, 2, 1)
            RichLog.Select(RichLog.GetFirstCharIndexFromLine(line), RichLog.Lines(line).Length)

            RichTextBox1.SelectAll()
            RichTextBox1.SelectedRtf = RichLog.SelectedRtf

            RichLog.SelectionLength = 0
            RichTextBox1.SelectionLength = 0
            RichLog.ScrollToCaret()
        End If
    End Sub

    Public Sub ThreadLog(ByRef str As String, Optional ByVal type As LogType = LogType.NEUTRAL, Optional ByVal newLine As Boolean = True, Optional ByVal percent As Integer = -1)
        If Me.InvokeRequired Then
            Me.Invoke(New LogDelegate(AddressOf Log), str, type, newLine, percent)
        Else
            Log(str, type, newLine, percent)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim t As New Thread(AddressOf Work)
        t.Start()
    End Sub

    Public Delegate Sub ChangeStatusDelegate(ByVal working As Boolean)
    Public Sub ChangeStatus(ByVal working As Boolean)
        GroupBox1.Enabled = Not working
        GroupBox2.Enabled = Not working
        GroupBox3.Enabled = Not working
        Button2.Enabled = Not working
    End Sub

    Public Sub ThreadChangeStatus(ByVal working As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(New ChangeStatusDelegate(AddressOf ChangeStatus), working)
        Else
            ChangeStatus(working)
        End If
    End Sub

    Public Function RenameFileFromDate(ByVal file As IO.FileInfo, ByVal dateTime As DateTime)
        If CheckBoxForce.Checked AndAlso ForceTime.Tag IsNot Nothing AndAlso TypeOf ForceTime.Tag Is TimeSpan Then
            If Me.Invoke(New Int(AddressOf Combo3Index)) = 0 Then
                dateTime = dateTime.Add(CType(ForceTime.Tag, TimeSpan))
            Else
                dateTime = dateTime.Subtract(CType(ForceTime.Tag, TimeSpan))
            End If
            If UpdateExifData.Checked Then
                Dim img As Image = Nothing
                Try
                    Dim enc As Encoding = Encoding.UTF8
                    img = New Bitmap(file.FullName)
                    Dim dateStr = dateTime.ToString("yyyy:MM:dd HH:mm:ss")

                    Dim propItem1 As PropertyItem = img.GetPropertyItem(36867)
                    If propItem1 IsNot Nothing Then
                        propItem1.Value = enc.GetBytes(dateStr)
                        img.SetPropertyItem(propItem1)
                    End If

                    Dim propItem2 As PropertyItem = img.GetPropertyItem(36868)
                    If propItem2 IsNot Nothing Then
                        propItem2.Value = enc.GetBytes(dateStr)
                        img.SetPropertyItem(propItem2)
                    End If

                    Dim str As String = file.Directory.FullName & "\" & dateTime.ToString(OutputPattern.Text, CultureInfo.InvariantCulture) & file.Extension
                    Dim cnt As Integer = 1
                    While My.Computer.FileSystem.FileExists(str)
                        str = file.Directory.FullName & "\" & dateTime.ToString(OutputPattern.Text, CultureInfo.InvariantCulture) & "_" & cnt & file.Extension
                        cnt += 1
                    End While
                    img.Save(str)
                    img.Dispose()
                    My.Computer.FileSystem.DeleteFile(file.FullName)
                    Return True
                Catch ex As Exception
                End Try
                If img IsNot Nothing Then
                    Try
                        img.Dispose()
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If
        Return RenameFileFromString(file, dateTime.ToString(OutputPattern.Text, CultureInfo.InvariantCulture))
    End Function

    Public Function RenameFileFromString(ByVal file As IO.FileInfo, ByVal newName As String)
        Try
            Dim c As Integer = -1
fileexsists: c += 1
            Dim name As String = newName & IIf(c > 0, "_" & c, "") & file.Extension.ToLower
            If My.Computer.FileSystem.FileExists(file.Directory.FullName & "\" & name) Then
                GoTo fileexsists
            Else
                My.Computer.FileSystem.RenameFile(file.FullName, name)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function RenameFromPattern(ByVal file As IO.FileInfo, ByVal writeErrorLog As Boolean) As Boolean
        Dim dt As DateTime = CheckPatternAndReturnDateTime(file.Name)
        If (dt <> Nothing) Then
            If RenameFileFromDate(file, dt) Then
                ThreadLog("OK!", LogType.SUCCESS)
                Return True
            Else
                If writeErrorLog Then ThreadLog("Unable to rename file!", LogType.ERR)
            End If
        End If
        If writeErrorLog Then ThreadLog("Unable to find pattern file!", LogType.ERR)
        Return False
    End Function

    Public Function GetRightExifTag(ByVal file As IO.FileInfo) As ExifTag
        Dim exif As New ExifTagCollection(file.FullName)
        For Each t As ExifTag In exif
            If t.FieldName.Equals("DateTimeOriginal") Then
                Return t
            End If
            If t.FieldName.Equals("DateTimeDigitized") Then
                Return t
            End If
        Next
        Return exif.Item(36867)
    End Function

    Public Function RenameFromExif(ByVal file As IO.FileInfo, ByVal writeErrorLog As Boolean) As Boolean
        Try
            Dim tag As ExifTag = GetRightExifTag(file)
            Dim d As String = tag.Value
            Dim dat As Date
            If (DateTime.TryParseExact(d, "yyyy:MM:dd HH:mm:ss", Nothing,
                  DateTimeStyles.None, dat)) Then
                If RenameFileFromDate(file, dat) Then
                    ThreadLog("OK!", LogType.SUCCESS)
                    Return True
                Else
                    If writeErrorLog Then ThreadLog("Unable to rename file!", LogType.ERR)
                End If
            End If
        Catch ex As Exception
        End Try
        If writeErrorLog Then ThreadLog("Unable to read exif data!", LogType.ERR)
        Return False
    End Function

    Public Function RenameCreationDate(ByVal file As IO.FileInfo, ByVal writeErrorLog As Boolean) As Boolean
        Dim combo2 As Integer = Me.Invoke(New Int(AddressOf Combo2Index))
        Dim d As Date = IIf(combo2 = 0, file.CreationTime, file.LastWriteTime)
        If RenameFileFromDate(file, d) Then
            ThreadLog("OK!", LogType.SUCCESS)
            Return True
        End If
        If writeErrorLog Then ThreadLog("Unable to rename file!", LogType.ERR)
        Return False
    End Function

    Public Delegate Sub MaxProgressBarDelegate(ByVal value As Integer)
    Public Sub SetProgressBarMaximum(ByVal value As Integer)
        ProgressBar1.Maximum = value
    End Sub

    Public Function Combo1Index() As Integer
        Return ComboBox1.SelectedIndex
    End Function

    Public Function Combo2Index() As Integer
        Return ComboBox2.SelectedIndex
    End Function

    Public Function Combo3Index() As Integer
        Return ComboBox3.SelectedIndex
    End Function

    Public Delegate Function Int() As Integer

    Public Sub Work()
        ThreadChangeStatus(True)
        ThreadLog(Environment.NewLine & "Counting Files...")
        Dim allFiles As IO.FileInfo()
        Try
            Dim di As New IO.DirectoryInfo(Path.Text)
            allFiles = di.GetFiles()
            If (allFiles Is Nothing) Then
                Throw New Exception
            End If
        Catch ex As Exception
            ThreadLog("Can't list files in directory! Be sure that the directory exsists and that you have the privilege to access it", LogType.ERR)
            GoTo finish
        End Try


        Me.Invoke(New MaxProgressBarDelegate(AddressOf SetProgressBarMaximum), allFiles.Length)
        Dim cnt As Integer = 1
        Dim errors As Integer = 0

        Dim combo1 As Integer = Me.Invoke(New Int(AddressOf Combo1Index))

        For Each file As IO.FileInfo In allFiles
            ThreadLog("Renaming file " & cnt & "/" & allFiles.Length & " """ & file.Name & """...", LogType.NEUTRAL, False, cnt - 1)

            Select Case file.Extension.ToLower
                Case ".jpg", ".jpeg", ".png", ".gif", ".bmp"
                    If CheckImages.Checked Then
exif:                   If combo1 = 0 Then
                            If Not RenameFromExif(file, False) Then
                                If Not RenameFromPattern(file, Not CheckCreationDate.Checked) Then
                                    If CheckCreationDate.Checked Then
                                        If Not RenameCreationDate(file, True) Then
                                            errors += 1
                                        End If
                                    Else
                                        errors += 1
                                    End If
                                End If
                            End If
                        Else
                            If Not RenameFromPattern(file, False) Then
                                If Not RenameFromExif(file, Not CheckCreationDate.Checked) Then
                                    If CheckCreationDate.Checked Then
                                        If Not RenameCreationDate(file, True) Then
                                            errors += 1
                                        End If
                                    Else
                                        errors += 1
                                    End If
                                End If
                            End If
                        End If
                    Else
                        ThreadLog("Skipped", LogType.NEUTRAL)
                    End If
                Case ".avi", ".mkv", ".m4v", ".mp4", "*.3gp", ".mov"
                    If CheckVideos.Checked Then
                        If Not RenameFromPattern(file, Not CheckCreationDate.Checked) Then
                            If CheckCreationDate.Checked Then
                                If Not RenameCreationDate(file, True) Then
                                    errors += 1
                                End If
                            Else
                                errors += 1
                            End If
                        End If
                    Else
                        ThreadLog("Skipped", LogType.NEUTRAL)
                    End If
                Case Else
                    If CheckOtherFiles.Checked Then
                        If Not RenameFromPattern(file, Not CheckCreationDate.Checked) Then
                            If CheckCreationDate.Checked Then
                                If Not RenameCreationDate(file, True) Then
                                    errors += 1
                                End If
                            Else
                                errors += 1
                            End If
                        End If
                    Else
                        ThreadLog("Skipped", LogType.NEUTRAL)
                        errors += 1
                    End If
            End Select

            cnt += 1
        Next

        ThreadLog("Operation ended with " & errors & " errors.", LogType.SUCCESS, True, allFiles.Length)

finish: ThreadChangeStatus(False)
    End Sub


    Public Function CheckPatternAndReturnDateTime(ByVal str As String) As DateTime
        Dim reg As String =
            DatePattern.Text.Replace("yy", "\d{2}").Replace("MM", "\d{2}").Replace("dd", "\d{2}").Replace("hh", "\d{2}").Replace("mm", "\d{2}").Replace("ss", "\d{2}").Replace("HH", "\d{2}")

        Dim parsedDate As Date
        str = Regex.Match(str, reg).Value
        If DateTime.TryParseExact(str, DatePattern.Text, Nothing, DateTimeStyles.None, parsedDate) Then
            Return parsedDate
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked, LinkLabel2.LinkClicked
        MessageBox.Show("yyyy or yy = year" & _
                         Environment.NewLine & "MM = month" & _
                         Environment.NewLine & "dd = day" & _
                         Environment.NewLine & "hh = 12h hours" & _
                         Environment.NewLine & "HH = 12h hours" & _
                         Environment.NewLine & "mm = minutes" & _
                         Environment.NewLine & "ss = seconds", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Form1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Path.DragEnter, Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub RefreshTimeSpanOnTextBox()
        If ForceTime.Tag Is Nothing Then
            ForceTime.Tag = New TimeSpan(0, 0, 0)
        End If
        If TypeOf ForceTime.Tag Is TimeSpan Then
            Dim ts As TimeSpan = CType(ForceTime.Tag, TimeSpan)
            ForceTime.Text = ts.Days & " days, " & ts.Hours & " hours, " & ts.Minutes & " minutes and " & ts.Seconds & " seconds"
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ts As TimeSpan = Nothing
        If CheckBoxForce.Checked AndAlso TypeOf ForceTime.Tag Is TimeSpan Then
            ts = CType(ForceTime.Tag, TimeSpan)
        End If
        Dim f As New TimeSpanSelector(ts)
        AddHandler f.OnEnd, AddressOf SetTimeSpan
        f.Show()
    End Sub

    Private Sub SetTimeSpan(ByVal ts As TimeSpan)
        ForceTime.Tag = ts
        RefreshTimeSpanOnTextBox()
    End Sub

    Private Sub UpdateExifData_CheckedChanged(sender As Object, e As EventArgs) Handles UpdateExifData.CheckedChanged

    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Enter

    End Sub
End Class
