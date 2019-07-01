Public Class Form1
    Dim fileOpened As Boolean = False
    Dim unsavedChanges As Boolean = False
    Private Sub OpenProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenProjectToolStripMenuItem.Click
        OpenFile.ShowDialog()
        If My.Computer.FileSystem.FileExists(OpenFile.FileName.ToString()) Then
            code.Text = My.Computer.FileSystem.ReadAllText(OpenFile.FileName.ToString)
            Me.Text = OpenFile.FileName.ToString()
            fileOpened = True
        Else
            MsgBox("The file does not exist.")
        End If
    End Sub

    Public Shared Function checkExtension()
        Try
            Dim read = Newtonsoft.Json.Linq.JObject.Parse(Form1.code.Text)
            Form1.extensionCheck.Text = "Program: Compatibile"
        Catch
            Form1.extensionCheck.Text = "Program: Incompatibile"
        End Try
    End Function

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        If fileOpened Then
            My.Computer.FileSystem.WriteAllText(OpenFile.FileName.ToString, code.Text, False)
            Process.Start(OpenFile.FileName.ToString)
        Else
            MsgBox("You must open a project first.")
        End If
    End Sub

    Private Sub code_TextChanged(sender As Object, e As EventArgs) Handles code.TextChanged
        If fileOpened Then
            unsavedChanges = True
            Me.Text = OpenFile.FileName.ToString() & "* (Unsaved changes)"
        End If
        checkExtension()
    End Sub

    Private Sub SaveProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.Click
        My.Computer.FileSystem.WriteAllText(OpenFile.FileName.ToString, code.Text, False)
        Me.Text = OpenFile.FileName.ToString()
        unsavedChanges = False

    End Sub

    Private Sub CloseProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseProjectToolStripMenuItem.Click
        If unsavedChanges Then
            Dim df = MsgBox("Changes to this file have not been saved, would you like to save them?", vbYesNo)
            If df = vbYes Then
                My.Computer.FileSystem.WriteAllText(OpenFile.FileName.ToString, code.Text, False)
                Me.Text = OpenFile.FileName.ToString()

            End If
        End If
        code.Text = vbNullString
        fileOpened = False
        unsavedChanges = False
        Me.Text = "Gooder Studio"
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
