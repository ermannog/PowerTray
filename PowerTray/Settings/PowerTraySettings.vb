﻿'https://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
'https://www.codeproject.com/Articles/9517/PropertyGrid-and-Drop-Down-properties
'https://www.codeproject.com/Articles/57760/Exposing-Object-Methods-in-the-PropertyGrid-Comman
'https://www.codeproject.com/Articles/13342/Filtering-properties-in-a-PropertyGrid

Imports System.ComponentModel
Imports System.Drawing.Design

<Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>
<System.ComponentModel.Description("PowerTray settings")>
<Serializable()>
Public Class PowerTraySettings
    Private Const GeneralCategory As String = "General"
    Private Const PSScriptsCategory As String = "PowerShell scripts"

#Region "Proprietà Default"
    Private Shared defaultInstanceValue As New PowerTraySettings

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>
    Public Shared ReadOnly Property [Default]() As PowerTraySettings
        Get
            Return defaultInstanceValue
        End Get
    End Property
#End Region

    <System.ComponentModel.Browsable(False)>
    Public ReadOnly Property FileName As String
        Get
            Dim product = System.Windows.Forms.Application.ProductName
            Return product & ".settings"
        End Get
    End Property

    <System.ComponentModel.Browsable(False)>
    Public ReadOnly Property UserFilePath As String
        Get
            Dim product = System.Windows.Forms.Application.ProductName
            Dim applicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim company = System.Windows.Forms.Application.CompanyName
            Return System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(applicationDataPath, company), product), Me.FileName)
        End Get
    End Property

    <System.ComponentModel.Browsable(False)>
    Public ReadOnly Property ApplicationFilePath As String
        Get
            Dim product = System.Windows.Forms.Application.ProductName
            Return System.IO.Path.Combine(My.Application.Info.DirectoryPath, Me.FileName)
        End Get
    End Property


    <System.ComponentModel.DisplayName("Settings file")>
    <System.ComponentModel.Description("Settings file path")>
    Public ReadOnly Property FilePath() As String
        Get
            'Verifica se è stato specificato un file di configurazione all'avvio
            If Not String.IsNullOrWhiteSpace(My.Application.StartupConfigurationFile) Then
                Me.sourceSettingsValue = PowerTraySettings.SourcesSettings.StartupParameter
                Return My.Application.StartupConfigurationFile
            End If


            'Dim product = System.Windows.Forms.Application.ProductName
            'Dim fileName = product & ".settings"

            'Se esiste un file di impostazione nel profilo utente viene restituito il path di questo file
            'Dim applicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            'Dim company = System.Windows.Forms.Application.CompanyName
            'Dim userSettingsFilePath = System.IO.Path.Combine(
            '        System.IO.Path.Combine(
            '        System.IO.Path.Combine(applicationDataPath, company), product),
            '        Me.FileName)
            If System.IO.File.Exists(Me.UserFilePath) Then
                Me.sourceSettingsValue = PowerTraySettings.SourcesSettings.User
                Return Me.UserFilePath
            End If

            'Se esiste un file di impostazione nel path in cui è stato avviata l'applicazione viene restituito il path di questo file
            'Dim applicationSettingFilePath = System.IO.Path.Combine(My.Application.Info.DirectoryPath, Me.FileName)
            If System.IO.File.Exists(Me.ApplicationFilePath) Then
                Me.sourceSettingsValue = PowerTraySettings.SourcesSettings.Application
                Return Me.ApplicationFilePath
            End If

            'In caso contrario viene restituito il path del file nel profilo utente
            Me.sourceSettingsValue = PowerTraySettings.SourcesSettings.User
            Return Me.UserFilePath
        End Get
    End Property


#Region "Property SourceSettings"
    <TypeConverter(GetType(UtilDescriptionEnumConverter))>
    Public Enum SourcesSettings As UInteger
        <System.ComponentModel.Description("User profile")>
        User
        <System.ComponentModel.Description("Application folder")>
        Application
        <System.ComponentModel.Description("Startup parameter")>
        StartupParameter
    End Enum

    Private sourceSettingsValue As PowerTraySettings.SourcesSettings = PowerTraySettings.SourcesSettings.User

    <System.ComponentModel.DisplayName("Source Settings")>
    <System.ComponentModel.Description("Source of the settings")>
    Public ReadOnly Property SourceSettings As PowerTraySettings.SourcesSettings
        Get
            Return Me.sourceSettingsValue
        End Get
    End Property
#End Region

#Region "Property RefreshInterval"
    Public Const MinimumRefreshInterval As Integer = 1000
    Public Const DefaultRefreshInterval As Integer = 5000
    Private refreshIntervalValue As Integer = PowerTraySettings.DefaultRefreshInterval

    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Refresh interval")>
    <System.ComponentModel.Description("Refresh interval in milliseconds")>
    <System.ComponentModel.DefaultValue(PowerTraySettings.DefaultRefreshInterval)>
    Public Property RefreshInterval As Integer
        Get
            Return Me.refreshIntervalValue
        End Get
        Set(value As Integer)
            If value < PowerTraySettings.MinimumRefreshInterval Then
                Throw New System.ArgumentOutOfRangeException()
            End If
            Me.refreshIntervalValue = value
        End Set
    End Property
#End Region

#Region "Property OutputConsoleSize"
    Public Shared ReadOnly Property DefaultOutputConsoleSize As System.Drawing.Size = New System.Drawing.Size(500, 250)

    Private outputConsoleSizeValue As System.Drawing.Size = PowerTraySettings.DefaultOutputConsoleSize

    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Output console size")>
    Public Property OutputConsoleSize As System.Drawing.Size
        Get
            Return Me.outputConsoleSizeValue
        End Get
        Set(value As System.Drawing.Size)
            Me.outputConsoleSizeValue = value
        End Set
    End Property

    Public Function ShouldSerializeOutputConsoleSize() As Boolean
        Return Not Me.outputConsoleSizeValue.Equals(PowerTraySettings.DefaultOutputConsoleSize)
    End Function

    Public Sub ResetOutputConsoleSize()
        Me.OutputConsoleSize = PowerTraySettings.DefaultOutputConsoleSize
    End Sub
#End Region

#Region "Property OutputConsoleBackColor"
    Public Shared ReadOnly Property DefaultOutputConsoleBackColor As System.Drawing.Color = System.Drawing.Color.FromArgb(1, 36, 86)

    Private outputConsoleBackColorValue As System.Drawing.Color = PowerTraySettings.DefaultOutputConsoleBackColor

    <System.Xml.Serialization.XmlIgnore()>
    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Output console backcolor")>
    Public Property OutputConsoleBackColor As System.Drawing.Color
        Get
            Return Me.outputConsoleBackColorValue
        End Get
        Set(value As System.Drawing.Color)
            Me.outputConsoleBackColorValue = value
        End Set
    End Property

    Public Function ShouldSerializeOutputConsoleBackColor() As Boolean
        Return Not Me.outputConsoleBackColorValue.Equals(PowerTraySettings.DefaultOutputConsoleBackColor)
    End Function

    Public Sub ResetOutputConsoleBackColor()
        Me.OutputConsoleBackColor = PowerTraySettings.DefaultOutputConsoleBackColor
    End Sub

    <System.ComponentModel.Browsable(False)>
    Public Property OutputConsoleBackColorHtml As String
        Get
            Return System.Drawing.ColorTranslator.ToHtml(Me.outputConsoleBackColorValue)
        End Get
        Set(value As String)
            Me.outputConsoleBackColorValue = System.Drawing.ColorTranslator.FromHtml(value)
        End Set
    End Property
    Public Function ShouldSerializeOutputConsoleBackColorHtml() As Boolean
        Return Me.ShouldSerializeOutputConsoleBackColor()
    End Function
#End Region

#Region "Property OutputConsoleForeColor"
    Public Shared ReadOnly Property DefaultOutputConsoleForeColor As System.Drawing.Color = System.Drawing.Color.White

    Private outputConsoleForeColorValue As System.Drawing.Color = PowerTraySettings.DefaultOutputConsoleForeColor

    <System.Xml.Serialization.XmlIgnore()>
    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Output console forecolor")>
    Public Property OutputConsoleForeColor As System.Drawing.Color
        Get
            Return Me.outputConsoleForeColorValue
        End Get
        Set(value As System.Drawing.Color)
            Me.outputConsoleForeColorValue = value
        End Set
    End Property

    Public Function ShouldSerializeOutputConsoleForeColor() As Boolean
        Return Not Me.outputConsoleForeColorValue.Equals(PowerTraySettings.DefaultOutputConsoleForeColor)
    End Function

    Public Sub ResetOutputConsoleForeColor()
        Me.OutputConsoleForeColor = PowerTraySettings.DefaultOutputConsoleForeColor
    End Sub

    <System.ComponentModel.Browsable(False)>
    Public Property OutputConsoleForeColorHtml As String
        Get
            Return System.Drawing.ColorTranslator.ToHtml(Me.outputConsoleForeColorValue)
        End Get
        Set(value As String)
            Me.outputConsoleForeColorValue = System.Drawing.ColorTranslator.FromHtml(value)
        End Set
    End Property
    Public Function ShouldSerializeOutputConsoleForeColorHtml() As Boolean
        Return Me.ShouldSerializeOutputConsoleForeColor()
    End Function
#End Region

#Region "Property OutputConsoleForeColorError"
    Public Shared ReadOnly Property DefaultOutputConsoleForeColorError As System.Drawing.Color = System.Drawing.Color.Red

    Private outputConsoleForeColorErrorValue As System.Drawing.Color = PowerTraySettings.DefaultOutputConsoleForeColorError

    <System.Xml.Serialization.XmlIgnore()>
    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Output console error forecolor")>
    Public Property OutputConsoleForeColorError As System.Drawing.Color
        Get
            Return Me.outputConsoleForeColorErrorValue
        End Get
        Set(value As System.Drawing.Color)
            Me.outputConsoleForeColorErrorValue = value
        End Set
    End Property

    Public Function ShouldSerializeOutputConsoleForeColorError() As Boolean
        Return Not Me.outputConsoleForeColorErrorValue.Equals(PowerTraySettings.DefaultOutputConsoleForeColorError)
    End Function

    Public Sub ResetOutputConsoleForeColorError()
        Me.OutputConsoleForeColorError = PowerTraySettings.DefaultOutputConsoleForeColorError
    End Sub

    <System.ComponentModel.Browsable(False)>
    Public Property OutputConsoleForeColorErrorHtml As String
        Get
            Return System.Drawing.ColorTranslator.ToHtml(Me.outputConsoleForeColorErrorValue)
        End Get
        Set(value As String)
            Me.outputConsoleForeColorErrorValue = System.Drawing.ColorTranslator.FromHtml(value)
        End Set
    End Property
    Public Function ShouldSerializeOutputConsoleForeColorErrorHtml() As Boolean
        Return Me.ShouldSerializeOutputConsoleForeColorError()
    End Function
#End Region

#Region "Property OutputConsoleFont"
    Public Shared ReadOnly Property DefaultOutputConsoleFont As System.Drawing.Font = System.Drawing.SystemFonts.DefaultFont

    Private outputConsoleFontValue As System.Drawing.Font = PowerTraySettings.DefaultOutputConsoleFont

    <System.Xml.Serialization.XmlIgnore()>
    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Output console font")>
    Public Property OutputConsoleFont As System.Drawing.Font
        Get
            Return Me.outputConsoleFontValue
        End Get
        Set(value As System.Drawing.Font)
            Me.outputConsoleFontValue = value
        End Set
    End Property

    Public Function ShouldSerializeOutputConsoleFont() As Boolean
        Dim fc As New System.Drawing.FontConverter
        Return Not (fc.ConvertToInvariantString(Me.outputConsoleFontValue) = fc.ConvertToInvariantString(PowerTraySettings.DefaultOutputConsoleFont))
    End Function

    Public Sub ResetOutputConsoleFont()
        Me.outputConsoleFontValue = PowerTraySettings.DefaultOutputConsoleFont
    End Sub

    <System.ComponentModel.Browsable(False)>
    Public Property OutputConsoleFontInvariantString As String
        Get
            Dim fc As New System.Drawing.FontConverter
            Return fc.ConvertToInvariantString(Me.outputConsoleFontValue)
        End Get
        Set(value As String)
            'Il confronto eseguito con il metodo Equals restisce False per che la Proprietà GdiCharSet differisce
            'https://docs.microsoft.com/en-us/dotnet/api/system.drawing.font.equals
            Dim fc As New System.Drawing.FontConverter
            Me.outputConsoleFontValue = DirectCast(fc.ConvertFromInvariantString(value), System.Drawing.Font)
        End Set
    End Property
    Public Function ShouldSerializeOutputConsoleFontInvariantString() As Boolean
        Return Me.ShouldSerializeOutputConsoleFont()
    End Function
#End Region

#Region "Property PSScripts"
    Private psScriptsValue As New System.Collections.Generic.List(Of PSScriptSettings)

    <System.ComponentModel.Browsable(False)>
    Public Property PSScripts() As System.Collections.Generic.List(Of PSScriptSettings)
        Get
            Return Me.psScriptsValue
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of PSScriptSettings))
            Me.psScriptsValue = value
        End Set
    End Property

    Public Function ShouldSerializePSScripts() As Boolean
        Return Me.psScriptsValue.Count > 0
    End Function
#End Region

#Region "Property PSPredefinedScripts"
    Private Shared psPredefinedScriptsInternal As System.Collections.Generic.Dictionary(Of String, String) = Nothing

    <System.ComponentModel.Browsable(False)>
    Public Shared ReadOnly Property PSPredefinedScripts() As System.Collections.Generic.Dictionary(Of String, String)
        Get
            If PowerTraySettings.psPredefinedScriptsInternal Is Nothing Then
                'La classe System.Collections.Specialized.StringDictionary
                'non è stata utilizzata perchè le key sono gestite in LowerCase

                Dim resourceSet = My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, False, True)
                PowerTraySettings.psPredefinedScriptsInternal = New System.Collections.Generic.Dictionary(Of String, String)

                Const PSQueryPrefix = "PSQuery_"

                For Each resource As System.Collections.DictionaryEntry In resourceSet
                    If TypeOf resource.Value Is String Then
                    End If

                    If TypeOf resource.Value Is String AndAlso resource.Key.ToString().StartsWith(PSQueryPrefix) Then
                        PowerTraySettings.psPredefinedScriptsInternal.Add(resource.Key.ToString().Remove(0, PSQueryPrefix.Length), resource.Value.ToString())
                    End If
                Next
            End If

            Return PowerTraySettings.psPredefinedScriptsInternal
        End Get
    End Property
#End Region

    Private Sub New()
        'Costruttore senza parametri per consentire la serializzazione
        'http://support.microsoft.com/kb/816225/en-us
    End Sub

    Public Overloads Sub Save()
        'Util.XmlSerialize(Me.FilePath, Me, True)
        Util.XmlSerialize(Me.FilePath, Me, False)
    End Sub

    Public Overloads Sub Export(file As String)
        'Util.XmlSerialize(Me.FilePath, Me, True)
        Util.XmlSerialize(file, Me, False)
    End Sub

    Public Function Load() As Boolean
        'Return Util.XmlDeserialize(Me.FilePath, Me, True)
        Return Util.XmlDeserialize(Me.FilePath, Me, False)
    End Function

    Public Function Import(file As String) As Boolean
        'Return Util.XmlDeserialize(Me.FilePath, Me, True)
        If Not Util.XmlDeserialize(file, Me, False) Then Return False

        Return Util.XmlDeserialize(Me.FilePath, Me, False)
    End Function

    Public Sub Clear()
        defaultInstanceValue = Nothing
        defaultInstanceValue = New PowerTraySettings
    End Sub

    Public Function CreateInstance() As PowerTraySettings
        Return New PowerTraySettings
    End Function
End Class


<Global.Microsoft.VisualBasic.HideModuleNameAttribute()>
Public Module PowerTraySettingsDefaultProperty
    Public ReadOnly Property PowerTrayConfiguration() As PowerTraySettings
        Get
            Return PowerTraySettings.Default
        End Get
    End Property
End Module