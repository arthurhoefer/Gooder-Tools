Public Class developerFeatures

    Public Shared Function logger(ByRef type, ByRef text)
        If Not My.Settings.logging Then
            Console.WriteLine(type & ", " & text)
            Return Nothing
        End If
        Dim t As String
        If type = "e" Then
            t = "Error"
        ElseIf type = "w" Then
            t = "Warning"
        ElseIf type = "d" Then
            t = "Debug"
        Else
            t = "Unknown"
        End If

        If Not My.Computer.FileSystem.DirectoryExists("C:/GT/logs/") Then
            My.Computer.FileSystem.CreateDirectory("C:/GT/logs/")
        End If

        My.Computer.FileSystem.WriteAllText("C:/GT/logs/logs_" & DateAndTime.Today.ToShortDateString.Replace("/", "-").ToString, vbCrLf & "(" & DateAndTime.Now.ToString & ")(" & t & ") " & text, True)

    End Function

End Class
