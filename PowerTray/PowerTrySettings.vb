'https://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
'https://www.codeproject.com/Articles/9517/PropertyGrid-and-Drop-Down-properties

Imports System.ComponentModel

<Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>
<System.ComponentModel.Description("PowerTry settings")>
<Serializable()>
Public Class PowerTrySettings
    Private Const IPv4InfoCategory As String = "IPv4 Info"
    Private Const PSScriptsCategory As String = "PowerShell scripts"

#Region "Proprietà Default"
    Private Shared defaultInstanceValue As New PowerTrySettings

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)>
    Public Shared ReadOnly Property [Default]() As PowerTrySettings
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

#Region "Property ShowIPv4Info"
    Private Const DefaultShowIPv4Info As Boolean = False
    Private showIPv4InfoValue As Boolean = PowerTrySettings.DefaultShowIPv4Info

    <System.ComponentModel.Category(IPv4InfoCategory)>
    <System.ComponentModel.DisplayName("Show IPv4 info")>
    <System.ComponentModel.Description("Show IPv4 adresses")>
    <System.ComponentModel.DefaultValue(PowerTrySettings.DefaultShowIPv4Info)>
    Public Property ShowIPv4Info As Boolean
        Get
            Return Me.showIPv4InfoValue
        End Get
        Set(value As Boolean)
            Me.showIPv4InfoValue = value
        End Set
    End Property

    Private Function ShouldSerializeShowIPv4Info() As Boolean
        Return Me.showIPv4InfoValue <> False
    End Function
#End Region

#Region "Property IPv4InfoLocation"
    Private ipv4InfoLocationValue As System.Drawing.Point = System.Drawing.Point.Empty

    <System.ComponentModel.Category(IPv4InfoCategory)>
    <System.ComponentModel.DisplayName("IPv4 info location")>
    <System.ComponentModel.Description("Location where IPv4 info will be show")>
    Public Property IPv4InfoLocation() As System.Drawing.Point
        Get
            Return Me.ipv4InfoLocationValue
        End Get
        Set(ByVal value As System.Drawing.Point)
            Me.ipv4InfoLocationValue = value
        End Set
    End Property

    Private Function ShouldSerializeIPv4InfoLocation() As Boolean
        Return Not Me.IPv4InfoLocation.Equals(System.Drawing.Point.Empty)
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
        defaultInstanceValue = New PowerTrySettings
    End Sub
End Class


<Global.Microsoft.VisualBasic.HideModuleNameAttribute()>
Public Module HVGuestConsoleSettingsDefaultProperty
    Public ReadOnly Property PowerTryConfiguration() As PowerTrySettings
        Get
            Return PowerTrySettings.Default
        End Get
    End Property
End Module

<System.ComponentModel.RefreshProperties(RefreshProperties.All)>
Public Class PSScriptSettings

    Private Sub New()
        Me.nameValue = "Script name"
    End Sub

#Region "Name"
    Private nameValue As String = String.Empty

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

    Public Enum ScriptSources As Integer
        Text
        File
        Predefined
    End Enum

#Region "Source"
    Private Const DefaultSource As PSScriptSettings.ScriptSources = ScriptSources.Text
    Private sourceValue As PSScriptSettings.ScriptSources = PSScriptSettings.DefaultSource

    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultSource)>
    Public Property Source() As PSScriptSettings.ScriptSources
        Get
            Return Me.sourceValue
        End Get
        Set(ByVal value As PSScriptSettings.ScriptSources)
            Me.sourceValue = value

            ' Set ReadOnly Attribute
            Const PredefinedScriptNamePropertyName = "PredefinedScriptName"
            Select Case Me.sourceValue
                Case ScriptSources.Text
                    Util.SetReadOnlyAttribute(Me.GetType(), PredefinedScriptNamePropertyName, True)
                Case ScriptSources.File
                    Util.SetReadOnlyAttribute(Me.GetType(), PredefinedScriptNamePropertyName, True)
                Case ScriptSources.Predefined
                    Util.SetReadOnlyAttribute(Me.GetType(), PredefinedScriptNamePropertyName, False)
            End Select
        End Set
    End Property
#End Region

#Region "PredefinedScriptName"
    Private predefinedScriptNameValue As String = String.Empty

    <System.ComponentModel.TypeConverter(GetType(ScriptNameConverter))>
    <System.ComponentModel.ReadOnly(True)>
    Public Property PredefinedScriptName() As String
        Get
            Return Me.predefinedScriptNameValue
        End Get
        Set(ByVal value As String)
            Me.predefinedScriptNameValue = value
        End Set
    End Property

    Private Function ShouldSerializePredefinedScriptName() As Boolean
        Return Me.predefinedScriptNameValue IsNot String.Empty
    End Function

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
            Dim names(Util.GetPredefinedScripts().Count) As String

            names(0) = String.Empty

            If Util.GetPredefinedScripts().Count > 0 Then
                Util.GetPredefinedScripts().Keys.CopyTo(names, 1)
            End If

            Return New StandardValuesCollection(names)
        End Function
    End Class
#End Region

#Region "Property Enabled"
    Private Const DefaultEnabled As Boolean = True
    Private enabledValue As Boolean = PSScriptSettings.DefaultEnabled

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

    'Private Function ShouldSerializeEnabled() As Boolean
    '    Return Me.enabledValue <> True 'PSScriptSettings.DefaultEnabled
    'End Function
#End Region

    Public Overrides Function ToString() As String
        Return Me.nameValue
    End Function



End Class