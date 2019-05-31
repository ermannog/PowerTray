Public Class LicenseForm
    Private Sub LicenseForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = String.Format(Me.Text, System.Windows.Forms.Application.ProductName)
        Me.rtbLicense.Rtf = My.Resources.MIT_License
    End Sub

    Private Sub BtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Using prtDlg As New System.Windows.Forms.PrintDialog
            If prtDlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                'Creazione file temporaneo
                Dim tmpFileName = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    My.Application.Info.AssemblyName & "-Eula.rtf")
                Me.rtbLicense.SaveFile(tmpFileName, RichTextBoxStreamType.RichText)

                Dim piStart As New ProcessStartInfo(tmpFileName)
                With piStart
                    .Verb = "Printto"
                    .Arguments = """" & prtDlg.PrinterSettings.PrinterName & """"
                    .CreateNoWindow = True
                    .WindowStyle = ProcessWindowStyle.Hidden
                End With

                Using p As New System.Diagnostics.Process()
                    p.StartInfo = piStart
                    p.Start()
                End Using

                'Eliminazione File
                System.IO.File.Delete(tmpFileName)

                'Rilascio risorse
                piStart = Nothing
            End If
        End Using
    End Sub
End Class