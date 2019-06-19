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
        Me.rtbLineNumbers = New System.Windows.Forms.RichTextBox()
        Me.rtbScriptText = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'rtbLineNumbers
        '
        Me.rtbLineNumbers.BackColor = System.Drawing.Color.White
        Me.rtbLineNumbers.Dock = System.Windows.Forms.DockStyle.Left
        Me.rtbLineNumbers.Enabled = False
        Me.rtbLineNumbers.Font = New System.Drawing.Font("Lucida Console", 9.0!)
        Me.rtbLineNumbers.Location = New System.Drawing.Point(0, 0)
        Me.rtbLineNumbers.Name = "rtbLineNumbers"
        Me.rtbLineNumbers.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.rtbLineNumbers.Size = New System.Drawing.Size(50, 567)
        Me.rtbLineNumbers.TabIndex = 0
        Me.rtbLineNumbers.Text = ""
        Me.rtbLineNumbers.WordWrap = False
        '
        'rtbScriptText
        '
        Me.rtbScriptText.BackColor = System.Drawing.Color.White
        Me.rtbScriptText.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbScriptText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbScriptText.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbScriptText.Location = New System.Drawing.Point(50, 0)
        Me.rtbScriptText.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbScriptText.Name = "rtbScriptText"
        Me.rtbScriptText.ReadOnly = True
        Me.rtbScriptText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.rtbScriptText.Size = New System.Drawing.Size(729, 567)
        Me.rtbScriptText.TabIndex = 4
        Me.rtbScriptText.Text = ""
        Me.rtbScriptText.WordWrap = False
        '
        'ScriptPreviewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 567)
        Me.Controls.Add(Me.rtbScriptText)
        Me.Controls.Add(Me.rtbLineNumbers)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimizeBox = False
        Me.Name = "ScriptPreviewForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "{0} Preview script [{1}]"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents rtbLineNumbers As RichTextBox
    Friend WithEvents rtbScriptText As RichTextBox
End Class
