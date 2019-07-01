Imports Gooder_Tools.security
Imports Gooder_Tools.developerFeatures
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
'Imports OpenDNS

Public Class functions
    Public Shared Function encryptFile()
        Dim filepath As String
        Dim passkey As String
        Console.WriteLine("Enter the files path you'd like to encrypt: ")
        filepath = Console.ReadLine()
        Console.WriteLine("Enter the encryption key you'd like to use: ")
        passkey = Console.ReadLine()
    End Function

    Public Shared Function resetPass()
        If authenticate() Then
            logger("d", "User has authenticated and requested to reset password")
singsowell:
            Dim p As String
            Console.WriteLine("Please enter a password to use: ")
            p = Console.ReadLine()
            Dim pc As String
            Console.WriteLine("Please re-enter this password: ")
            pc = Console.ReadLine()
            If pc = p Then
                My.Computer.FileSystem.CreateDirectory("C:/GT/")
                My.Computer.FileSystem.WriteAllText("C:/GT/pass.auth", EncryptSHA256Managed(p), False)
                p = EncryptSHA256Managed(p)
                My.Settings.PasswordHash = EncryptSHA256Managed(EncryptSHA256Managed(p))
                My.Settings.Save()
                pc = vbNull
                p = vbNull
                logger("d", "Password has been changed")
            Else
                GoTo singsowell
            End If
        End If
    End Function


    Public Shared Function sendARequest(ByRef ip As String, Optional ByRef newS As Boolean = False)
        Try
            If newS Then
                Console.WriteLine("Would you like to save this server as your default pihole server? You'll be able to change it by running th command """ & "pihole-change" & """")
                Dim c
                c = Console.ReadLine().ToLower()
                If c = "yes" Or c = "y" Then
                    My.Settings.piholeServer = ip
                    My.Settings.Save()
                End If
            End If
            Dim request As WebRequest = WebRequest.Create("http://" & ip & "/admin/api.php?summary=")
            request.Method = "POST"
            Dim postData As String = "pw=BuL9SwJb"
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            Dim read = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer)
            Console.WriteLine("Here's details about the DNS server " & ip)
            Console.WriteLine("Domains being blocked: " & read.Item("domains_being_blocked").ToString())
            Console.WriteLine("DNS Queries today so far: " & read.Item("dns_queries_today").ToString())
            Console.WriteLine("Ads blocked today so far: " & read.Item("ads_blocked_today").ToString())
        Catch
            logger("e", ErrorToString)
        End Try
    End Function

    'Public Shared Function spamDNS(ByRef ip)
    '    Console.WriteLine("This feature is still under development")
    '    Return Nothing
    '    Console.WriteLine("Spamming the dns server: " & ip)
    '    Dim query = New DnsQuery()
    '    query.Servers.Add(ip)

    '    query.Domain = "arthurhoefer.com"
    '    query.QueryType = Types.TXT
    '    Dim c As Double
    '    Do Until c > 500
    '        query.Send()
    '        Threading.Thread.Sleep(100)
    '        c += 1D
    '        Dim progress = CDec((c / 500) * 100)

    '        Console.SetCursorPosition(0I, Console.CursorTop)
    '        Console.Write(progress.ToString("00.00") & " %")
    '    Loop
    'End Function

    Public Shared Function runExtension()
        Try
            Console.WriteLine("Enter the location of the extension:")
            Dim f As String = Console.ReadLine()
            If My.Computer.FileSystem.FileExists(f) Then
                logger("d", "Installing extension")
                Console.WriteLine("Installing extension...")
                My.Computer.FileSystem.MoveFile(f, "C:/GT/extensions/" & Path.GetFileName(f))
                My.Computer.FileSystem.WriteAllText("C:/GT/extensions/" & Path.GetFileName(f), AESEncrypt(My.Computer.FileSystem.ReadAllText("C:/GT/extensions/" & Path.GetFileName(f)), generateKeys(), "ok"), False)
                logger("d", "Extension installed")
                Console.WriteLine("Extension installed.")
            End If
        Catch
            logger("e", ErrorToString())
        End Try
    End Function

    Public Shared Function checkExtensionStep(ByRef f As String, Optional ByRef systemExtension As Boolean = False)
        Static stepResetCount As Double = 0
        Try
            Dim co As String
            If My.Settings.verboseLogging Then
                logger("d", "Step value: " & f.ToString)
            End If

            If f.ToString = "launch_command_center" Then
                If isAuthenticated Or systemExtension Then
                    logger("w", "Launching command center")
                    Command()
                Else
                    logger("e", "Failed to launch command center, user was never authenticated")
                    Console.WriteLine("Failed to launch command center, you haven't entered your password yet and the extension that attempted to launch it wasn't granted full access to Gooder Tools")
                End If
            ElseIf f.ToString.Contains("display_message: ") Then
                co = f.ToString.Replace("display_message: ", "")
                If co.Contains("$permission_level") Then
                    If systemExtension Then
                        co = co.Replace("$permission_level", "SYSTEM")
                    Else
                        co = co.Replace("$permission_level", "USER")
                    End If

                End If
                If systemExtension Then
                    co = cleanSentence(co, True)
                Else
                    co = cleanSentence(co)
                End If
                Console.WriteLine(co)
            ElseIf f.ToString.Contains("change_text_color: ") Then
                Select Case f.ToString.Replace("change_text_color: ", "")
                    Case "blue"
                        Console.ForegroundColor = ConsoleColor.Blue
                    Case "red"
                        Console.ForegroundColor = ConsoleColor.Red
                    Case "yellow"
                        Console.ForegroundColor = ConsoleColor.Yellow
                    Case "green"
                        Console.ForegroundColor = ConsoleColor.Green
                    Case "white"
                        Console.ForegroundColor = ConsoleColor.White
                End Select
                'ElseIf f.ToString.Contains("reset_user_password") Then
                '    If My.Settings.verboseLogging Then
                '        logger("w", "Extension has requested user to change password")
                '    End If

                '    Console.WriteLine("Extension has sent a reset password command, please enter your password if you want to reset it, or else stop Gooder Tools and uninstall the extension")
                '    resetPass()
            ElseIf f.ToString.Contains("wait_for_input") Then
                Console.ReadLine()

            ElseIf f.ToString.Contains("change_step: ") Then
                'If stepResetCount > 10 Then
                '    If Not systemExtension Then
                '        'If My.Settings.verboseLogging Then
                '        '    logger("e", "This progt has used the step changing command too much, access has been restricted for this feature.")
                '        'End If

                '        'Return Nothing
                '    End If

                If My.Settings.verboseLogging Then
                    logger("d", "The step position has been modified by the program.")
                End If

                changeStep(f.ToString.Replace("change_step: ", ""))
                'stepResetCount += 1
            ElseIf f.ToString.Contains("configure_test_environment") Then
                If systemExtension Then
                    systemExtension = False
                Else
                    systemExtension = True
                End If
            ElseIf f.ToString.Contains("restart_pc") Then
                If systemExtension Then
                    System.Diagnostics.Process.Start("shutdown", "-r -t 00")
                Else
                    Console.WriteLine(extensionName & " is attempting to restart the PC, do you want to allow this? Y/N")
                    Dim s As String = Console.ReadLine().ToLower
                    If s = "y" Or s = "yes" Then
                        System.Diagnostics.Process.Start("shutdown", "-r -t 00")
                    Else
                        Console.WriteLine(extensionName & " was blocked from restarting the PC.")
                    End If
                End If
                    Else
                    Console.WriteLine("Unknown step value: " & f.ToString)
            End If
        Catch
            logger("e", ErrorToString)
        End Try
    End Function

    Public Shared Function cleanSentence(ByRef s As String, Optional ByRef a As Boolean = False)
        ' If systemExtension Then
        's = s.Replace("$permission_level", "SYSTEM LEVEL")
        'Else
        's = s.Replace("$permission_level", "USER LEVEL")
        'End If
        If Not a Then
            developerFeatures.logger("e", Module1.extensionName & " attempted to perform an action outside of it's jurisdiction, the attempt was blocked.")
            s = s.Replace("$program_name", Module1.extensionName)
            s = s.Replace("$verbose_logging", "authorative_error")
            s = s.Replace("$logging", "authorative_error")
            s = s.Replace("$security", "authorative_error")
            Return s
        Else
            s = s.Replace("$program_name", Module1.extensionName)
            s = s.Replace("$verbose_logging", My.Settings.verboseLogging.ToString)
            s = s.Replace("$logging", My.Settings.logging.ToString)
            s = s.Replace("$security", My.Settings.ignoreSecurity.ToString)
        End If
        Return s.ToString
    End Function

    Public Shared Function SystemExtensionCheck(ByRef hash As String)
        If hash = "4SNMaNFpDEwTyGq+tYzHFedgKvicF2c8AZGVTOEoHQ8=" Then
            Return True
        End If
    End Function
End Class
