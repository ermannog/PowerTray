<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.nicMain = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cmsNotifyIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mniNotifyIconOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mniNotifyIconSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mniNotifyIconSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mniNotifyIconExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.tmrExecuteScripts = New System.Windows.Forms.Timer(Me.components)
        Me.cmsNotifyIcon.SuspendLayout()
        Me.SuspendLayout()
        '
        'nicMain
        '
        Me.nicMain.ContextMenuStrip = Me.cmsNotifyIcon
        Me.nicMain.Text = "NotifyIcon1"
        Me.nicMain.Visible = True
        '
        'cmsNotifyIcon
        '
        Me.cmsNotifyIcon.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmsNotifyIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniNotifyIconOpen, Me.mniNotifyIconSettings, Me.mniNotifyIconSeparator1, Me.mniNotifyIconExit})
        Me.cmsNotifyIcon.Name = "cmsNotifyIcon"
        Me.cmsNotifyIcon.Size = New System.Drawing.Size(130, 88)
        '
        'mniNotifyIconOpen
        '
        Me.mniNotifyIconOpen.Image = CType(resources.GetObject("mniNotifyIconOpen.Image"), System.Drawing.Image)
        Me.mniNotifyIconOpen.Name = "mniNotifyIconOpen"
        Me.mniNotifyIconOpen.Size = New System.Drawing.Size(129, 26)
        Me.mniNotifyIconOpen.Text = "Open"
        '
        'mniNotifyIconSettings
        '
        Me.mniNotifyIconSettings.Image = CType(resources.GetObject("mniNotifyIconSettings.Image"), System.Drawing.Image)
        Me.mniNotifyIconSettings.Name = "mniNotifyIconSettings"
        Me.mniNotifyIconSettings.Size = New System.Drawing.Size(129, 26)
        Me.mniNotifyIconSettings.Text = "Settings..."
        '
        'mniNotifyIconSeparator1
        '
        Me.mniNotifyIconSeparator1.Name = "mniNotifyIconSeparator1"
        Me.mniNotifyIconSeparator1.Size = New System.Drawing.Size(126, 6)
        '
        'mniNotifyIconExit
        '
        Me.mniNotifyIconExit.Name = "mniNotifyIconExit"
        Me.mniNotifyIconExit.Size = New System.Drawing.Size(129, 26)
        Me.mniNotifyIconExit.Text = "Exit"
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.SystemColors.Desktop
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(600, 366)
        Me.pnlMain.TabIndex = 1
        '
        'tmrExecuteScripts
        '
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 366)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TrayForm"
        Me.TopMost = True
        Me.cmsNotifyIcon.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents nicMain As NotifyIcon
    Friend WithEvents cmsNotifyIcon As ContextMenuStrip
    Friend WithEvents mniNotifyIconExit As ToolStripMenuItem
    Friend WithEvents pnlMain As Panel
    Friend WithEvents mniNotifyIconSettings As ToolStripMenuItem
    Friend WithEvents mniNotifyIconSeparator1 As ToolStripSeparator
    Friend WithEvents mniNotifyIconOpen As ToolStripMenuItem
    Friend WithEvents tmrExecuteScripts As Timer
End Class
