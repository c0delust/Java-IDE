Public Class Form2

    Private Const AW_BLEND = &H80000  'Uses a fade effect. This flag can be used only if hwnd is a top-level window.
    Private Declare Function AnimateWindow Lib "user32" (ByVal hwnd As Int32, ByVal dwTime As Int32, ByVal dwFlags As Int32) As Boolean
    Dim winHide As Integer = &H10000
    Dim winBlend As Integer = &H80000

    Public Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form1.Enabled = False
        UseWaitCursor = False
        Me.Cursor = Cursors.Default
        Label1.Text = "Do you want to save changes to " + Form1.fileName + " ?"
    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click

        Select Case (Form1.Caller)
            Case "New"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                If Form1.DialogR = 1 Then
                    Reset()
                    Form1.Activate()
                End If
                Form1.Enabled = True
                Form1.Activate()
                Me.Close()

            Case "Open"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                Me.Close()
                If Form1.DialogR = 1 Then
                    Form1.Activate()
                    Form1.OpenFileDialog1.ShowDialog()
                    Me.Close()
                    Form1.Opener()
                    Form1.Enabled = True
                Else
                    Form1.Enabled = True
                    Form1.Activate()
                End If

            Case "Close"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                If Form1.DialogR = 1 Then
                    Form1.Activate()
                    Form1.Enabled = True
                    Reset()
                Else
                    Me.Close()
                    Form1.Activate()
                    Form1.Enabled = True
                End If

            Case "JavaT"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                If Form1.DialogR = 1 Then
                    Form1.Activate()
                    Form1.Enabled = True
                    Reset()
                    Form1.RichTextBox1.Text = My.Resources.Java
                Else
                    Me.Close()
                    Form1.Activate()
                    Form1.Enabled = True
                End If

            Case "AppletT"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                If Form1.DialogR = 1 Then
                    Form1.Activate()
                    Form1.Enabled = True
                    Reset()
                    Form1.RichTextBox1.Text = My.Resources.Applet
                Else
                    Me.Close()
                    Form1.Activate()
                    Form1.Enabled = True
                End If

            Case "Exit"
                Form1.SaveToolStripMenuItem_Click(Nothing, Nothing)
                AnimateWindow(Me.Handle.ToInt32, CInt(100), winHide Or winBlend)
                AnimateWindow(Form1.Handle.ToInt32, CInt(500), winHide Or winBlend)
                Application.Exit()
        End Select

    End Sub

    Private Sub nSave_Click(sender As Object, e As EventArgs) Handles nSave.Click

        Select Case (Form1.Caller)
            Case "New"
                Me.Close()
                Form1.Enabled = True
                Reset()
                Form1.Activate()

            Case "Open"
                Form1.OpenFileDialog1.ShowDialog()
                Form1.Opener()
                Form1.Activate()
                Form1.Enabled = True
                Me.Close()

            Case "Close"
                Me.Close()
                Form1.Activate()
                Form1.Enabled = True
                Reset()
                Form1.Activate()

            Case "JavaT"
                Me.Close()
                Form1.Activate()
                Form1.Enabled = True
                Reset()
                Form1.RichTextBox1.Text = My.Resources.Java
                Form1.Activate()

            Case "AppletT"
                Me.Close()
                Form1.Activate()
                Form1.Enabled = True
                Reset()
                Form1.RichTextBox1.Text = My.Resources.Applet
                Form1.Activate()

            Case "Exit"
                AnimateWindow(Me.Handle.ToInt32, CInt(100), winHide Or winBlend)
                AnimateWindow(Form1.Handle.ToInt32, CInt(500), winHide Or winBlend)
                Application.Exit()
        End Select

    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Form1.Enabled = True
        Form1.cFlag = False
        Me.Close()
    End Sub

    Public Sub Reset()
        Form1.fileName = "Untitled"
        Form1.RichTextBox1.Text = ""
        Form1.filePath = Nothing
        Form1.Text = Form1.fileName + " - JavaIDE"
        Me.Close()
    End Sub

End Class