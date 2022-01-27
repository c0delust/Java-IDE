Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Public Class Form1

    Private Const AW_BLEND = &H80000  'Uses a fade effect. This flag can be used only if hwnd is a top-level window.
    Private Declare Function AnimateWindow Lib "user32" (ByVal hwnd As Int32, ByVal dwTime As Int32, ByVal dwFlags As Int32) As Boolean
    Dim winHide As Integer = &H10000
    Dim winBlend As Integer = &H80000

    Public filePath As String
    Public fileName As String
    Public fileExt As String
    Public Files() As String

    Public cFlag As Boolean = True
    Private fileDrop As Boolean = False

    Public Caller As String = Nothing

    Public DialogR As DialogResult

    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fileName = "Untitled"
        Me.Text = "Untitled - Java IDE"
        Run.Enabled = False
        RichTextBox1.AcceptsTab = True
        FontDialog1.Font() = RichTextBox1.Font
        RichTextBox1.AllowDrop = True
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = 116 Then
            If Run.Enabled Then
                Run_Click(Nothing, Nothing)
            End If
        End If
    End Sub
    Private Sub RichTextBox1_KeyDown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles RichTextBox1.KeyDown

        If e.KeyCode = 116 Then
            If Run.Enabled Then
                Run_Click(Nothing, Nothing)
            End If
        End If

    End Sub

    Private Sub RichTextBox1_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles RichTextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Public Sub RichTextBox1_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles RichTextBox1.DragDrop
        Files = e.Data.GetData(DataFormats.FileDrop)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            If Path.GetExtension(Files(0)).Contains(".java") Then
                If Not Me.Text.Contains("*") Then
                    fileDrop = True

                    filePath = Files(0)
                    fileName = Path.GetFileNameWithoutExtension(Files(0))
                    fileExt = Path.GetExtension(Files(0))

                    Opener()
                Else
                    fileDrop = True
                    Caller = "DragDrop"
                    Form2.Show()

                End If
            Else
                MsgBox("Invalid File Type", 0, "Java IDE")
            End If

        End If
    End Sub

    Public Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click

        Caller = "New"

        If Me.Text.Contains("*") Then
            Form2.Show()
        Else
            fileName = "Untitled"
            RichTextBox1.Text = ""
            filePath = Nothing
            Me.Text = fileName + " - Java IDE"
        End If

    End Sub

    Public Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        Caller = "Open"
        OpenFileDialog1.Filter = "Java Source Code|*.java|All Files|*.*"
        OpenFileDialog1.FileName = ""

        If Not Me.Text.Contains("*") Then
            On Error Resume Next
            DialogR = OpenFileDialog1.ShowDialog()
            Using openfile As New StreamReader(filePath)
                RichTextBox1.Text = openfile.ReadToEnd()
                Me.Text = fileName + " - Java IDE"
            End Using
        Else
            Form2.Show()
        End If

    End Sub

    Public Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        On Error Resume Next

        If Me.Text = "*Untitled - Java IDE" Or filePath = "notsaved" Then
            SaveAsToolStripMenuItem_Click(Nothing, Nothing)
        Else
            Using savefile As New StreamWriter(filePath)
                savefile.WriteLine(RichTextBox1.Text)
                Me.Text = fileName + " - Java IDE"
            End Using
        End If

        If Not RichTextBox1.Text = "" Then
            Run.Enabled = True
        End If
    End Sub

    Public Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click

        On Error Resume Next

        SaveFileDialog1.Filter = "Java Source Code|*.java|All Files|*.*"
        SaveFileDialog1.FileName = ""
        DialogR = SaveFileDialog1.ShowDialog()

        Using savefile As New StreamWriter(filePath)
            savefile.WriteLine(RichTextBox1.Text)
            fileName = Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName())
        End Using

        If DialogR = 1 Then
            Me.Text = fileName + " - Java IDE"
        End If

        If Not RichTextBox1.Text = "" Then
            Run.Enabled = True
        End If

    End Sub

    Public Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

        Caller = "Close"

        On Error Resume Next
        If Me.Text.Contains("*") Then
            Form2.Show()
        Else
            Form2.Reset()
        End If

    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        filePath = SaveFileDialog1.FileName()
        fileName = Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName())
        fileExt = Path.GetExtension(SaveFileDialog1.FileName())
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        filePath = OpenFileDialog1.FileName()
        fileName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName())
        fileExt = Path.GetExtension(OpenFileDialog1.FileName())
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged

        Dim tLine, tLength As Integer

        If fileName = "Untitled" Then
            If Not RichTextBox1.Text = Nothing Then
                Me.Text = "*" + fileName + " - Java IDE"
            Else
                Me.Text = fileName + " - Java IDE"
            End If
        Else
            Me.Text = "*" + fileName + " - Java IDE"
        End If


        If RichTextBox1.Text = Nothing Or filePath = Nothing Then
            Run.Enabled = False
        Else
            Run.Enabled = True
        End If

        tLine = RichTextBox1.Lines.Count
        lines.Text = "Lines: " + tLine.ToString(tLine)

        tLength = RichTextBox1.Text.Length
        length.Text = "Length: " + tLength.ToString(tLength)


        If (RichTextBox1.Text.IndexOf("<applet code=", 0, StringComparison.CurrentCultureIgnoreCase) > -1 Or RichTextBox1.Text.IndexOf("<applet code =", 0, StringComparison.CurrentCultureIgnoreCase) > -1) And RichTextBox1.Text.IndexOf("</applet>", 0, StringComparison.CurrentCultureIgnoreCase) > -1 Then
            rApplet.Checked = True
        Else
            rJava.Checked = True
        End If

        If fileDrop Then
            Me.Text = fileName + " - Java IDE"
            fileDrop = False
        End If

    End Sub

    Private Sub RichTextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox1.KeyPress

        If AutoCompleteToolStripMenuItem.Checked = True Then

            Dim key As Char = e.KeyChar
            Dim index As Integer = RichTextBox1.SelectionStart
            Dim auto As String

            If key = "(" Then
                e.Handled = True
                auto = "()"
                RichTextBox1.Text = RichTextBox1.Text.Insert(index, auto)
                RichTextBox1.SelectionStart = index + auto.Length - 1
            ElseIf key = "[" Then
                e.Handled = True
                auto = "[]"
                RichTextBox1.Text = RichTextBox1.Text.Insert(index, auto)
                RichTextBox1.SelectionStart = index + auto.Length - 1
            ElseIf key = "{" Then
                e.Handled = True
                auto = "{}"
                RichTextBox1.Text = RichTextBox1.Text.Insert(index, auto)
                RichTextBox1.SelectionStart = index + auto.Length - 1
            ElseIf key = """" Then
                e.Handled = True
                auto = """"""
                RichTextBox1.Text = RichTextBox1.Text.Insert(index, auto)
                RichTextBox1.SelectionStart = index + auto.Length - 1
            Else
                e.Handled = False
            End If
        End If

    End Sub

    Private Sub Run_Click(sender As Object, e As EventArgs) Handles Run.Click

        SaveToolStripMenuItem_Click(Nothing, Nothing)

        If fileName = Nothing And filePath = Nothing Then
            MessageBox.Show("Nothing")
        Else
            If Not RichTextBox1.Text = "" Then

                Dim viewer As String = Nothing
                Dim fileNameExt As String = fileName + ".java"
                Dim fileNameWithoutExt As String = """" + filePath.Replace(fileNameExt, "") + """"
                Dim safeFileName As String = fileName
                Dim classPath As String = """" + filePath.Replace(fileNameExt, "") + fileName + ".class"""

                If rJava.Checked = True Then
                    viewer = "java " + safeFileName
                ElseIf rApplet.Checked = True Then
                    viewer = "appletviewer " + fileNameExt
                End If

                Dim command As String = "cmd.exe /k cd /d " + """" + fileNameWithoutExt + """" + " && javac " + fileNameExt + " && " + viewer

                If File.Exists(classPath.Replace("""", "")) Then
                    Shell("cmd.exe /k del " + classPath, 0)
                    Shell(command, 1)
                Else
                    Shell(command, 1)
                End If
            End If
        End If

    End Sub


    Public Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Caller = "Exit"

        On Error Resume Next

        If Me.Text.Contains("*") Then
            Form2.Show()
        Else
            AnimateWindow(Me.Handle.ToInt32, CInt(500), winHide Or winBlend)
            Application.Exit()
        End If

    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FontToolStripMenuItem.Click
        FontDialog1.ShowDialog()
        RichTextBox1.Font = FontDialog1.Font()
        RichTextBox1.ForeColor = FontDialog1.Color()
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        RichTextBox1.Cut()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        RichTextBox1.Copy()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        RichTextBox1.Paste()
    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        RichTextBox1.Undo()
        RichTextBox1.ClearUndo()
    End Sub


    Private Sub AboutJavaIDEToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutJavaIDEToolStripMenuItem1.Click
        MessageBox.Show("Java IDE - The Most Light Weight Java IDE " + vbCrLf + "Developed by: Mohammadsaad Mulla (codelust)", "About Java IDE")
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs)
        RichTextBox1.SelectAll()
    End Sub

    Private Sub SelectAllToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem1.Click
        RichTextBox1.SelectAll()
    End Sub

    Private Sub JavaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JavaToolStripMenuItem.Click

        Caller = "JavaT"

        On Error Resume Next
        If Me.Text.Contains("*") Then
            Form2.Show()
        Else
            Form2.Reset()
            RichTextBox1.Text = My.Resources.Java
        End If

    End Sub

    Private Sub AppletToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AppletToolStripMenuItem.Click

        Caller = "AppletT"

        On Error Resume Next
        If Me.Text.Contains("*") Then
            Form2.Show()
        Else
            Form2.Reset()
            RichTextBox1.Text = My.Resources.Applet
        End If

    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If e.CloseReason = CloseReason.UserClosing Then

            Caller = "Exit"

            On Error Resume Next

            If Me.Text.Contains("*") Then
                Form2.Show()
            Else
                AnimateWindow(Me.Handle.ToInt32, CInt(500), winHide Or winBlend)
                Application.Exit()
            End If

            e.Cancel = True
        End If
        MyBase.OnFormClosing(e)
    End Sub


    Private Sub WordWrapToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles WordWrapToolStripMenuItem.CheckedChanged
        If WordWrapToolStripMenuItem.Checked Then
            RichTextBox1.WordWrap = True
        Else
            RichTextBox1.WordWrap = False
        End If

    End Sub

    Public Sub Opener()
        OpenFileDialog1.Filter = "Java Source Code|*.java|All Files|*.*"
        OpenFileDialog1.FileName = ""

        On Error Resume Next
        Using openfile As New StreamReader(filePath)
            RichTextBox1.Text = openfile.ReadToEnd()
            Me.Text = fileName + " - Java IDE"
        End Using

    End Sub

End Class
