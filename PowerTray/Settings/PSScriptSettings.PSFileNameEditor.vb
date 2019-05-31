Imports System.ComponentModel
Imports System.Drawing.Design

Partial Class PSScriptSettings
    Public Class PSFileNameEditor
        Inherits System.Windows.Forms.Design.FileNameEditor

        Protected Overrides Sub InitializeDialog(openFileDialog As OpenFileDialog)
            MyBase.InitializeDialog(openFileDialog)
            openFileDialog.DefaultExt = "ps1"
            openFileDialog.Filter = "PowerShell File|*.ps1|All Files|*.*"

            Me.GetEditStyle()
        End Sub

        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If context IsNot Nothing AndAlso context.PropertyDescriptor.IsReadOnly Then
                Return UITypeEditorEditStyle.None
            End If

            Return UITypeEditorEditStyle.Modal
        End Function
    End Class


End Class
