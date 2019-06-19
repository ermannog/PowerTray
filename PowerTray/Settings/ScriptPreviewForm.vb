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
            'Aggiunta nuova riga
            If Me.rtbScriptText.Lines.Length > 0 Then
                Me.rtbScriptText.AppendText(Environment.NewLine)
                Me.rtbLineNumbers.AppendText(Environment.NewLine)
            End If

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
                Me.rtbScriptText.SelectionColor = System.Drawing.Color.Green
            Else
                Me.rtbScriptText.SelectionColor = Me.rtbScriptText.ForeColor
            End If

            'Aggiunta testo
            Me.rtbScriptText.AppendText(line)

            'Aggiunta numero riga
            Me.rtbLineNumbers.SelectionAlignment = HorizontalAlignment.Right
            Me.rtbLineNumbers.AppendText(Me.rtbScriptText.Lines.Length.ToString())
        Next

        Me.rtbScriptText.SelectionStart = 0
    End Sub

#Region "Sincronizzazione vertical scroll tra RichTextBox Lines e ScriptText"
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Int32, ByVal wParam As Int32, ByRef lParam As System.Drawing.Point) As Int32
    End Function

    Private Sub rtbScriptTextc_VScroll(sender As Object, e As EventArgs) Handles rtbScriptText.VScroll
        Const WM_USER As Integer = &H400
        Const EM_GETSCROLLPOS As Integer = WM_USER + 221
        Const EM_SETSCROLLPOS As Integer = WM_USER + 222

        Dim currentScrollPosition As Point
        ScriptPreviewForm.SendMessage(Me.rtbScriptText.Handle, EM_GETSCROLLPOS, 0, currentScrollPosition)

        ScriptPreviewForm.SendMessage(Me.rtbLineNumbers.Handle, EM_SETSCROLLPOS, 0, currentScrollPosition)
    End Sub
#End Region

End Class