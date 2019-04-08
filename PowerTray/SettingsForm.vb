Public Class SettingsForm
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = String.Format(Me.Text, My.Application.Info.Title)

    End Sub
End Class