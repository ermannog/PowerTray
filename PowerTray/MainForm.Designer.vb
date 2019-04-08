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
        Me.nicMain = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cmsNotifyIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mniNotifyIconExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.mniNotifyIconSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mniNotifyIconSettings = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.cmsNotifyIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniNotifyIconSettings, Me.mniNotifyIconSeparator1, Me.mniNotifyIconExit})
        Me.cmsNotifyIcon.Name = "cmsNotifyIcon"
        Me.cmsNotifyIcon.Size = New System.Drawing.Size(126, 54)
        '
        'mniNotifyIconExit
        '
        Me.mniNotifyIconExit.Name = "mniNotifyIconExit"
        Me.mniNotifyIconExit.Size = New System.Drawing.Size(125, 22)
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
        'mniNotifyIconSeparator1
        '
        Me.mniNotifyIconSeparator1.Name = "mniNotifyIconSeparator1"
        Me.mniNotifyIconSeparator1.Size = New System.Drawing.Size(122, 6)
        '
        'mniNotifyIconSettings
        '
        Me.mniNotifyIconSettings.Name = "mniNotifyIconSettings"
        Me.mniNotifyIconSettings.Size = New System.Drawing.Size(152, 22)
        Me.mniNotifyIconSettings.Text = "Settings..."
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
End Class
