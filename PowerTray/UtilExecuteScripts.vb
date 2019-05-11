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

#Region "Property ScriptsOutput"
    Private scriptsOutputValue As New System.Collections.Generic.Dictionary(Of PSScriptSettings, String)

    Public ReadOnly Property ScriptsOutput As System.Collections.Generic.Dictionary(Of PSScriptSettings, String)
        Get
            Return UtilExecuteScripts.scriptsOutputValue
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

    Public Sub ExecuteScripts()
        Try
            UtilExecuteScripts.lastExecutionTimeValue = Now
            UtilExecuteScripts.executionErrorValue = False
            UtilExecuteScripts.errorMessagesValue.Clear()

            For Each script In PowerTrayConfiguration.PSScripts
                If Not script.Enabled Then Continue For

                'Impostazione sourceScript
                Dim sourceScript = String.Empty

                If Not UtilExecuteScripts.scriptsOutputValue.ContainsKey(script) Then
                    Select Case script.Source
                        Case PSScriptSettings.Sources.Text
                            sourceScript = script.Text
                        Case PSScriptSettings.Sources.File
                            Try
                                sourceScript = System.IO.File.ReadAllText(script.FilePath)
                            Catch ex As Exception
                                UtilExecuteScripts.executionErrorValue = True
                                UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during read script '{0}'", script.PredefinedScriptName), ex))
                                Continue For
                            End Try
                        Case PSScriptSettings.Sources.Predefined
                            Try
                                sourceScript = Util.GetPredefinedScripts().Item(script.PredefinedScriptName)
                            Catch ex As Exception
                                UtilExecuteScripts.executionErrorValue = True
                                UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during read predefined script '{0}'", script.PredefinedScriptName), ex))
                                Continue For
                            End Try
                    End Select
                Else
                    sourceScript = UtilExecuteScripts.scriptsOutputValue.Item(script)
                End If

                'Esecuzione script
                Dim output = String.Empty

                Try
                    output = Util.RunPowerShellScript(sourceScript)
                    If UtilExecuteScripts.scriptsOutputValue.ContainsKey(script) Then
                        UtilExecuteScripts.scriptsOutputValue(script) = output
                    Else
                        UtilExecuteScripts.scriptsOutputValue.Add(script, output)
                    End If
                Catch ex As Exception
                    UtilExecuteScripts.executionErrorValue = True
                    UtilExecuteScripts.errorMessagesValue.Add(Util.GetExceptionMessage(String.Format("Error during execute script '{0}'", script.PredefinedScriptName), ex))
                    Continue For
                End Try

                UtilExecuteScripts.OnScriptExecuted(Nothing, New UtilScriptExecutedEventArgs(script, output))
            Next
        Catch ex As Exception
            UtilExecuteScripts.executionErrorValue = True
            Util.GetExceptionMessage("Error during execute scripts.", ex)
        End Try

        UtilExecuteScripts.OnScriptsExecuted(Nothing, New System.EventArgs)
    End Sub

#Region "Gestione Evento ScriptExecuted"
    'Definizione Evento
    Public Event ScriptExecuted As System.EventHandler(Of UtilScriptExecutedEventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnScriptExecuted(ByVal sender As Object, ByVal e As UtilScriptExecutedEventArgs)
        RaiseEvent ScriptExecuted(sender, e)
    End Sub
#End Region

#Region "Gestione Evento ScriptsExecuted"
    'Definizione Evento
    Public Event ScriptsExecuted As System.EventHandler(Of System.EventArgs)

    'Definizione Sub per il Raise dell'evento
    Private Sub OnScriptsExecuted(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ScriptsExecuted(sender, e)
    End Sub
#End Region

End Module


Public Class UtilScriptExecutedEventArgs
    Inherits System.EventArgs

    Public Sub New(script As PSScriptSettings, scriptOutput As String)
        MyBase.New()
        Me.scriptValue = script
        Me.scriptOutputValue = scriptOutput
    End Sub

#Region "Property Script"
    Private scriptValue As PSScriptSettings = Nothing
    Public ReadOnly Property Script As PSScriptSettings
        Get
            Return Me.scriptValue
        End Get
    End Property
#End Region

#Region "Property ScriptOutput"
    Private scriptOutputValue As String = String.Empty
    Public ReadOnly Property ScriptOutput As String
        Get
            Return Me.scriptOutputValue
        End Get
    End Property
#End Region
End Class
