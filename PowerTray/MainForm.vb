Imports System.ComponentModel

Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.nicMain.Icon = My.Resources.PowerTrayGreen
        Me.nicMain.Text = My.Application.Info.Title
        Me.mniNotifyIconSettings.Font = New Font(Me.mniNotifyIconSettings.Font, FontStyle.Bold)

        'Lettura impostazioni
        Try
            PowerTryConfiguration.Load()
        Catch ex As Exception
            Util.ShowErrorException("Error during load settings.", ex, False)
        End Try

        Util.RunPowerShellScript2(My.Resources.PSQuery_IPv4Info.ToString())
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
        Using frm As New SettingsForm
            'frm.Icon = System.Drawing.Icon.FromHandle(
            '    DirectCast(DirectCast(sender, ToolStripMenuItem).Image, System.Drawing.Bitmap).GetHicon)
            frm.prgMain.SelectedObject = PowerTryConfiguration

            If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                'Salvataggio Impostazioni
                Try
                    PowerTryConfiguration.Save()
                Catch ex As Exception
                    Util.ShowErrorException("Error during save settings.", ex, False)
                End Try

                '    'Aggiornameno impostazioni timer
                '    Me.SetTimer()

                '    'Refresh
                '    'Me.RefreshUI(True)
                '    'Me.btnRefresh.PerformClick()
                'Else
                '    'Per evitare problemi di aggiornamento
                '    'si esegue un refresh di tutti i controlli
                '    'Util.RefreshControlAndChilds(Me)
            End If
        End Using
    End Sub

    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint
        Using graphics = Me.pnlMain.CreateGraphics()
            ' graphics.DrawLine(System.Drawing.Pens.Azure, 50, 100, 150, 200)
            graphics.DrawString("test CaptionFont", System.Drawing.SystemFonts.CaptionFont, System.Drawing.SystemBrushes.HighlightText, 50, 0)
            graphics.DrawString("test DefaultFont", System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.HighlightText, 50, 20)
            graphics.DrawString("test DialogFont", System.Drawing.SystemFonts.DialogFont, System.Drawing.SystemBrushes.HighlightText, 50, 40)
            graphics.DrawString("test IconTitleFont", System.Drawing.SystemFonts.IconTitleFont, System.Drawing.SystemBrushes.HighlightText, 50, 60)
            graphics.DrawString("test MenuFont", System.Drawing.SystemFonts.MenuFont, System.Drawing.SystemBrushes.HighlightText, 50, 80)
            graphics.DrawString("test MessageBoxFont", System.Drawing.SystemFonts.MessageBoxFont, System.Drawing.SystemBrushes.HighlightText, 50, 100)
            graphics.DrawString("test SmallCaptionFont", System.Drawing.SystemFonts.SmallCaptionFont, System.Drawing.SystemBrushes.HighlightText, 50, 120)
            graphics.DrawString("test StatusFont", System.Drawing.SystemFonts.StatusFont, System.Drawing.SystemBrushes.HighlightText, 50, 140)
        End Using

    End Sub
End Class