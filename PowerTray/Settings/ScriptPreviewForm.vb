Public Class ScriptPreviewForm

#Region "Property Script"
    Private scriptValue As PowerTray.PSScriptSettings = Nothing
    Public Property Script As PowerTray.PSScriptSettings
        Get
            Return Me.scriptValue
        End Get
        Set(value As PowerTray.PSScriptSettings)
            Me.scriptValue = value
        End Set
    End Property
#End Region


    Private Sub ScriptPreviewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Impostazione Text  Form
        Me.Text = String.Format(Me.Text, My.Application.Info.Title, Me.scriptValue.Name)

        'Load dello script
        Dim scriptText = String.Empty
        If Me.Script.Source = PSScriptSettings.Sources.PredefinedScript Then
            scriptText = PowerTraySettings.PSPredefinedScripts.Item(Me.scriptValue.PredefinedScriptName)
        End If

        Dim lines As String() = scriptText.Split(New String() {Environment.NewLine}, StringSplitOptions.None)

        Dim commentBlock = False
        Dim commentLine = False
        For Each line In lines
            'Ricerca commenti
            If line.StartsWith("<#") Then
                commentBlock = True
                commentLine = True
            ElseIf line.StartsWith("#>") Then
                commentBlock = False
                commentLine = True
            ElseIf line.StartsWith("#") Then
                commentLine = True
            ElseIf commentBlock Then
                commentLine = True
            Else
                commentLine = False
            End If

            'Colorazione commenti
            If commentLine Then
                Me.rtbMain.SelectionColor = System.Drawing.Color.Green
            Else
                Me.rtbMain.SelectionColor = Me.rtbMain.ForeColor
            End If

            'Aggiunta testo
            Me.rtbMain.AppendText(line)
            Me.rtbMain.AppendText(Environment.NewLine)

            'Agginta numero riga
            Me.lsvMain.Items.Add((Me.lsvMain.Items.Count + 1).ToString())
        Next



    End Sub
End Class