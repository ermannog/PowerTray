Module MainModule
    Public ReturnValue As Integer = 0

    Public ReadOnly Property ApplicationAssemby As System.Reflection.Assembly
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly()
        End Get
    End Property

    Public ReadOnly Property ApplicationGuid As String
        Get
            Dim guidAttributes = MainModule.ApplicationAssemby.GetCustomAttributes(GetType(System.Runtime.InteropServices.GuidAttribute), False)
            Return CType(guidAttributes(0), System.Runtime.InteropServices.GuidAttribute).Value
        End Get
    End Property

    Public ReadOnly Property ApplicationTitle As String
        Get
            Dim titleAttributes = MainModule.ApplicationAssemby.GetCustomAttributes(GetType(System.Reflection.AssemblyTitleAttribute), False)
            Return CType(titleAttributes(0), System.Reflection.AssemblyTitleAttribute).Title
        End Get
    End Property

    Function Main(ByVal cmdArgs() As String) As Integer
        Try
            'Use mutex for have single instance in the user session
            Using mutex As New System.Threading.Mutex(False, MainModule.ApplicationGuid)
                Try
                    Dim running As Boolean = Not mutex.WaitOne(0, False)

                    If running Then
                        MainModule.ReturnValue = -1
                    Else
                        Dim applicationContext As New PowerTray.MyApplicationContext()
                        System.Windows.Forms.Application.Run(applicationContext)
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, MainModule.ApplicationTitle)
                    MainModule.ReturnValue = ex.HResult
                End Try
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, MainModule.ApplicationTitle)
            MainModule.ReturnValue = ex.HResult
        End Try

        Return MainModule.ReturnValue
    End Function
End Module

'https://docs.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=netframework-4.7.2
'On a server that is running Terminal Services, a named system mutex can have two levels of visibility.
'If its name begins with the prefix "Global\", the mutex is visible in all terminal server sessions. 
'If its name begins with the prefix "Local\", the mutex is visible only in the terminal server session where it was created. 
'In that case, a separate mutex with the same name can exist in each of the other terminal server sessions on the server. 
'If you do not specify a prefix when you create a named mutex, it takes the prefix "Local\". 
'Within a terminal server session, two mutexes whose names differ only by their prefixes are separate mutexes, 
'and both are visible to all processes in the terminal server session. 
'That is, the prefix names "Global\" and "Local\" describe the scope of the mutex name relative to terminal server sessions, not relative to processes.

'https://csharp.hotexamples.com/examples/-/System.Threading.Mutex/WaitOne/php-system.threading.mutex-waitone-method-examples.html

'http://www.fmsinc.com/free/NewTips/NET/NETtip17.asp
'Dim mut As System.Threading.Mutex =
'   New System.Threading.Mutex(False, Application.ProductName)
'Dim running As Boolean = Not mut.WaitOne(0, False)
'If running Then
'Application.ExitThread()
'Return
'End If
