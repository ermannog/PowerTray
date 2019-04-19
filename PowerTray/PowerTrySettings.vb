'https://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
'https://www.codeproject.com/Articles/9517/PropertyGrid-and-Drop-Down-properties
'https://www.codeproject.com/Articles/57760/Exposing-Object-Methods-in-the-PropertyGrid-Comman

Imports System.ComponentModel
Imports System.Drawing.Design

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
Public Module PowerTrySettingsDefaultProperty
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

    Private Const GeneralCategory As String = "General"
    Private Const SourceCategory As String = "Source"
    Private Const LayoutCategory As String = "Layout"
    Private Const BehaviourCategory As String = "Behaviour"

#Region "Name"
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

#Region "Source"
    Public Enum Sources As Integer
        Text
        File
        Predefined
    End Enum

    Private Const DefaultSource As PSScriptSettings.Sources = Sources.Text
    Private sourceValue As PSScriptSettings.Sources = PSScriptSettings.DefaultSource

    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultSource)>
    Public Property Source() As PSScriptSettings.Sources
        Get
            Return Me.sourceValue
        End Get
        Set(ByVal value As PSScriptSettings.Sources)
            Me.sourceValue = value

            ' Set ReadOnly Attribute
            Const PredefinedScriptNamePropertyName = "PredefinedScriptName"
            Const TextPropertyName = "Text"
            Const FilePathPropertyName = "FilePath"
            Select Case Me.sourceValue
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

#Region "Text"
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

#Region "FilePath"
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

#Region "PredefinedScriptName"
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

    'Private Function ShouldSerializeEnabled() As Boolean
    '    Return Me.enabledValue <> True 'PSScriptSettings.DefaultEnabled
    'End Function
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