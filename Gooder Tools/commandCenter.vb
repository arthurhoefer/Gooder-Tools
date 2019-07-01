Imports System.Threading
Imports Gooder_Tools.functions
Imports Gooder_Tools.security
Public Class commandCenter
    Public Shared Function command()
        Try
Start:
            Console.Write("Command: ")
            Dim c As String = Console.ReadLine().ToLower()
            If c.Contains("set_flag:") Then
                If c.Replace("set_flag: ", "") = "ignore_security_requirements" Then
                    If Not My.Settings.ignoreSecurity Then
                        My.Settings.ignoreSecurity = True
                        My.Settings.Save()
                        Console.WriteLine("Warning: Password will no longer be required when an authentication request is generated")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    Else
                        My.Settings.ignoreSecurity = False
                        My.Settings.Save()
                        Console.WriteLine("Warning: Password will be required when an authentication request is generated")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    End If

                ElseIf c.Replace("set_flag: ", "") = "enable_extension_full_access" Then
                    If Not My.Settings.AllowFullAccessExtension Then
                        My.Settings.AllowFullAccessExtension = True
                        My.Settings.Save()
                        Console.WriteLine("All extensions now have full access to Gooder Tools by default.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    Else
                        My.Settings.AllowFullAccessExtension = False
                        My.Settings.Save()
                        Console.WriteLine("All extensions now will have to ask to have full access.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    End If
                ElseIf c.Replace("set_flag: ", "") = "disable_logging" Then
                    If My.Settings.logging Then
                        My.Settings.logging = False
                        Console.WriteLine("Logging has been disabled, this will make it harder to debug problems.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    Else
                        My.Settings.logging = True
                        Console.WriteLine("Logging has been enabled, this will make it easier to identify problems with Gooder Tools.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    End If
                ElseIf c.Replace("set_flag: ", "") = "verbose_logging" Then
                    If Not My.Settings.verboseLogging Then
                        My.Settings.verboseLogging = True
                        Console.WriteLine("Verbose logging has been enabled.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    Else
                        My.Settings.verboseLogging = False
                        Console.WriteLine("Verbose logging has been disabled.")
                        Gooder_Tools.developerFeatures.logger("w", c.Replace("set_flag: ", "") & " flag has been set")
                    End If
                    My.Settings.Save()
                Else
                        Console.WriteLine("The flag to set is invalid.")
                End If

                GoTo Start
            End If
            Select Case c
                Case "encryptfile", "ef", "encrypt_file", "encrypt file"
                    encryptFile()
                Case "reset pass", "rp", "reset_pass", "resetpass"
                    resetPass()
                Case "pihole", "pi-hole", "pi hole", "ph"
                    If My.Settings.piholeServer = "" Then
                        Console.WriteLine("Enter an IP to check: ")
                        sendARequest(Console.ReadLine(), True)
                    Else
                        sendARequest(My.Settings.piholeServer)
                    End If
                'Case "spamdns"
                '    Console.WriteLine("Enter an IP to spam with dns requests: ")
                '    Dim o As String = Console.ReadLine()
                '    spamDNS(o)
                Case "clear"
                    Console.Clear()
                Case "install_extension", "install extension"
                    If Not My.Computer.FileSystem.DirectoryExists("C:/GT/extensions/") Then
                        My.Computer.FileSystem.CreateDirectory("C:/GT/extensions/")
                    End If
                    runExtension()
                Case "run extension"
                    runAllExtensions()
                Case "decompile extension", "decompile_extension"
                    Console.WriteLine("Enter the extension path:")
                    Dim path As String = Console.ReadLine
                    Console.WriteLine("Decompiling extension...")
                    My.Computer.FileSystem.WriteAllText(path, AESDecrypt(My.Computer.FileSystem.ReadAllText(path), generateKeys(), "ok"), False)
                    Console.WriteLine("Extension decompiled, you can now view its original contents.")
                Case "hash file"
                    Console.WriteLine("Enter the file path to hash:")
                    Console.WriteLine(EncryptSHA256Managed(My.Computer.FileSystem.ReadAllText(Console.ReadLine())))
                Case "check extension"
                    Console.WriteLine("Enter the extension path:")
                    Try
                        Dim read = Newtonsoft.Json.Linq.JObject.Parse(My.Computer.FileSystem.ReadAllText(Console.ReadLine()))
                        Console.WriteLine("The extension looks like it's been programmed correctly.")
                    Catch
                        Console.WriteLine("The extension didn't load properly, which either means it's been programmed for an older version of Gooder Tools or it's just not programmed correctly.")
                    End Try
                Case ""
                    Console.WriteLine("The command cannot be blank")
                Case Else
                    Console.WriteLine("The command """ & c & """ isn't recognized")
            End Select
            GoTo Start
        Catch
            Gooder_Tools.developerFeatures.logger("e", ErrorToString())
            Console.WriteLine("A fatal error has occured")
        End Try
    End Function

End Class
