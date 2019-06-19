<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tbcMain = New System.Windows.Forms.TabControl()
        Me.tbpApplication = New System.Windows.Forms.TabPage()
        Me.prgApplicationSettings = New System.Windows.Forms.PropertyGrid()
        Me.cmnPropertyGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmiPropertyGridReset = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbpScripts = New System.Windows.Forms.TabPage()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.prgScriptSettings = New System.Windows.Forms.PropertyGrid()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.lblScriptProperties = New System.Windows.Forms.Label()
        Me.lblScripts = New System.Windows.Forms.Label()
        Me.lsvScripts = New System.Windows.Forms.ListView()
        Me.tbcMain.SuspendLayout()
        Me.tbpApplication.SuspendLayout()
        Me.cmnPropertyGrid.SuspendLayout()
        Me.tbpScripts.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(456, 486)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(537, 486)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tbcMain
        '
        Me.tbcMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbcMain.Controls.Add(Me.tbpApplication)
        Me.tbcMain.Controls.Add(Me.tbpScripts)
        Me.tbcMain.Location = New System.Drawing.Point(0, 0)
        Me.tbcMain.Name = "tbcMain"
        Me.tbcMain.SelectedIndex = 0
        Me.tbcMain.Size = New System.Drawing.Size(625, 480)
        Me.tbcMain.TabIndex = 4
        '
        'tbpApplication
        '
        Me.tbpApplication.Controls.Add(Me.prgApplicationSettings)
        Me.tbpApplication.Location = New System.Drawing.Point(4, 22)
        Me.tbpApplication.Name = "tbpApplication"
        Me.tbpApplication.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpApplication.Size = New System.Drawing.Size(617, 454)
        Me.tbpApplication.TabIndex = 0
        Me.tbpApplication.Text = "Application"
        Me.tbpApplication.UseVisualStyleBackColor = True
        '
        'prgApplicationSettings
        '
        Me.prgApplicationSettings.ContextMenuStrip = Me.cmnPropertyGrid
        Me.prgApplicationSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.prgApplicationSettings.Location = New System.Drawing.Point(3, 3)
        Me.prgApplicationSettings.Name = "prgApplicationSettings"
        Me.prgApplicationSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized
        Me.prgApplicationSettings.Size = New System.Drawing.Size(611, 448)
        Me.prgApplicationSettings.TabIndex = 3
        Me.prgApplicationSettings.ToolbarVisible = False
        '
        'cmnPropertyGrid
        '
        Me.cmnPropertyGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmiPropertyGridReset})
        Me.cmnPropertyGrid.Name = "cmnPropertyGrid"
        Me.cmnPropertyGrid.Size = New System.Drawing.Size(103, 26)
        '
        'cmiPropertyGridReset
        '
        Me.cmiPropertyGridReset.Enabled = False
        Me.cmiPropertyGridReset.Name = "cmiPropertyGridReset"
        Me.cmiPropertyGridReset.Size = New System.Drawing.Size(102, 22)
        Me.cmiPropertyGridReset.Text = "Reset"
        '
        'tbpScripts
        '
        Me.tbpScripts.Controls.Add(Me.btnPreview)
        Me.tbpScripts.Controls.Add(Me.btnRemove)
        Me.tbpScripts.Controls.Add(Me.btnAdd)
        Me.tbpScripts.Controls.Add(Me.prgScriptSettings)
        Me.tbpScripts.Controls.Add(Me.btnDown)
        Me.tbpScripts.Controls.Add(Me.btnUp)
        Me.tbpScripts.Controls.Add(Me.lblScriptProperties)
        Me.tbpScripts.Controls.Add(Me.lblScripts)
        Me.tbpScripts.Controls.Add(Me.lsvScripts)
        Me.tbpScripts.Location = New System.Drawing.Point(4, 22)
        Me.tbpScripts.Name = "tbpScripts"
        Me.tbpScripts.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpScripts.Size = New System.Drawing.Size(617, 454)
        Me.tbpScripts.TabIndex = 1
        Me.tbpScripts.Text = "Scripts"
        Me.tbpScripts.UseVisualStyleBackColor = True
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPreview.Enabled = False
        Me.btnPreview.Location = New System.Drawing.Point(533, 425)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(75, 23)
        Me.btnPreview.TabIndex = 7
        Me.btnPreview.Text = "Preview..."
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Enabled = False
        Me.btnRemove.Location = New System.Drawing.Point(89, 425)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 6
        Me.btnRemove.Text = "Remove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Location = New System.Drawing.Point(8, 425)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'prgScriptSettings
        '
        Me.prgScriptSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgScriptSettings.ContextMenuStrip = Me.cmnPropertyGrid
        Me.prgScriptSettings.Location = New System.Drawing.Point(199, 19)
        Me.prgScriptSettings.Name = "prgScriptSettings"
        Me.prgScriptSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized
        Me.prgScriptSettings.Size = New System.Drawing.Size(409, 400)
        Me.prgScriptSettings.TabIndex = 5
        Me.prgScriptSettings.ToolbarVisible = False
        '
        'btnDown
        '
        Me.btnDown.Enabled = False
        Me.btnDown.Image = CType(resources.GetObject("btnDown.Image"), System.Drawing.Image)
        Me.btnDown.Location = New System.Drawing.Point(170, 48)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(23, 23)
        Me.btnDown.TabIndex = 4
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Enabled = False
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.Location = New System.Drawing.Point(170, 19)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(23, 23)
        Me.btnUp.TabIndex = 3
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'lblScriptProperties
        '
        Me.lblScriptProperties.AutoSize = True
        Me.lblScriptProperties.Location = New System.Drawing.Point(196, 3)
        Me.lblScriptProperties.Name = "lblScriptProperties"
        Me.lblScriptProperties.Size = New System.Drawing.Size(74, 13)
        Me.lblScriptProperties.TabIndex = 2
        Me.lblScriptProperties.Text = "Properties {0}:"
        '
        'lblScripts
        '
        Me.lblScripts.AutoSize = True
        Me.lblScripts.Location = New System.Drawing.Point(8, 3)
        Me.lblScripts.Name = "lblScripts"
        Me.lblScripts.Size = New System.Drawing.Size(42, 13)
        Me.lblScripts.TabIndex = 1
        Me.lblScripts.Text = "Scripts:"
        '
        'lsvScripts
        '
        Me.lsvScripts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lsvScripts.HideSelection = False
        Me.lsvScripts.Location = New System.Drawing.Point(8, 19)
        Me.lsvScripts.MultiSelect = False
        Me.lsvScripts.Name = "lsvScripts"
        Me.lsvScripts.Size = New System.Drawing.Size(156, 400)
        Me.lsvScripts.TabIndex = 0
        Me.lsvScripts.UseCompatibleStateImageBehavior = False
        Me.lsvScripts.View = System.Windows.Forms.View.List
        '
        'SettingsForm
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(624, 521)
        Me.Controls.Add(Me.tbcMain)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "SettingsForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "{0} Settings"
        Me.tbcMain.ResumeLayout(False)
        Me.tbpApplication.ResumeLayout(False)
        Me.cmnPropertyGrid.ResumeLayout(False)
        Me.tbpScripts.ResumeLayout(False)
        Me.tbpScripts.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents tbcMain As TabControl
    Friend WithEvents tbpApplication As TabPage
    Friend WithEvents tbpScripts As TabPage
    Friend WithEvents prgApplicationSettings As PropertyGrid
    Friend WithEvents lsvScripts As ListView
    Friend WithEvents lblScripts As Label
    Friend WithEvents btnDown As Button
    Friend WithEvents btnUp As Button
    Friend WithEvents lblScriptProperties As Label
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents prgScriptSettings As PropertyGrid
    Friend WithEvents btnPreview As Button
    Friend WithEvents cmnPropertyGrid As ContextMenuStrip
    Friend WithEvents cmiPropertyGridReset As ToolStripMenuItem
End Class
