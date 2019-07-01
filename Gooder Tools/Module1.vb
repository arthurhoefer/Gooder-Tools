Imports Gooder_Tools.security
Imports Gooder_Tools.developerFeatures
Imports Gooder_Tools.commandCenter
Imports System.IO
Imports Gooder_Tools.functions
Imports System.Deployment

Module Module1
    Public c As Double = 0
    Public isAuthenticated As Boolean = False
    Public extensionName As String = ""
    Sub Main()
        Console.WriteLine("Gooder Tools v0.1 alpha")
        Try
            If Not My.Application.CommandLineArgs(0) = "" Then
                Try
                    changeStep(1)
                    Dim read = Newtonsoft.Json.Linq.JObject.Parse(My.Computer.FileSystem.ReadAllText(My.Application.CommandLineArgs(0)))
                    logger("d", "Program " & read.Item("name").ToString & " successfully loaded")
                    extensionName = read.Item("name")
                    '   Console.WriteLine("Extension author: " & read.Item("creator").ToString)
                    '     Console.WriteLine("Are you sure you want to run this extension? Running extensions without reviewing their code first can be dangerous")
                    '    If Console.ReadLine = "y" Then
                    '   logger("d", "Extension " & f.ToString & " was given permission to run")
                    ' Dim c As Double = 0
                    Dim a As Boolean = False
                    If read.Item("permission_level") = "full" Then
                        If Not My.Settings.AllowFullAccessExtension Then
                            Console.WriteLine("""" & read.Item("name").ToString & """" & " has requested full permissions to Gooder Tools, do you want to allow this?")
                            If Console.ReadLine = "y" Then
                                '    If authenticate() Then
                                If My.Settings.verboseLogging Then
                                    logger("d", read.Item("name").ToString & " is running as a system extension")
                                End If
                                a = True
                                'End If
                            Else
                                If My.Settings.verboseLogging Then
                                    logger("d", read.Item("name").ToString & " requested full access permission but was denied")
                                End If

                                a = False
                            End If
                        Else
                            If My.Settings.verboseLogging Then
                                logger("d", read.Item("name").ToString & " permission level: User")
                            End If

                            a = True
                        End If
                    Else
                        If My.Settings.verboseLogging Then
                            logger("d", read.Item("name").ToString & " permission level: System")
                        End If

                    End If
ilove:
                    If read.Item("system_permission_required") = "true" And Not a Then
                        If My.Settings.verboseLogging Then
                            logger("d", read.Item("name").ToString & " was denied system permissions but is specified to only run when given system permissions, suspending extension.")
                        End If
                        Console.WriteLine(read.Item("name").ToString & " cannot run unless given full permissions, suspending program.")
                        GoTo skip
                    End If
                    'Console.WriteLine(f.ToString & " was created by " & read.Item("author").ToString())
                    c = 0
                    Do Until c > Val(read.Item("total_steps").ToString)
                        c += 1
                        checkExtensionStep(read.Item("step" & c.ToString), a)

                    Loop
skip:
                    a = False
                    'Else
                    'logger("d", "Extension " & f.ToString & " was denied permission to run")
                    'Console.WriteLine("Extension was not given permission to run")
                    'End If

                Catch
                    Console.WriteLine("An error occured when running the extensions, either one of them are not programmed correctly or they weren't installed properly")
                    logger("e", ErrorToString)
                End Try
            Else
                Console.WriteLine("Setup already complete, try running a program.")
            End If
        Catch
            logger("e", "Gooder Tools was started directly, a handled except has occured.")
        End Try
        '        My.Settings.FileAssociationSet = False
        '        My.Settings.Save()
        'unitedfruit:
        '        If My.Settings.FileAssociationSet Then
        '            If authenticate() Then
        '                runAllExtensions()
        '                command()
        '            End If
        'Else
        '    Try
        '        My.Computer.Registry.ClassesRoot.CreateSubKey(".gte").SetValue("", "Gooder Tools Executable", Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.ClassesRoot.CreateSubKey("Hello\shell\open\command").SetValue("", IO.Path.GetDirectoryName(Diagnostics.Process.GetCurrentProcess().MainModule.FileName) & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
        '        Console.WriteLine("Configuration set")
        '        My.Settings.FileAssociationSet = True
        '        My.Settings.Save()
        '        GoTo unitedfruit
        '    Catch
        '        Console.WriteLine("An error occured")
        '        Console.ReadLine()
        '    End Try
        'End If
    End Sub

    Public Function changeStep(ByRef d As Double)
        Module1.c = Val(d)
    End Function
    Public Function runAllExtensions()
        Try
            Dim dir As New DirectoryInfo("C:/GT/extensions/")
            For Each f In dir.GetFiles()
                changeStep(1)
                Dim read = Newtonsoft.Json.Linq.JObject.Parse(AESDecrypt(My.Computer.FileSystem.ReadAllText("C:/GT/extensions/" & f.ToString), generateKeys(), "ok"))
                logger("d", "Extension " & read.Item("name").ToString & " successfully loaded")
                extensionName = read.Item("name")
                '   Console.WriteLine("Extension author: " & read.Item("creator").ToString)
                '     Console.WriteLine("Are you sure you want to run this extension? Running extensions without reviewing their code first can be dangerous")
                '    If Console.ReadLine = "y" Then
                '   logger("d", "Extension " & f.ToString & " was given permission to run")
                ' Dim c As Double = 0
                Dim a As Boolean = False
                If read.Item("permission_level") = "full" Then
                    If SystemExtensionCheck(EncryptSHA256Managed(AESDecrypt(My.Computer.FileSystem.ReadAllText("C:/GT/extensions/" & f.ToString), generateKeys(), "ok"))) Then
                        a = True
                        If My.Settings.verboseLogging Then
                            logger("d", read.Item("name").ToString & " was automatically given system level permissions")
                        End If
                        GoTo ilove
                    End If
                    If Not My.Settings.AllowFullAccessExtension Then
                        Console.WriteLine(read.Item("name").ToString & " has requested full permissions to Gooder Tools, do you want to allow this? This will give the extension the highest permissions Gooder Tools has to offer to extensions. We don't recommend it unless you trust this extension.")
                        If Console.ReadLine = "y" Then
                            '    If authenticate() Then
                            If My.Settings.verboseLogging Then
                                logger("d", read.Item("name").ToString & " is running as a system extension")
                            End If
                            a = True
                            'End If
                        Else
                            If My.Settings.verboseLogging Then
                                logger("d", read.Item("name").ToString & " requested full access permission but was denied")
                            End If

                            a = False
                        End If
                    Else
                        If My.Settings.verboseLogging Then
                            logger("d", read.Item("name").ToString & " permission level: User")
                        End If

                        a = True
                    End If
                Else
                    If My.Settings.verboseLogging Then
                        logger("d", read.Item("name").ToString & " permission level: System")
                    End If

                End If
ilove:
                If read.Item("system_permission_required") = "true" And Not a Then
                    If My.Settings.verboseLogging Then
                        logger("d", read.Item("name").ToString & " was denied system permissions but is specified to only run when given system permissions, suspending extension.")
                    End If
                    Console.WriteLine(read.Item("name").ToString & " cannot run unless given full permissions, suspending extension.")
                    GoTo skip
                End If
                'Console.WriteLine(f.ToString & " was created by " & read.Item("author").ToString())
                c = 0
                Do Until c > Val(read.Item("total_steps").ToString)
                    c += 1
                    checkExtensionStep(read.Item("step" & c.ToString), a)

                Loop
skip:
                a = False
                'Else
                'logger("d", "Extension " & f.ToString & " was denied permission to run")
                'Console.WriteLine("Extension was not given permission to run")
                'End If
            Next
        Catch
            Console.WriteLine("An error occured when running the extensions, either one of them are not programmed correctly or they weren't installed properly")
            logger("e", ErrorToString)
        End Try
    End Function
    Public Function authenticate()
reload:


        Try
            If My.Settings.ignoreSecurity Then
                Return True
            End If
            If My.Computer.FileSystem.FileExists("C:/GT/pass.auth") Then
                Dim pasw As String = My.Computer.FileSystem.ReadAllText("C:/GT/pass.auth")
                If Not EncryptSHA256Managed(EncryptSHA256Managed(pasw)) = My.Settings.PasswordHash Then
                    logger("e", "Password hash mismatch!")
                    logger("e", "Password tampering detected, putting Gooder Tools into safety mode")
                    Console.WriteLine("Security check failed")
                    lockdownMode()
                End If
                Console.WriteLine("Please enter your password: ")
                Dim p As String = Console.ReadLine()

                If pasw = EncryptSHA256Managed(p) Then
                    p = vbNull
                    Console.WriteLine("Password is correct")
                    isAuthenticated = True
                    logger("d", "A successful login just took place")
                    Return True
                Else
                    Console.WriteLine("Password is incorrect")
                    logger("e", "A failed login attempt just took place")
                    GoTo reload
                End If
            ElseIf My.Computer.FileSystem.FileExists("C:/GT/pass.auth") & My.Settings.PasswordHash = "" Then
                If My.Settings.verboseLogging Then
                    logger("e", "Password file is on the disk but there's no password hash in the registry!")
                    logger("e", "Password tampering detected, putting Gooder Tools into safety mode")
                End If

                Console.WriteLine("Security check failed")
                lockdownMode()
            Else
messup:
                If My.Settings.verboseLogging Then
                    logger("d", "No password has been set, requesting user to set password")
                End If

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
                    Main()
                Else
                    GoTo messup
                End If
            End If
        Catch
            logger("e", ErrorToString)
            Console.WriteLine("A fatal error has occured, please restart the program and try again")
            Console.ReadLine()
        End Try
    End Function

    Public Function lockdownMode()
Par:
        Console.ReadLine()
        GoTo Par
    End Function

End Module
