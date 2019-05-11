'https://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
'https://www.codeproject.com/Articles/9517/PropertyGrid-and-Drop-Down-properties
'https://www.codeproject.com/Articles/57760/Exposing-Object-Methods-in-the-PropertyGrid-Comman

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

    Private ReadOnly Property FilePath() As String
        Get
            Dim applicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim company = System.Windows.Forms.Application.CompanyName
            Dim product = System.Windows.Forms.Application.ProductName

            Return System.IO.Path.Combine(
                System.IO.Path.Combine(
                    System.IO.Path.Combine(applicationDataPath, company), product),
                    product & ".settings")
        End Get
    End Property

#Region "Property RefreshBackgroundInterval"
    Private Const DefaultRefreshBackgroundInterval As Integer = 5000
    Private refreshBackgroundIntervalValue As Integer = PowerTraySettings.DefaultRefreshBackgroundInterval

    <System.ComponentModel.Category(GeneralCategory)>
    <System.ComponentModel.DisplayName("Refresh background interval")>
    <System.ComponentModel.Description("Refresh background interval in milliseconds")>
    Public Property RefreshBackgroundInterval As Integer
        Get
            Return Me.refreshBackgroundIntervalValue
        End Get
        Set(value As Integer)
            Me.refreshBackgroundIntervalValue = value
        End Set
    End Property

    Private Function ShouldSerializeRefreshBackgroundInterval() As Boolean
        Return Me.refreshBackgroundIntervalValue <> PowerTraySettings.DefaultRefreshBackgroundInterval
    End Function
#End Region

#Region "Property PSScripts"
    Private psScriptsValue As New System.Collections.Generic.List(Of PSScriptSettings)

    <System.ComponentModel.Category(PSScriptsCategory)>
    <System.ComponentModel.Description("PowerShell scripts")>
    <System.ComponentModel.DisplayName("PowerShell scripts list")>
    Public Property PSScripts() As System.Collections.Generic.List(Of PSScriptSettings)
        Get
            Return Me.psScriptsValue
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of PSScriptSettings))
            Me.psScriptsValue = value
        End Set
    End Property

    Private Function ShouldSerializePSScripts() As Boolean
        Return Me.psScriptsValue.Count > 0
    End Function
#End Region

    Private Sub New()
        'Costruttore senza parametri per consentire la serializzazione
        'http://support.microsoft.com/kb/816225/en-us
    End Sub

    Public Sub Save()
        'Util.XmlSerialize(Me.FilePath, Me, True)
        Util.XmlSerialize(Me.FilePath, Me, False)
    End Sub

    Public Function Load() As Boolean
        'Return Util.XmlDeserialize(Me.FilePath, Me, True)
        Return Util.XmlDeserialize(Me.FilePath, Me, False)
    End Function

    Public Sub Clear()
        defaultInstanceValue = Nothing
        defaultInstanceValue = New PowerTraySettings
    End Sub
End Class


<Global.Microsoft.VisualBasic.HideModuleNameAttribute()>
Public Module PowerTraySettingsDefaultProperty
    Public ReadOnly Property PowerTrayConfiguration() As PowerTraySettings
        Get
            Return PowerTraySettings.Default
        End Get
    End Property
End Module

<System.ComponentModel.RefreshProperties(RefreshProperties.All)>
Public Class PSScriptSettings

    Private Sub New()
        Me.nameValue = "Script name"

        'Inizializzazione Source per corretta inizializzazione del PropertyGrid
        Me.Source = PSScriptSettings.DefaultSource
    End Sub

    Private Const GeneralCategory As String = "General"
    Private Const SourceCategory As String = "Source"
    Private Const LayoutCategory As String = "Layout"
    Private Const BehaviourCategory As String = "Behaviour"

#Region "Property Name"
    Private nameValue As String = String.Empty

    <System.ComponentModel.Category(PSScriptSettings.GeneralCategory)>
    Public Property Name() As String
        Get
            Return Me.nameValue
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Throw New System.ArgumentNullException()
            End If

            Me.nameValue = value
        End Set
    End Property
#End Region

#Region "Property Source"
    Public Enum Sources As Integer
        Text
        File
        Predefined
    End Enum

    Private Const DefaultSource As PSScriptSettings.Sources = Sources.Text
    Private sourceInternal As PSScriptSettings.Sources = PSScriptSettings.DefaultSource

    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultSource)>
    Public Property Source() As PSScriptSettings.Sources
        Get
            Return Me.sourceInternal
        End Get
        Set(ByVal value As PSScriptSettings.Sources)
            Me.sourceInternal = value

            ' Set Browsable Attribute
            Const PredefinedScriptNamePropertyName = "PredefinedScriptName"
            Const TextPropertyName = "Text"
            Const FilePathPropertyName = "FilePath"
            Select Case Me.sourceInternal
                Case Sources.Text
                    Util.SetBrowsableAttribute(Me.GetType(), TextPropertyName, True)
                    Util.SetBrowsableAttribute(Me.GetType(), FilePathPropertyName, False)
                    Me.FilePath = String.Empty
                    Util.SetBrowsableAttribute(Me.GetType(), PredefinedScriptNamePropertyName, False)
                    Me.PredefinedScriptName = String.Empty
                Case Sources.File
                    Util.SetBrowsableAttribute(Me.GetType(), TextPropertyName, False)
                    Me.Text = String.Empty
                    Util.SetBrowsableAttribute(Me.GetType(), FilePathPropertyName, True)
                    Util.SetBrowsableAttribute(Me.GetType(), PredefinedScriptNamePropertyName, False)
                    Me.PredefinedScriptName = String.Empty
                Case Sources.Predefined
                    Util.SetBrowsableAttribute(Me.GetType(), TextPropertyName, False)
                    Me.Text = String.Empty
                    Util.SetBrowsableAttribute(Me.GetType(), FilePathPropertyName, False)
                    Me.FilePath = String.Empty
                    Util.SetBrowsableAttribute(Me.GetType(), PredefinedScriptNamePropertyName, True)
            End Select
        End Set
    End Property
#End Region

#Region "Property Text"
    Private textInternalValue As String = String.Empty

    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Script text")>
    <System.ComponentModel.Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.DefaultValue("")>
    Public Property Text() As String
        Get
            Return Me.textInternalValue
        End Get
        Set(ByVal value As String)
            Me.textInternalValue = value
            If String.IsNullOrWhiteSpace(Me.textInternalValue) Then
                Me.textInternalValue = String.Empty
            End If
        End Set
    End Property
#End Region

#Region "Property FilePath"
    Private filePathInternalValue As String = String.Empty

    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Script file path")>
    <System.ComponentModel.Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))>
    <System.ComponentModel.Browsable(False)>
    <System.ComponentModel.DefaultValue("")>
    Public Property FilePath() As String
        Get
            Return Me.filePathInternalValue
        End Get
        Set(ByVal value As String)
            Me.filePathInternalValue = value
            If String.IsNullOrWhiteSpace(Me.filePathInternalValue) Then
                Me.filePathInternalValue = String.Empty
            End If
        End Set
    End Property
#End Region

#Region "Property PredefinedScriptName"
    Private predefinedScriptNameInternalValue As String = String.Empty

    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Predefined script name")>
    <System.ComponentModel.TypeConverter(GetType(ScriptNameConverter))>
    <System.ComponentModel.Browsable(False)>
    <System.ComponentModel.DefaultValue("")>
    Public Property PredefinedScriptName() As String
        Get
            Return Me.predefinedScriptNameInternalValue
        End Get
        Set(ByVal value As String)
            Me.predefinedScriptNameInternalValue = value
            If String.IsNullOrWhiteSpace(Me.predefinedScriptNameInternalValue) Then
                Me.predefinedScriptNameInternalValue = String.Empty
            End If
        End Set
    End Property

    Public Class ScriptNameConverter
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
            Dim predefinedScripts = Util.GetPredefinedScripts()
            Dim names(predefinedScripts.Count) As String

            names(0) = String.Empty

            If predefinedScripts.Count > 0 Then
                predefinedScripts.Keys.CopyTo(names, 1)
            End If

            Return New StandardValuesCollection(names)
        End Function
    End Class
#End Region

#Region "Property Enabled"
    Private Const DefaultEnabled As Boolean = True
    Private enabledValue As Boolean = PSScriptSettings.DefaultEnabled

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Enabled")>
    <System.ComponentModel.Description("Enable execution of the script")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultEnabled)>
    Public Property Enabled() As Boolean
        Get
            Return Me.enabledValue
        End Get
        Set(ByVal value As Boolean)
            Me.enabledValue = value
        End Set
    End Property
#End Region

#Region "Property ExecutionCacheEnabled"
    Private Const DefaultExecutionCacheEnabled As Boolean = False
    Private executionCacheEnabledValue As Boolean = PSScriptSettings.DefaultExecutionCacheEnabled

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Execution cache enabled")>
    <System.ComponentModel.Description("Enable the execution cache")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultExecutionCacheEnabled)>
    Public Property ExecutionCacheEnabled() As Boolean
        Get
            Return Me.executionCacheEnabledValue
        End Get
        Set(ByVal value As Boolean)
            Me.executionCacheEnabledValue = value

            ' Set ReadOnly Attribute
            Const ExecutionCachePropertyName = "ExecutionCache"
            Util.SetReadOnlyAttribute(Me.GetType(), ExecutionCachePropertyName, Not value)
        End Set
    End Property
#End Region

#Region "Property ExecutionCache"
    Private Const DefaultExecutionCache As UInteger = 60000
    Private executionCacheValue As UInteger = PSScriptSettings.DefaultExecutionCache

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Execution cache [ms]")>
    <System.ComponentModel.Description("Execution cache in milli seconds")>
    <System.ComponentModel.ReadOnly(True)>
    Public Property ExecutionCache() As UInteger
        Get
            Return Me.executionCacheValue
        End Get
        Set(ByVal value As UInteger)
            Me.executionCacheValue = value
        End Set
    End Property

    Private Function ShouldSerializeExecutionCache() As Boolean
        Return Me.executionCacheValue <> PSScriptSettings.DefaultExecutionCache
    End Function
#End Region

#Region "Property ShowOutput"
    Private Const DefaultShowOutput As Boolean = True
    Private showOutputValue As Boolean = PSScriptSettings.DefaultShowOutput

    <System.ComponentModel.Category(PSScriptSettings.LayoutCategory)>
    <System.ComponentModel.DisplayName("Show output")>
    <System.ComponentModel.Description("Show execution output of the script")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultShowOutput)>
    Public Property ShowOutput() As Boolean
        Get
            Return Me.showOutputValue
        End Get
        Set(ByVal value As Boolean)
            Me.showOutputValue = value
        End Set
    End Property
#End Region

#Region "Property OutputLocation"
    Private outputLocationValue As System.Drawing.Point = System.Drawing.Point.Empty

    <System.ComponentModel.Category(PSScriptSettings.LayoutCategory)>
    <System.ComponentModel.DisplayName("Output location")>
    <System.ComponentModel.Description("Location where output will be show")>
    Public Property OutputLocation() As System.Drawing.Point
        Get
            Return Me.outputLocationValue
        End Get
        Set(ByVal value As System.Drawing.Point)
            Me.outputLocationValue = value
        End Set
    End Property

    Private Function ShouldSerializeOutputLocation() As Boolean
        Return Not Me.outputLocationValue.Equals(System.Drawing.Point.Empty)
    End Function
#End Region

    Public Overrides Function ToString() As String
        Return Me.nameValue
    End Function
End Class