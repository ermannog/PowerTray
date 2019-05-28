Public Module UtilExecuteScripts

#Region "Property LastExecutionTime"
    Private lastExecutionTimeValue As Date? = Nothing
    Public ReadOnly Property LastExecutionTime As Date?
        Get
            Return UtilExecuteScripts.lastExecutionTimeValue
        End Get
    End Property
#End Region

#Region "Property ExecutionError"
    Private executionErrorValue As Boolean = False
    Public ReadOnly Property ExecutionError As Boolean
        Get
            Return UtilExecuteScripts.executionErrorValue
        End Get
    End Property
#End Region

#Region "Property ScriptsExecuteInfo"
    Private scriptsExecuteInfoValue As New System.Collections.Generic.Dictionary(Of PSScriptSettings, UtilScriptsExecuteInfo)

    Public ReadOnly Property ScriptsExecuteInfo As System.Collections.Generic.Dictionary(Of PSScriptSettings, UtilScriptsExecuteInfo)
        Get
            Return UtilExecuteScripts.scriptsExecuteInfoValue
        End Get
    End Property
#End Region

#Region "Property ErrorMessages"
    Private errorMessagesValue As New System.Collections.Specialized.StringCollection()

    Public ReadOnly Property ErrorMessages As System.Collections.Specialized.StringCollection
        Get
            Return UtilExecuteScripts.errorMessagesValue
        End Get
    End Property
#End Region

    Public Enum ExecuteScriptsModes
        OnOnStart
        OnnOpen
        OnManual
        OnRefreshInterval
    End Enum

    Private executeScriptsAsyncTask As System.Threading.Tasks.Task = Nothing

    'http://codetailor.blogspot.com/2012/03/async-come-trasformare-un-metodo-non.html
    Public Sub ExecuteScriptsAsync(mode As PSScriptSettings.ExecutionModes, mainForm As System.Windows.Forms.Form)
        Try
            If UtilExecuteScripts.isExecutingValue Then
                UtilExecuteScripts.executeScriptsAsyncTask.Wait(5000)
            End If

            UtilExecuteScripts.executeScriptsAsyncTask = System.Threading.Tasks.Task.Run(Sub() UtilExecuteScripts.ExecuteScripts(mode, mainForm))
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage("Error during async scritps execution.", ex))
        End Try
    End Sub

#Region "Proprietà IsExecutingValue"
    Private isExecutingValue As Boolean = False

    Public ReadOnly Property IsExecuting As Boolean
        Get
            Return UtilExecuteScripts.isExecutingValue
        End Get
    End Property
#End Region


    Public Sub ExecuteScripts(mode As PSScriptSettings.ExecutionModes, mainForm As System.Windows.Forms.Form)
        If UtilExecuteScripts.isExecutingValue Then Exit Sub

        'Raise evento ExecuteScriptsStarting
        Try
            Dim e As New System.ComponentModel.CancelEventArgs
            UtilExecuteScripts.OnExecuteScriptsStarting(Nothing, e)
            If e.Cancel Then Exit Sub
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage("Error during raise event ExecuteScriptsStarting.", ex))
        End Try

        UtilExecuteScripts.isExecutingValue = True

        'Raise evento ExecuteScriptsStarted
        Try
            UtilExecuteScripts.OnExecuteScriptsStarted(Nothing, New System.EventArgs)
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage("Error during raise event ExecuteScriptsStarted.", ex))
        End Try

        Try
            UtilExecuteScripts.lastExecutionTimeValue = Now
            UtilExecuteScripts.executionErrorValue = False
            UtilExecuteScripts.errorMessagesValue.Clear()

            For Each script In PowerTrayConfiguration.PSScripts
                'Gestione condizioni in cui lo script non deve essere eseguito
                If Not script.Enabled Then Continue For

                If script.ReExecuteOnlyWhenVisible AndAlso UtilExecuteScripts.scriptsExecuteInfoValue.ContainsKey(script) Then
                    If mainForm Is Nothing OrElse Not mainForm.Visible Then Continue For
                End If

                If Not script.ExecutionMode = mode Then Continue For

                If script.ExecutionMode = PSScriptSettings.ExecutionModes.OnStartupOnly Then
                    If UtilExecuteScripts.scriptsExecuteInfoValue.ContainsKey(script) Then
                        Continue For
                    End If
                End If

                If script.ExecutionMode = PSScriptSettings.ExecutionModes.OnRefreshInterval Then
                    If script.ExecutionInterval.HasValue AndAlso
                            UtilExecuteScripts.scriptsExecuteInfoValue.ContainsKey(script) AndAlso
                            (Now - UtilExecuteScripts.scriptsExecuteInfoValue.Item(script).Date).TotalMilliseconds < script.ExecutionInterval.Value Then
                        Continue For
                    End If
                End If

                'Impostazione sourceScript
                Dim sourceScript = String.Empty

                'If Not UtilExecuteScripts.scriptsOutputValue.ContainsKey(script) Then
                Select Case script.Source
                    Case PSScriptSettings.Sources.Text
                        sourceScript = script.Text
                    Case PSScriptSettings.Sources.File
                        Try
                            sourceScript = System.IO.File.ReadAllText(script.FilePath)
                        Catch ex As Exception
                            UtilExecuteScripts.executionErrorValue = True
                            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during read script file '{0}'", script.FilePath), ex))
                            Continue For
                        End Try
                    Case PSScriptSettings.Sources.PredefinedScript
                        Try
                            sourceScript = PowerTraySettings.PSPredefinedScripts.Item(script.PredefinedScriptName)
                        Catch ex As Exception
                            UtilExecuteScripts.executionErrorValue = True
                            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during read predefined script '{0}'", script.PredefinedScriptName), ex))
                            Continue For
                        End Try
                End Select

                'Else
                '    sourceScript = UtilExecuteScripts.scriptsOutputValue.Item(script)
                'End If

                'Raise evento ScriptExecuting
                Dim utilScriptExecutingEventArgs = New UtilScriptExecutingEventArgs(script)
                UtilExecuteScripts.OnScriptExecuting(script, utilScriptExecutingEventArgs)
                If utilScriptExecutingEventArgs.Cancel Then
                    Continue For
                End If

                'Esecuzione script
                Dim output = String.Empty
                Dim executeInfo As UtilScriptsExecuteInfo = Nothing
                Try
                    output = Util.RunPowerShellScript(sourceScript, script.Timeout)
                    executeInfo = New UtilScriptsExecuteInfo(script, output, Nothing, Now)

                    If UtilExecuteScripts.scriptsExecuteInfoValue.ContainsKey(script) Then
                        UtilExecuteScripts.scriptsExecuteInfoValue(script) = executeInfo
                    Else
                        UtilExecuteScripts.scriptsExecuteInfoValue.Add(script, executeInfo)
                    End If
                Catch ex As Exception
                    UtilExecuteScripts.executionErrorValue = True
                    UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during execute script '{0}'", script.Name), ex))
                    executeInfo = New UtilScriptsExecuteInfo(script, output, ex, Now)
                    Continue For
                End Try

                'Raise evento ScriptExecuted
                Try
                    UtilExecuteScripts.OnScriptExecuted(script, New UtilScriptExecutedEventArgs(script, executeInfo))
                Catch ex As Exception
                    UtilExecuteScripts.executionErrorValue = True
                    UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during during raise event ScriptExecuted for script '{0}'", script.Name), ex))
                    executeInfo = New UtilScriptsExecuteInfo(script, output, ex, Now)
                End Try
            Next
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage("Error during execute scripts.", ex))
        End Try

        UtilExecuteScripts.isExecutingValue = False

        'Raise evento ExecuteScriptsComplete
        Try
            UtilExecuteScripts.OnExecuteScriptsComplete(Nothing, New System.EventArgs)
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage("Error during raise event ExecuteScriptsComplete.", ex))
        End Try
    End Sub

#Region "Gestione Evento ExecuteScriptsStarting"
    'Definizione Evento
    Public Event ExecuteScriptsStarting As System.EventHandler(Of System.ComponentModel.CancelEventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnExecuteScriptsStarting(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        RaiseEvent ExecuteScriptsStarting(sender, e)
    End Sub
#End Region

#Region "Gestione Evento ExecuteScriptsStarted"
    'Definizione Evento
    Public Event ExecuteScriptsStarted As System.EventHandler(Of System.EventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnExecuteScriptsStarted(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ExecuteScriptsStarted(sender, e)
    End Sub
#End Region

#Region "Gestione Evento ScriptExecuting"
    'Definizione Evento
    Public Event ScriptExecuting As System.EventHandler(Of UtilScriptExecutingEventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnScriptExecuting(ByVal sender As Object, ByVal e As UtilScriptExecutingEventArgs)
        RaiseEvent ScriptExecuting(sender, e)
    End Sub
#End Region

#Region "Gestione Evento ScriptExecuted"
    'Definizione Evento
    Public Event ScriptExecuted As System.EventHandler(Of UtilScriptExecutedEventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnScriptExecuted(ByVal sender As Object, ByVal e As UtilScriptExecutedEventArgs)
        RaiseEvent ScriptExecuted(sender, e)
    End Sub
#End Region

#Region "Gestione Evento ExecuteScriptsComplete"
    'Definizione Evento
    Public Event ExecuteScriptsComplete As System.EventHandler(Of System.EventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnExecuteScriptsComplete(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ExecuteScriptsComplete(sender, e)
    End Sub
#End Region

End Module

Public Class UtilScriptExecutingEventArgs
    Inherits System.ComponentModel.CancelEventArgs

    Public Sub New(script As PSScriptSettings)
        MyBase.New()
        Me.scriptValue = script
    End Sub

#Region "Property Script"
    Private scriptValue As PSScriptSettings = Nothing
    Public ReadOnly Property Script As PSScriptSettings
        Get
            Return Me.scriptValue
        End Get
    End Property
#End Region
End Class

Public Class UtilScriptExecutedEventArgs
    Inherits System.EventArgs

    Public Sub New(script As PSScriptSettings, executeInfo As UtilScriptsExecuteInfo)
        MyBase.New()
        Me.scriptValue = script
        Me.executeInfoValue = executeInfo
    End Sub

#Region "Property Script"
    Private scriptValue As PSScriptSettings = Nothing
    Public ReadOnly Property Script As PSScriptSettings
        Get
            Return Me.scriptValue
        End Get
    End Property
#End Region

#Region "Property ExecuteInfo"
    Private executeInfoValue As UtilScriptsExecuteInfo = Nothing
    Public ReadOnly Property ExecuteInfo As UtilScriptsExecuteInfo
        Get
            Return Me.executeInfoValue
        End Get
    End Property
#End Region
End Class

Public Class UtilScriptsExecuteInfo

    Public Sub New(script As PSScriptSettings, output As String, [exception] As System.Exception, [date] As System.DateTime)
        Me.scriptValue = script
        Me.outputValue = output
        Me.exceptionValue = [exception]
        Me.dateValue = [date]
    End Sub

    Private scriptValue As PSScriptSettings = Nothing

    Public ReadOnly Property Script As PSScriptSettings
        Get
            Return Me.scriptValue
        End Get
    End Property

    Private outputValue As String = String.Empty

    Public ReadOnly Property Output As String
        Get
            Return Me.outputValue
        End Get
    End Property

    Private exceptionValue As System.Exception = Nothing

    Public ReadOnly Property Exception As System.Exception
        Get
            Return Me.exceptionValue
        End Get
    End Property

    Private dateValue As System.DateTime = Nothing

    Public ReadOnly Property [Date] As System.DateTime
        Get
            Return Me.dateValue
        End Get
    End Property


End Class
