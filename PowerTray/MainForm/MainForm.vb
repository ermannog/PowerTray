Imports System.ComponentModel

Public Class MainForm
    Private isInitializing As Boolean = True

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.nicMain.Icon = My.Resources.PowerTrayGreen
        Me.nicMain.Text = My.Application.Info.Title
        Me.mniNotifyIconSettings.Font = New Font(Me.mniNotifyIconSettings.Font, FontStyle.Bold)

        'Impostazione Panel Output
        Me.pnlMain.UtilSetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        'Lettura impostazioni
        Try
            PowerTrayConfiguration.Load()
        Catch ex As Exception
            Util.ShowErrorException("Error during load settings.", ex, False)
        End Try

        Me.MainFormInitializeWithSettings()

        Me.ExecuteScriptsInitialize()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        ' https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.onshown?view=netframework-4.7.2
        ' The Shown event occurs whenever the form is first shown.
        ' When overriding OnShown(EventArgs) in a derived class, be sure to call the base class's OnShown(EventArgs) method
        ' so that registered delegates receive the event.
        MyBase.OnShown(e)
        Me.Hide()

        'Reset initializing flag
        Me.isInitializing = False
    End Sub

    Private Sub MainForm_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        Me.Hide()
    End Sub
    Private Sub MainForm_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed
        Me.nicMain.Visible = False
    End Sub

    Private Sub MainFormInitializeWithSettings()
        'Impostazione size e location
        If Me.Size <> PowerTrayConfiguration.OutputConsoleSize Then
            Me.Size = PowerTrayConfiguration.OutputConsoleSize
            Me.SetFormLocation()
        End If
    End Sub

    Private Sub mniNotifyIconExit_Click(sender As Object, e As EventArgs) Handles mniNotifyIconExit.Click
        Me.Close()
    End Sub

    Private Sub nicMain_DoubleClick(sender As Object, e As EventArgs) Handles nicMain.DoubleClick
        Me.mniNotifyIconOpen.PerformClick()
    End Sub

    Private Sub mniNotifyIconOpen_Click(sender As Object, e As EventArgs) Handles mniNotifyIconOpen.Click
        If Not Me.Visible Then
            'Dim y As Integer = My.Computer.Screen.WorkingArea.Top
            'If Cursor.Position.Y > My.Computer.Screen.WorkingArea.Height / 2 Then
            '    y = My.Computer.Screen.WorkingArea.Bottom - Me.Height
            'End If

            'Dim x = My.Computer.Screen.WorkingArea.Left
            'If Cursor.Position.X > My.Computer.Screen.WorkingArea.Width / 2 Then
            '    x = My.Computer.Screen.WorkingArea.Right - Me.Width
            'End If

            'Me.Location = New Point(x, y)
            Me.SetFormLocation()
            Me.Visible = True
            Me.Activate()
        Else
            Me.Visible = False
        End If
    End Sub

    Private Sub SetFormLocation()
        Dim y As Integer = My.Computer.Screen.WorkingArea.Top
        If Cursor.Position.Y > My.Computer.Screen.WorkingArea.Height / 2 Then
            y = My.Computer.Screen.WorkingArea.Bottom - Me.Height
        End If

        Dim x = My.Computer.Screen.WorkingArea.Left
        If Cursor.Position.X > My.Computer.Screen.WorkingArea.Width / 2 Then
            x = My.Computer.Screen.WorkingArea.Right - Me.Width
        End If

        Me.Location = New Point(x, y)
    End Sub

    Private Sub mniNotifyIconSettings_Click(sender As Object, e As EventArgs) Handles mniNotifyIconSettings.Click
        'Disabilitazione voce menu contestuale settings per evitare avvii multipli
        Me.mniNotifyIconSettings.Enabled = False

        'Stop del timer di esecuzione scripts
        Me.tmrExecuteScripts.Stop()

        Try
            Using frm As New SettingsForm
                If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    'Rilettura delle impostazoni
                    Try
                        PowerTrayConfiguration.Load()
                    Catch ex As Exception
                        Util.ShowErrorException("Error during load settings.", ex, False)
                    End Try

                    'Inizializzazione main form con nuove impostazioni
                    Me.MainFormInitializeWithSettings()

                    'Clear Scripts execute info
                    UtilExecuteScripts.ClearScriptsExecuteInfo()

                    'Forzatura esecuzione scripts di avvio
                    UtilExecuteScripts.ExecuteScriptsAsync(PSScriptSettings.ExecutionModes.OnStartup, Me)
                End If
            End Using
        Catch ex As Exception
            Util.ShowErrorException("Error during open settings.", ex, False)
        End Try

        'Start del timer di esecuzione scripts
        Me.tmrExecuteScripts.Start()

        'Riabilitazione voce menu contestuale settings
        Me.mniNotifyIconSettings.Enabled = True
    End Sub

#Region "Draw Output"
    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint
        Try
            'Draw output or error
            If Not UtilExecuteScripts.ExecutionError Then
                'Draw output
                For Each executeInfo In UtilExecuteScripts.ScriptsExecuteInfo
                    Dim outputText = executeInfo.Value.Output
                    If Not String.IsNullOrEmpty(executeInfo.Key.Label) Then
                        Select Case executeInfo.Key.LabelPosition
                            Case PSScriptSettings.LabelPositions.Left
                                outputText = executeInfo.Key.Label + outputText
                            Case PSScriptSettings.LabelPositions.Right
                                outputText += executeInfo.Key.Label
                            Case PSScriptSettings.LabelPositions.Up
                                outputText = executeInfo.Key.Label + ControlChars.NewLine + outputText
                            Case PSScriptSettings.LabelPositions.Down
                                outputText += ControlChars.NewLine + executeInfo.Key.Label
                        End Select
                    End If
                    e.Graphics.DrawString(outputText, System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, executeInfo.Key.OutputLocation)
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

End Class