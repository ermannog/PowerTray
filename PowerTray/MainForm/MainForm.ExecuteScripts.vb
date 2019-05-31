Partial Class MainForm

    Private Sub ExecuteScriptsInitialize()
        'Abilitazione gestione eventi esecuzione scripts
        AddHandler UtilExecuteScripts.ExecuteScriptsStarting, AddressOf UtilExecuteScripts_ExecuteScriptsStarting
        AddHandler UtilExecuteScripts.ExecuteScriptsStarted, AddressOf UtilExecuteScripts_ExecuteScriptsStarted
        AddHandler UtilExecuteScripts.ScriptExecuted, AddressOf UtilExecuteScripts_ScriptExecuted
        AddHandler UtilExecuteScripts.ExecuteScriptsComplete, AddressOf UtilExecuteScripts_ExecuteScriptsComplete

        'Esecuzione scripts di avvio
        UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnStartup, Me)

        'Impostazione e attivazione Timer
        Me.tmrExecuteScripts.Interval = PowerTrayConfiguration.RefreshInterval
        Me.tmrExecuteScripts.Enabled = True
    End Sub

    Private Sub UtilExecuteScripts_ExecuteScriptsStarting(sender As Object, e As System.ComponentModel.CancelEventArgs)
        'Condizioni di blocco
    End Sub

    Private Sub UtilExecuteScripts_ExecuteScriptsStarted(sender As Object, e As System.EventArgs)
        Me.pnlMain.UtilInvokeRefresh()

        'Impostazione icona blue durante esecuzione scripts
        Me.nicMain.Icon = My.Resources.PowerTrayBlue
    End Sub

    Private Sub UtilExecuteScripts_ScriptExecuted(sender As Object, e As UtilScriptExecutedEventArgs)
        Me.pnlMain.UtilInvokeRefresh()
    End Sub

    Private Sub UtilExecuteScripts_ExecuteScriptsComplete(sender As Object, e As System.EventArgs)
        Me.pnlMain.UtilInvokeRefresh()

        'Reimpostazione icona
        If Not UtilExecuteScripts.ExecutionError Then
            Me.nicMain.Icon = My.Resources.PowerTrayGreen
        Else
            Me.nicMain.Icon = My.Resources.PowerTrayRed
        End If
    End Sub

    Private Sub tmrExecuteScripts_Tick(sender As Object, e As EventArgs) Handles tmrExecuteScripts.Tick
        'Esecuzione scripts OnRefreshInterval
        UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnRefreshInterval, Me)
    End Sub

    Private Sub MainFormExecuteScripts_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Not Me.isInitializing AndAlso Me.Visible Then
            'Esecuzione scripts OnOpen
            UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnOpen, Me)
        End If
    End Sub
End Class
