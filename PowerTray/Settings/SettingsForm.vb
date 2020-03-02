Public Class SettingsForm
    Private defaultLabelScriptPropertiesText As String = String.Empty

    Public Property PowerTrayConfigurationClone As PowerTraySettings

    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Impostazione Text  Form
        Me.Text = String.Format(Me.Text, My.Application.Info.Title)

        'Memorizzazione Text Label Script Properties
        Me.defaultLabelScriptPropertiesText = Me.lblScriptProperties.Text

        'Creazione clone delle impostazioni
        Try
            Me.PowerTrayConfigurationClone = PowerTray.PowerTrayConfiguration.CreateInstance()
        Catch ex As Exception
            Util.ShowErrorException("Error during initialize settings.", ex, False)
            Exit Sub
        End Try

        'Lettura impostazioni
        Try
            Me.PowerTrayConfigurationClone.Load()
        Catch ex As Exception
            Util.ShowErrorException("Error during load settings.", ex, False)
            Exit Sub
        End Try

        'Inizializzazione controlli
        Me.InitializeApplicationSettingsControls()
        Me.InitializeScriptsSettingsControls()

        'Selezione primo script
        If Me.lsvScripts.Items.Count >= 1 Then
            Me.lsvScripts.Items(0).Selected = True
        End If
    End Sub

    Private Sub ResetLabelScriptPropertiesText()
        Me.lblScriptProperties.Text = String.Format(Me.defaultLabelScriptPropertiesText, String.Empty)
    End Sub

    Private Sub InitializeApplicationSettingsControls()
        Me.prgApplicationSettings.SelectedObject = Nothing
        Me.prgApplicationSettings.SelectedObject = Me.PowerTrayConfigurationClone

        'Inizializzaione Button ManageSettingsFile e DeleteManageSettingsFile
        If Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.User Then
            Me.btnManageSettingsFile.Text = "Move settings file to application folder"
            Me.btnManageSettingsFile.Enabled = System.IO.File.Exists(Me.PowerTrayConfigurationClone.UserFilePath)
            Me.btnManageSettingsFile.Visible = True

            Me.btnDeleteSettingsFile.Text = "Delete user settings file"
            Me.btnDeleteSettingsFile.Enabled = System.IO.File.Exists(Me.PowerTrayConfigurationClone.UserFilePath)
            Me.btnDeleteSettingsFile.Visible = True
        ElseIf Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.Application Then
            Me.btnManageSettingsFile.Text = "Copy settings file to user profile"
            Me.btnManageSettingsFile.Enabled = System.IO.File.Exists(Me.PowerTrayConfigurationClone.ApplicationFilePath)
            Me.btnManageSettingsFile.Visible = True

            Me.btnDeleteSettingsFile.Text = "Delete application settings file"
            Me.btnDeleteSettingsFile.Enabled = System.IO.File.Exists(Me.PowerTrayConfigurationClone.ApplicationFilePath)
            Me.btnDeleteSettingsFile.Visible = True
        End If
    End Sub

    Private Sub InitializeScriptsSettingsControls()
        Me.lsvScripts.Clear()
        For Each script In Me.PowerTrayConfigurationClone.PSScripts
            With Me.lsvScripts.Items.Add(script.Name)
                .Tag = script
            End With
        Next

        Me.ResetLabelScriptPropertiesText()

        Me.prgScriptSettings.SelectedObject = Nothing

        Me.btnRemove.Enabled = False
        Me.btnUp.Enabled = False
        Me.btnDown.Enabled = False
        Me.btnPreview.Enabled = False
    End Sub

    Private Sub lsvScripts_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lsvScripts.ItemSelectionChanged
        Try
            If Me.lsvScripts.SelectedItems.Count = 1 Then
                Me.lblScriptProperties.Text = String.Format(Me.defaultLabelScriptPropertiesText, Me.lsvScripts.SelectedItems(0).Text)
                Me.prgScriptSettings.SelectedObject = Nothing
                Me.prgScriptSettings.SelectedObject = Me.lsvScripts.SelectedItems(0).Tag
                Me.btnRemove.Enabled = True
                Me.btnPreview.Enabled = True

                If Me.lsvScripts.Items.Count <= 1 Then
                    Me.btnUp.Enabled = False
                    Me.btnDown.Enabled = False
                ElseIf Me.lsvScripts.SelectedIndices(0) = 0 Then
                    Me.btnUp.Enabled = False
                    Me.btnDown.Enabled = True
                ElseIf Me.lsvScripts.SelectedIndices(0) = Me.lsvScripts.Items.Count - 1 Then
                    Me.btnUp.Enabled = True
                    Me.btnDown.Enabled = False
                Else
                    Me.btnUp.Enabled = True
                    Me.btnDown.Enabled = True
                End If
            Else
                Me.ResetLabelScriptPropertiesText()
                Me.prgScriptSettings.SelectedObject = Nothing
                Me.btnRemove.Enabled = False
                Me.btnUp.Enabled = False
                Me.btnDown.Enabled = False
                Me.btnPreview.Enabled = False
            End If
        Catch ex As Exception
            Util.ShowErrorException("Error during change script selection.", ex, False)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            'Inizializzazione nome script
            Dim scriptNameTemplate = "Script {0:0000}"
            Dim scriptNameIndex = 0
            Dim scriptName = String.Empty

            Do
                If scriptNameIndex = 0 Then
                    scriptNameIndex = Me.PowerTrayConfigurationClone.PSScripts.Count + 1
                Else
                    scriptNameIndex += 1
                End If

                scriptName = String.Format(scriptNameTemplate, scriptNameIndex)
            Loop Until Me.PowerTrayConfigurationClone.PSScripts.FirstOrDefault(Function(s) s.Name = scriptName) Is Nothing

            'Aggiunta script
            Dim script = PowerTray.PSScriptSettings.CreateInstance(scriptName)
            Me.PowerTrayConfigurationClone.PSScripts.Add(script)

            'Inizializzazione controlli scripts
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
            Me.PowerTrayConfigurationClone.PSScripts.Remove(script)

            'Inizializzaizone controlli scripts
            Me.InitializeScriptsSettingsControls()
        Catch ex As Exception
            Util.ShowErrorException("Error during remove script.", ex, False)
        End Try
    End Sub
    Private Sub btnUp_Click(sender As Object, e As EventArgs) Handles btnUp.Click
        Try
            Dim script = DirectCast(Me.lsvScripts.SelectedItems(0).Tag, PowerTray.PSScriptSettings)
            Dim index = Me.PowerTrayConfigurationClone.PSScripts.IndexOf(script)

            Me.PowerTrayConfigurationClone.PSScripts.Remove(script)
            Me.PowerTrayConfigurationClone.PSScripts.Insert(index - 1, script)

            'Inizializzaizone controlli scripts
            Me.InitializeScriptsSettingsControls()

            'Selezione nuovo ListViewItem
            Dim listViewitem = DirectCast(Me.lsvScripts.Items.Cast(Of System.Windows.Forms.ListViewItem).Where(Function(item) item.Tag Is script).First(), System.Windows.Forms.ListViewItem)
            listViewitem.Selected = True

            'Fuoco sul ListView
            Me.lsvScripts.Focus()
        Catch ex As Exception
            Util.ShowErrorException("Error during move up script.", ex, False)
        End Try
    End Sub

    Private Sub btnDown_Click(sender As Object, e As EventArgs) Handles btnDown.Click
        Try
            Dim script = DirectCast(Me.lsvScripts.SelectedItems(0).Tag, PowerTray.PSScriptSettings)
            Dim index = Me.PowerTrayConfigurationClone.PSScripts.IndexOf(script)

            Me.PowerTrayConfigurationClone.PSScripts.Remove(script)
            Me.PowerTrayConfigurationClone.PSScripts.Insert(index + 1, script)

            'Inizializzaizone controlli scripts
            Me.InitializeScriptsSettingsControls()

            'Selezione nuovo ListViewItem
            Dim listViewitem = DirectCast(Me.lsvScripts.Items.Cast(Of System.Windows.Forms.ListViewItem).Where(Function(item) item.Tag Is script).First(), System.Windows.Forms.ListViewItem)
            listViewitem.Selected = True

            'Fuoco sul ListView
            Me.lsvScripts.Focus()
        Catch ex As Exception
            Util.ShowErrorException("Error during move down script.", ex, False)
        End Try
    End Sub

    Private Sub tbcMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbcMain.SelectedIndexChanged
        If Me.tbcMain.SelectedTab Is Me.tbpScripts Then
            Me.lsvScripts.Focus()
        End If
    End Sub

    Private Sub prgScriptSettings_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles prgScriptSettings.PropertyValueChanged
        'Update Selected ListViewItem Text with Name value
        If e.ChangedItem.PropertyDescriptor.Name = "Name" AndAlso
            Me.lsvScripts.SelectedItems.Count = 1 AndAlso
            Me.lsvScripts.SelectedItems(0).Text = e.OldValue.ToString() Then
            Me.lsvScripts.SelectedItems(0).Text = e.ChangedItem.Value.ToString()
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'Salvataggio Impostazioni
        Try
            Me.PowerTrayConfigurationClone.Save()
        Catch ex As Exception
            Util.ShowErrorException("Error during save settings.", ex, False)
        End Try
    End Sub

    Private Sub cmnPropertyGrid_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmnPropertyGrid.Opening
        Dim menu = DirectCast(sender, System.Windows.Forms.ContextMenuStrip)
        Dim propertyGrid = DirectCast(menu.SourceControl, System.Windows.Forms.PropertyGrid)
        If propertyGrid.SelectedGridItem.PropertyDescriptor IsNot Nothing Then
            Dim canResetItem = propertyGrid.SelectedGridItem.PropertyDescriptor.CanResetValue(propertyGrid.SelectedObject)
            Me.cmiPropertyGridReset.Enabled = canResetItem
        End If
    End Sub

    Private Sub cmiPropertyGridReset_Click(sender As Object, e As EventArgs) Handles cmiPropertyGridReset.Click
        Dim menuItem = DirectCast(sender, System.Windows.Forms.ToolStripMenuItem)
        Dim menu = DirectCast(menuItem.Owner, System.Windows.Forms.ContextMenuStrip)
        Dim propertyGrid = DirectCast(menu.SourceControl, System.Windows.Forms.PropertyGrid)

        propertyGrid.SelectedGridItem.PropertyDescriptor.ResetValue(propertyGrid.SelectedObject)
        propertyGrid.Refresh()
    End Sub

    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        Using previewForm As New ScriptPreviewForm
            previewForm.Script = DirectCast(Me.lsvScripts.SelectedItems(0).Tag, PowerTray.PSScriptSettings)
            previewForm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnMoveSettingsFileToApplicationFolder_Click(sender As Object, e As EventArgs) Handles btnManageSettingsFile.Click
        Try
            If Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.User Then
                If Util.ShowQuestion("Are you sure you want to move the settings file from the user profile folder to the application folder?") = DialogResult.No Then Exit Sub

                'Move del file dal profilo utente alla cartella dell'applicazione
                My.Computer.FileSystem.MoveFile(Me.PowerTrayConfigurationClone.UserFilePath, Me.PowerTrayConfigurationClone.ApplicationFilePath, True)
            ElseIf Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.Application Then
                If Util.ShowQuestion("Are you sure you want to move the settings file from the application folder to the user profile folder?") = DialogResult.No Then Exit Sub

                'Move del file dal profilo utente alla cartella dell'applicazione
                My.Computer.FileSystem.CopyFile(Me.PowerTrayConfigurationClone.ApplicationFilePath, Me.PowerTrayConfigurationClone.UserFilePath, True)
            End If
        Catch ex As Exception
            If Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.User Then
                Util.ShowErrorException("Error during move settings file from the user profile folder to the application folder.", ex, True)
            ElseIf Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.Application Then
                Util.ShowErrorException("Error during move settings file from the application folder to the user profile folder.", ex, True)
            Else
                Util.ShowErrorException(ex, True)
            End If
        End Try
    End Sub

    Private Sub btnDeleteSettingsFile_Click(sender As Object, e As EventArgs) Handles btnDeleteSettingsFile.Click
        Try
            If Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.User Then
                If Util.ShowQuestion("Are you sure you want to delete the settings file from the user profile folder?") = DialogResult.No Then Exit Sub

                'Move del file dal profilo utente alla cartella dell'applicazione
                My.Computer.FileSystem.DeleteFile(Me.PowerTrayConfigurationClone.UserFilePath)
            ElseIf Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.Application Then
                If Util.ShowQuestion("Are you sure you want to delete the settings file from the application folder?") = DialogResult.No Then Exit Sub

                'Move del file dal profilo utente alla cartella dell'applicazione
                My.Computer.FileSystem.DeleteFile(Me.PowerTrayConfigurationClone.ApplicationFilePath)
            End If
        Catch ex As Exception
            If Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.User Then
                Util.ShowErrorException("Error during delete settings file from the user profile folder.", ex, True)
            ElseIf Me.PowerTrayConfigurationClone.SourceSettings = PowerTraySettings.SourcesSettings.Application Then
                Util.ShowErrorException("Error during delete settings file from the application folder.", ex, True)
            Else
                Util.ShowErrorException(ex, True)
            End If
        End Try
    End Sub
End Class