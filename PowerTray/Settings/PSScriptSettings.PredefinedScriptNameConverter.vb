Imports System.ComponentModel
Imports System.Drawing.Design

Partial Class PSScriptSettings

    Public Class PredefinedScriptNameConverter
        Inherits System.ComponentModel.StringConverter

        Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            'true if GetStandardValues() should be called to find a common set of values the object supports
            Return True
        End Function

        Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
            'true if the TypeConverter.StandardValuesCollection returned from GetStandardValues() is an exhaustive list of possible values
            Return True
        End Function

        Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
            'Dim predefinedScripts = Util.GetPredefinedScripts()

            Dim names(PowerTraySettings.PSPredefinedScripts.Count) As String

            names(0) = String.Empty

            If PowerTraySettings.PSPredefinedScripts.Count > 0 Then
                PowerTraySettings.PSPredefinedScripts.Keys.CopyTo(names, 1)
            End If

            Return New StandardValuesCollection(names)
        End Function
    End Class

End Class
