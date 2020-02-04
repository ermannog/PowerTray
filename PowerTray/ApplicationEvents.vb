Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Public Modalita As ModalitaApplicative = ModalitaApplicative.GUI
        Public Enum ModalitaApplicative As Integer
            GUI = 0
            Batch = 1
        End Enum

        Const CommandLineOptionIDHelp As String = "/?"
        Const CommandLineOptionConfigurationFile As String = "/config:"

        Private startupConfigurationFileValue As String = String.Empty
        Public ReadOnly Property StartupConfigurationFile As String
            Get
                Return Me.startupConfigurationFileValue
            End Get
        End Property



        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            'Check License 
            Try
                If Not Util.CheckEulaAccepted(My.Application.Info.CompanyName, My.Application.Info.ProductName) Then
                    e.Cancel = True
                    System.Environment.Exit(0)
                End If
            Catch ex As Exception
                Util.ShowErrorException("Error during check EULA accepted.", ex, True)
                e.Cancel = True
                Exit Sub
            End Try

            'Se l'applicativo viene avviato senza parametri si visualizza la form di configurazione
            If e.CommandLine.Count = 0 Then
                Me.Modalita = ModalitaApplicative.GUI
                Exit Sub
            End If

            'Gestione Help
            If e.CommandLine.Contains(MyApplication.CommandLineOptionIDHelp, StringComparer.OrdinalIgnoreCase) Then
                Dim helpText As String = String.Format("PowerTray [{0}""file path""]",
                                                        MyApplication.CommandLineOptionConfigurationFile) & ControlChars.NewLine
                helpText &= MyApplication.CommandLineOptionConfigurationFile & " - PowerTray configuration file" & ControlChars.NewLine

                Util.ShowMessage(helpText)
                e.Cancel = True
                Exit Sub
            End If

            Dim commandlineOptionsProcessed = 0

            'Gestione Configuration File
            Try
                Dim commandLineConfigurationFile = e.CommandLine.Where(Function(p) p.IndexOf(MyApplication.CommandLineOptionConfigurationFile, StringComparison.CurrentCultureIgnoreCase) >= 0).SingleOrDefault()
                If Not String.IsNullOrWhiteSpace(commandLineConfigurationFile) Then
                    commandlineOptionsProcessed += 1
                    Me.startupConfigurationFileValue = System.Environment.ExpandEnvironmentVariables(commandLineConfigurationFile.Remove(0, MyApplication.CommandLineOptionConfigurationFile.Length))
                    If Not System.IO.File.Exists(Me.startupConfigurationFileValue) Then
                        Util.ShowError(String.Format("PowerTray configuration file {0} not exists.", Me.startupConfigurationFileValue), True)
                    End If
                End If
            Catch ex As Exception
                Util.ShowErrorException("Error processing PowerTray configuration file", ex, True)
            End Try

            ''Estrazione Parametri
            'Try
            '    Dim commandLineParameters = e.CommandLine.Where(Function(p) p.IndexOf(MyApplication.CommandLineOptionIDParameter, StringComparison.CurrentCultureIgnoreCase) >= 0)
            '    For Each clp In commandLineParameters
            '        Dim parameterNameValue = clp.Remove(0, MyApplication.CommandLineOptionIDParameter.Length)
            '        Dim equalSignIndex = parameterNameValue.IndexOf("="c)
            '        If equalSignIndex >= 0 Then
            '            Dim name = parameterNameValue.Substring(0, equalSignIndex)
            '            Dim value = parameterNameValue.Substring(equalSignIndex + 1, parameterNameValue.Length - equalSignIndex - 1)
            '            Me.startupParametersValue.Add(name, value)
            '            commandlineOptionsProcessed += 1
            '        Else
            '            UtilMsgBox.ShowError(String.Format("Incorrect parameter option {0}", parameterNameValue), True)
            '        End If
            '    Next
            'Catch ex As Exception
            '    UtilMsgBox.ShowErrorException("Error processing parameters", ex, True)
            'End Try

            'Check opzioni a riga di comando sconosciute
            If commandlineOptionsProcessed < e.CommandLine.Count Then
                Util.ShowError(String.Format("Error unknown options {0}", e.CommandLine.Count - commandlineOptionsProcessed), True)
            End If

        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            'Close della MainForm
            If Me.MainForm IsNot Nothing AndAlso Not Me.MainForm.IsDisposed Then
                Me.MainForm.Close()
            End If
        End Sub
    End Class
End Namespace
