Public Class SettingsForm1
    Private defaultLabelScriptPropertiesText As String = String.Empty

    Private Sub SettingsForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Impostazione Text  Form
        Me.Text = String.Format(Me.Text, My.Application.Info.Title)

        'Memorizzazione Text Label Script Properties
        Me.defaultLabelScriptPropertiesText = Me.lblScriptProperties.Text

        'Inizializzazione controlli
        Me.InitializeApplicationSettingsControls()
        Me.InitializeScriptsSettingsControls()

        'Me.ResetLabelScriptPropertiesText()

        'Me.prgApplicationSettings.SelectedObject = PowerTrayConfiguration

        'Me.lsvScripts.Items.Clear()
        'For Each script In PowerTrayConfiguration.PSScripts
        '    With Me.lsvScripts.Items.Add(script.Name)
        '        .Tag = script
        '    End With
        'Next
    End Sub

    Private Sub ResetLabelScriptPropertiesText()
        Me.lblScriptProperties.Text = String.Format(Me.defaultLabelScriptPropertiesText, String.Empty)
    End Sub

    Private Sub InitializeApplicationSettingsControls()
        Me.prgApplicationSettings.SelectedObject = Nothing
        Me.prgApplicationSettings.SelectedObject = PowerTray.PowerTrayConfiguration
    End Sub

    Private Sub InitializeScriptsSettingsControls()
        Me.lsvScripts.Clear()
        For Each script In PowerTrayConfiguration.PSScripts
            With Me.lsvScripts.Items.Add(script.Name)
                .Tag = script
            End With
        Next

        Me.ResetLabelScriptPropertiesText()

        Me.prgScriptSettings.SelectedObject = Nothing

        Me.btnRemove.Enabled = False

        Me.btnPreview.Enabled = False
    End Sub

    Private Sub lsvScripts_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lsvScripts.ItemSelectionChanged
        Try
            If Me.lsvScripts.SelectedItems.Count = 1 Then
                Me.lblScriptProperties.Text = String.Format(Me.defaultLabelScriptPropertiesText, Me.lsvScripts.SelectedItems(0).Text)
                Me.prgScriptSettings.SelectedObject = Me.lsvScripts.SelectedItems(0).Tag
                Me.btnRemove.Enabled = True
                Me.btnPreview.Enabled = True
            Else
                Me.ResetLabelScriptPropertiesText()
                Me.prgScriptSettings.SelectedObject = Nothing
                Me.btnRemove.Enabled = False
                Me.btnPreview.Enabled = False
            End If
        Catch ex As Exception
            Util.ShowErrorException("Error during change script selection.", ex, False)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            'Aggiunta script
            Dim script = PowerTray.PSScriptSettings.CreateInstance()
            PowerTray.PowerTrayConfiguration.PSScripts.Add(script)

            'Inizializzaizone controlli scripts
            Me.InitializeScriptsSettingsControls()

            'Selezione nuovo ListViewItem
            Dim listViewitem = DirectCast(Me.lsvScripts.Items.Cast(Of System.Windows.Forms.ListViewItem).Where(Function(item) item.Tag Is script).First(), System.Windows.Forms.ListViewItem)
            listViewitem.Selected = True

            'Fuoco sul ListView
            Me.lsvScripts.Focus()
        Catch ex As Exception
            Util.ShowErrorException("Error during add script.", ex, False)
        End Try
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Try
            Dim script = DirectCast(Me.lsvScripts.SelectedItems(0).Tag, PowerTray.PSScriptSettings)

            'Richiesta conferma rimozione script
            If Util.ShowQuestion(String.Format("Confirm the removal of the script '{0}'?", script.Name)) = DialogResult.No Then Exit Sub

            'Rimozione script
            PowerTray.PowerTrayConfiguration.PSScripts.Remove(script)

            'Inizializzaizone controlli scripts
            Me.InitializeScriptsSettingsControls()
        Catch ex As Exception
            Util.ShowErrorException("Error during remove script.", ex, False)
        End Try
    End Sub
End Class