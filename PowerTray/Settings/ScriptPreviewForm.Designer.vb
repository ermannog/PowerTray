<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScriptPreviewForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lsvMain = New System.Windows.Forms.ListView()
        Me.rtbMain = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'lsvMain
        '
        Me.lsvMain.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lsvMain.Dock = System.Windows.Forms.DockStyle.Left
        Me.lsvMain.Enabled = False
        Me.lsvMain.Font = New System.Drawing.Font("Lucida Console", 9.0!)
        Me.lsvMain.Location = New System.Drawing.Point(0, 0)
        Me.lsvMain.Name = "lsvMain"
        Me.lsvMain.Size = New System.Drawing.Size(40, 567)
        Me.lsvMain.TabIndex = 1
        Me.lsvMain.UseCompatibleStateImageBehavior = False
        Me.lsvMain.View = System.Windows.Forms.View.List
        '
        'rtbMain
        '
        Me.rtbMain.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbMain.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbMain.Location = New System.Drawing.Point(40, 0)
        Me.rtbMain.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbMain.Name = "rtbMain"
        Me.rtbMain.ReadOnly = True
        Me.rtbMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.rtbMain.Size = New System.Drawing.Size(739, 567)
        Me.rtbMain.TabIndex = 3
        Me.rtbMain.Text = ""
        Me.rtbMain.WordWrap = False
        '
        'ScriptPreviewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 567)
        Me.Controls.Add(Me.rtbMain)
        Me.Controls.Add(Me.lsvMain)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimizeBox = False
        Me.Name = "ScriptPreviewForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "{0} Preview script [{1}]"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lsvMain As ListView
    Friend WithEvents rtbMain As RichTextBox
End Class
