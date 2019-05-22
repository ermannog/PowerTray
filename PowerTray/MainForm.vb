Imports System.ComponentModel
Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.nicMain.Icon = My.Resources.PowerTrayGreen
        Me.nicMain.Text = My.Application.Info.Title
        Me.mniNotifyIconSettings.Font = New Font(Me.mniNotifyIconSettings.Font, FontStyle.Bold)

        'Impostazione Panel Output
        ' Me.pnlMain.UtilSetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)

        Me.pnlMain.UtilSetStyle(ControlStyles.DoubleBuffer, True)

        'Me.pnlMain.SetStyle(ControlStyles.UserPaint, True)
        'Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        'Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        'Lettura impostazioni
        Try
            PowerTrayConfiguration.Load()
        Catch ex As Exception
            Util.ShowErrorException("Error during load settings.", ex, False)
        End Try

        'Abilitazione gestione eventi esecuzuione scripts
        AddHandler UtilExecuteScripts.ExecuteScriptsStarting, AddressOf UtilExecuteScripts_ExecuteScriptsStarting
        AddHandler UtilExecuteScripts.ExecuteScriptsStarted, AddressOf UtilExecuteScripts_ExecuteScriptsStarted
        AddHandler UtilExecuteScripts.ScriptExecuted, AddressOf UtilExecuteScripts_ScriptExecuted
        AddHandler UtilExecuteScripts.ExecuteScriptsComplete, AddressOf UtilExecuteScripts_ExecuteScriptsComplete

        'Esecuzione scripts di avvio
        UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnStartupOnly)

        'Impostazione e attivazione Timer
        Me.tmrExecuteScripts.Interval = PowerTrayConfiguration.RefreshInterval
        Me.tmrExecuteScripts.Enabled = True

        'Me.ExecuteScripts()

        'Util.RunPowerShellScript2(My.Resources.PSQuery_IPv4Info.ToString())
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        ' https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.onshown?view=netframework-4.7.2
        ' The Shown event occurs whenever the form is first shown.
        ' When overriding OnShown(EventArgs) in a derived class, be sure to call the base class's OnShown(EventArgs) method
        ' so that registered delegates receive the event.
        MyBase.OnShown(e)
        Me.Hide()
    End Sub
    Private Sub MainForm_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        Me.Hide()
    End Sub
    Private Sub MainForm_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed
        Me.nicMain.Visible = False
    End Sub

    Private Sub mniNotifyIconExit_Click(sender As Object, e As EventArgs) Handles mniNotifyIconExit.Click
        Me.Close()
    End Sub

    Private Sub nicMain_DoubleClick(sender As Object, e As EventArgs) Handles nicMain.DoubleClick
        Me.mniNotifyIconOpen.PerformClick()
    End Sub

    Private Sub mniNotifyIconOpen_Click(sender As Object, e As EventArgs) Handles mniNotifyIconOpen.Click
        If Not Me.Visible Then
            Dim y As Integer = My.Computer.Screen.WorkingArea.Top
            If Cursor.Position.Y > My.Computer.Screen.WorkingArea.Height / 2 Then
                y = My.Computer.Screen.WorkingArea.Bottom - Me.Height
            End If

            Dim x = My.Computer.Screen.WorkingArea.Left
            If Cursor.Position.X > My.Computer.Screen.WorkingArea.Width / 2 Then
                x = My.Computer.Screen.WorkingArea.Right - Me.Width
            End If

            Me.Location = New Point(x, y)
            Me.Visible = True
            Me.Activate()
        Else
            Me.Visible = False
        End If
    End Sub

    Private Sub mniNotifyIconSettings_Click(sender As Object, e As EventArgs) Handles mniNotifyIconSettings.Click
        Me.mniNotifyIconSettings.Enabled = False

        'Stop del timer di esecuzione scripts
        Me.tmrExecuteScripts.Stop()

        Try
            Using frm As New SettingsForm
                frm.prgMain.SelectedObject = PowerTrayConfiguration

                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    'Salvataggio Impostazioni
                    Try
                        PowerTrayConfiguration.Save()
                    Catch ex As Exception
                        Util.ShowErrorException("Error during save settings.", ex, False)
                    End Try
                End If
            End Using
        Catch ex As Exception
            Util.ShowErrorException("Error during open settings.", ex, False)
        End Try

        'Start del timer di esecuzione scripts
        Me.tmrExecuteScripts.Start()

        'Util.SetWaitCursor(False)

        Me.mniNotifyIconSettings.Enabled = True
    End Sub

#Region "Draw Output"
    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint
        'Using graphics = Me.pnlMain.CreateGraphics()
        '    'graphics.DrawLine(System.Drawing.Pens.Azure, 50, 100, 150, 200)
        '    'graphics.DrawString("test CaptionFont", System.Drawing.SystemFonts.CaptionFont, System.Drawing.SystemBrushes.HighlightText, 50, 0)
        '    'graphics.DrawString("test DefaultFont", System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, 50, 20)
        '    'graphics.DrawString("test DialogFont", System.Drawing.SystemFonts.DialogFont, System.Drawing.SystemBrushes.HighlightText, 50, 40)
        '    'graphics.DrawString("test IconTitleFont", System.Drawing.SystemFonts.IconTitleFont, System.Drawing.SystemBrushes.HighlightText, 50, 60)
        '    'graphics.DrawString("test MenuFont", System.Drawing.SystemFonts.MenuFont, System.Drawing.SystemBrushes.HighlightText, 50, 80)
        '    'graphics.DrawString("test MessageBoxFont", System.Drawing.SystemFonts.MessageBoxFont, System.Drawing.SystemBrushes.HighlightText, 50, 100)
        '    'graphics.DrawString("test SmallCaptionFont", System.Drawing.SystemFonts.SmallCaptionFont, System.Drawing.SystemBrushes.HighlightText, 50, 120)
        '    'graphics.DrawString("test StatusFont", System.Drawing.SystemFonts.StatusFont, System.Drawing.SystemBrushes.HighlightText, 50, 140)
        'End Using


        Try
            ' Dim graphics = e.Graphics
            ' Using graphics = Me.pnlMain.CreateGraphics()

            'Draw output or error
            If Not UtilExecuteScripts.ExecutionError Then
                'Draw output
                For Each executeInfo In UtilExecuteScripts.ScriptsExecuteInfo
                    e.Graphics.DrawString(executeInfo.Value.Output, System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, executeInfo.Key.OutputLocation)
                Next
            Else
                'Draw error
                If UtilExecuteScripts.ErrorMessages.Count >= 1 Then
                    Me.DrawErrorText(UtilExecuteScripts.ErrorMessages(0), e.Graphics)
                Else
                    Me.DrawErrorText("Error during excuting scripts!", e.Graphics)
                End If
            End If

            'Draw last execution time
            Dim lastExecutionLocation As New Point(Me.pnlMain.ClientSize.Width - 130, (Me.pnlMain.ClientSize.Height - 12))
            If UtilExecuteScripts.IsExecuting Then
                e.Graphics.DrawString("Execution in progress...", System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, lastExecutionLocation)
            ElseIf UtilExecuteScripts.LastExecutionTime.HasValue Then
                e.Graphics.DrawString(String.Format("Last execution: {0}", UtilExecuteScripts.LastExecutionTime.Value.ToLongTimeString()), System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, lastExecutionLocation)
            Else
                e.Graphics.DrawString("Last execution: never", System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, lastExecutionLocation)
            End If
        Catch ex As Exception
            Me.DrawErrorText(Util.GetExceptionMessage("Error during draw output.", ex), e.Graphics)
        End Try
    End Sub

    Private Sub DrawErrorText(text As String, graphics As System.Drawing.Graphics)
        Const Offset = 10
        Dim rectangleF = New System.Drawing.RectangleF(Offset, Offset, graphics.VisibleClipBounds.Width - Offset, graphics.VisibleClipBounds.Height - Offset)

        graphics.DrawString(text, System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.Red, rectangleF)
    End Sub
#End Region

#Region "Execute Scripts"
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

    'Private Sub pnlMain_Refresh()
    '    If Me.pnlMain.InvokeRequired Then
    '        Me.pnlMain.BeginInvoke(New Action(Of Object, System.EventArgs)(AddressOf UtilExecuteScripts_ScriptsExecuted), sender, e)
    '    Else
    '        Me.pnlMain.Refresh()
    '    End If
    'End Sub


    'Private executeScriptsErrorText As String = String.Empty
    'Private scriptsOutput As New System.Collections.Generic.Dictionary(Of String, String)

    'Private Sub ExecuteScripts()
    '    Me.executeScriptsErrorText = String.Empty

    '    For Each script In PowerTrayConfiguration.PSScripts
    '        If Not script.Enabled Then Continue For

    '        'Impostazione sourceScript
    '        Dim sourceScript = String.Empty

    '        If Not Me.scriptsOutput.ContainsKey(script.Name) Then
    '            Select Case script.Source
    '                Case PSScriptSettings.Sources.Text

    '                Case PSScriptSettings.Sources.File

    '                Case PSScriptSettings.Sources.Predefined
    '                    Try
    '                        sourceScript = Util.GetPredefinedScripts().Item(script.PredefinedScriptName)
    '                    Catch ex As Exception
    '                        'Me.DrawErrorText(Util.GetExceptionMessage("Error during read predefined scripts", ex), Graphics)
    '                        Me.executeScriptsErrorText = Util.GetExceptionMessage(String.Format("Error during read predefined script '{0}'", script.PredefinedScriptName), ex)
    '                        Exit Sub
    '                    End Try
    '            End Select
    '        Else
    '            sourceScript = Me.scriptsOutput.Item(script.Name)
    '        End If

    '        'Esecuzione script
    '        'Me.scriptsOutput.Item(script.Name) = Util.RunPowerShellScript(sourceScript)
    '        'System.Threading.Thread.Sleep(20000)
    '    Next
    'End Sub


    'Private tmrMainLock As New Object
    'Private tmrMainRunning As Boolean = False
    'Private tmrMainLastExecution As Date = Date.MinValue

    'Private xtmrExecuteScriptsError As String = String.Empty

    Private Sub tmrExecuteScripts_Tick(sender As Object, e As EventArgs) Handles tmrExecuteScripts.Tick
        'Esecuzione scripts
        UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnRefreshInterval)
    End Sub

#End Region

End Class