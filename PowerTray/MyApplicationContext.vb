Public Class MyApplicationContext
    Inherits System.Windows.Forms.ApplicationContext

    Private notifyIcon As System.Windows.Forms.NotifyIcon = Nothing
    Private notifyIconContextMenuStrip As System.Windows.Forms.ContextMenuStrip = Nothing
    Private appActive As Boolean = False

    Public Sub New()
        AddHandler Application.ApplicationExit, AddressOf OnApplicationExit

        notifyIconContextMenuStrip = New System.Windows.Forms.ContextMenuStrip()
        notifyIconContextMenuStrip.Items.Add("Exit1")
        notifyIconContextMenuStrip.Items.Add("Exit")

        Me.notifyIcon = New NotifyIcon()
        Me.notifyIcon.Icon = My.Resources.PowerTrayGreen
        Me.notifyIcon.Text = "The app is active."
        Me.notifyIcon.ContextMenuStrip = notifyIconContextMenuStrip

        AddHandler Me.notifyIcon.MouseClick, AddressOf OnIconMouseClick

        Me.appActive = True
        Me.notifyIcon.Visible = True


    End Sub


    Private Sub OnApplicationExit(ByVal sender As Object, ByVal e As EventArgs)
        If Me.notifyIcon IsNot Nothing Then
            Me.notifyIcon.Dispose()
        End If
    End Sub



    Private Sub OnIconMouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)

        If e.Button = MouseButtons.Left Then
            Me.appActive = Not appActive
            Me.notifyIcon.Icon = If(Me.appActive, My.Resources.PowerTrayGreen, My.Resources.PowerTrayGray)
            Me.notifyIcon.Text = If(Me.appActive, "The app is active.", "The app is not active.")
        Else
            If MsgBox("Do you want to Exit?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Me.notifyIcon.Visible = False
                ExitThread()
            End If
        End If
    End Sub

End Class