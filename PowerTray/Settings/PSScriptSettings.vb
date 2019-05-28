'https://social.msdn.microsoft.com/Forums/vstudio/en-US/985b3727-c96c-4d98-8908-56ca402df0f8/propertygrid-with-readonly-properties-And-uitypeeditor-set-Not-grey?forum=winforms

'https://social.msdn.microsoft.com/Forums/windows/en-US/07ad29f2-3040-4f1d-81c6-d55e0522afe7/property-grid-with-file-path?forum=winforms

Imports System.ComponentModel
Imports System.Drawing.Design

Public Class PSScriptSettings
    Public Shared Function CreateInstance() As PSScriptSettings
        Return New PSScriptSettings()
    End Function

    Private Sub New()
        Me.nameValue = "Script name"
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

#Region "Property Label"
    Private labelValue As String = String.Empty

    <System.ComponentModel.Category(PSScriptSettings.LayoutCategory)>
    <System.ComponentModel.DisplayName("Label text")>
    <System.ComponentModel.DefaultValue("")>
    Public Property Label() As String
        Get
            Return Me.labelValue
        End Get
        Set(ByVal value As String)
            Me.labelValue = value
        End Set
    End Property
#End Region

#Region "Property LabelPosition"
    Public Enum LabelPositions
        Left
        Right
        Up
        Down
    End Enum

    Private Const DefaultLabelPosition As LabelPositions = LabelPositions.Left
    Private labelPositionValue As LabelPositions = PSScriptSettings.DefaultLabelPosition

    <System.ComponentModel.Category(PSScriptSettings.LayoutCategory)>
    <System.ComponentModel.DisplayName("Label position")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultLabelPosition)>
    Public Property LabelPosition() As PSScriptSettings.LabelPositions
        Get
            Return Me.labelPositionValue
        End Get
        Set(ByVal value As PSScriptSettings.LabelPositions)
            Me.labelPositionValue = value
        End Set
    End Property
#End Region

#Region "Property Source"
    Public Enum Sources As Integer
        Text
        File
        <System.ComponentModel.Description("Predefined script")>
        PredefinedScript
    End Enum

    Private Const DefaultSource As PSScriptSettings.Sources = Sources.Text
    Private sourceInternal As PSScriptSettings.Sources = PSScriptSettings.DefaultSource

    <System.ComponentModel.RefreshProperties(RefreshProperties.All)>
    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultSource)>
    Public Property Source() As PSScriptSettings.Sources
        Get
            'Set ReadOnly attribute on property Text
            Util.SetReadOnlyAttribute(GetType(PSScriptSettings), "Text", Not Me.sourceInternal = Sources.Text)

            'Set ReadOnly attribute on property FilePath
            Util.SetReadOnlyAttribute(GetType(PSScriptSettings), "FilePath", Not Me.sourceInternal = Sources.File)

            'Set ReadOnly attribute on property PredefinedScriptName
            Util.SetReadOnlyAttribute(GetType(PSScriptSettings), "PredefinedScriptName", Not Me.sourceInternal = Sources.PredefinedScript)

            Return Me.sourceInternal
        End Get
        Set(ByVal value As PSScriptSettings.Sources)
            Me.sourceInternal = value

            Select Case Me.sourceInternal
                Case Sources.Text
                    Me.ResetFilePath()
                    Me.ResetPredefinedScriptName()
                Case Sources.File
                    Me.ResetText()
                    Me.ResetPredefinedScriptName()
                Case Sources.PredefinedScript
                    Me.ResetText()
                    Me.ResetFilePath()
            End Select
        End Set
    End Property
#End Region

#Region "Property Text"
    Private textInternalValue As String = String.Empty

    <System.ComponentModel.Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))>
    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Source text")>
    <System.ComponentModel.DefaultValue("")>
    <System.ComponentModel.ReadOnly(True)>
    Public Property Text() As String
        Get
            'Set ReadOnly attribute
            'Util.SetReadOnlyAttribute(Me.GetType(), "Text", Not (Me.Source = Sources.Text))


            Return Me.textInternalValue
        End Get
        Set(ByVal value As String)
            Me.textInternalValue = value
            If String.IsNullOrWhiteSpace(Me.textInternalValue) Then
                Me.textInternalValue = String.Empty
            End If

            Me.ResetFilePath()
            Me.ResetPredefinedScriptName()
        End Set
    End Property

    Public Sub ResetText()
        Me.textInternalValue = String.Empty
    End Sub
#End Region

#Region "Property FilePath"
    Private filePathInternalValue As String = String.Empty

    <System.ComponentModel.Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))>
    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Source file path")>
    <System.ComponentModel.DefaultValue("")>
    <System.ComponentModel.ReadOnly(True)>
    Public Property FilePath() As String
        Get
            ''Set ReadOnly attribute
            'Util.SetReadOnlyAttribute(Me.GetType(), "FilePath", Not (Me.Source = Sources.File))

            Return Me.filePathInternalValue
        End Get
        Set(ByVal value As String)
            Me.filePathInternalValue = value
            If String.IsNullOrWhiteSpace(Me.filePathInternalValue) Then
                Me.filePathInternalValue = String.Empty
            End If

            Me.ResetText()
            Me.ResetPredefinedScriptName()
        End Set
    End Property

    Public Sub ResetFilePath()
        Me.filePathInternalValue = String.Empty
    End Sub
#End Region

#Region "Property PredefinedScriptName"
    Private predefinedScriptNameInternalValue As String = String.Empty

    <System.ComponentModel.TypeConverter(GetType(PredefinedScriptNameConverter))>
    <System.ComponentModel.Category(PSScriptSettings.SourceCategory)>
    <System.ComponentModel.DisplayName("Source predefined script")>
    <System.ComponentModel.DefaultValue("")>
    <System.ComponentModel.ReadOnly(True)>
    Public Property PredefinedScriptName() As String
        Get
            ''Set ReadOnly attribute
            'Util.SetReadOnlyAttribute(Me.GetType(), "PredefinedScriptName", Not (Me.Source = Sources.PredefinedScript))

            Return Me.predefinedScriptNameInternalValue
        End Get
        Set(ByVal value As String)
            Me.predefinedScriptNameInternalValue = value
            If String.IsNullOrWhiteSpace(Me.predefinedScriptNameInternalValue) Then
                Me.predefinedScriptNameInternalValue = String.Empty
            End If

            Me.ResetText()
            Me.ResetFilePath()
        End Set
    End Property

    Public Sub ResetPredefinedScriptName()
        Me.predefinedScriptNameInternalValue = String.Empty
    End Sub

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

#Region "Property ExecutionMode"
    Public Enum ExecutionModes As Integer
        OnStartupOnly
        OnOpen
        OnRefreshInterval
        OnManualStart
    End Enum

    Private Const DefaultExecutionMode As PSScriptSettings.ExecutionModes = ExecutionModes.OnRefreshInterval

    Private executionModeInternal As PSScriptSettings.ExecutionModes = PSScriptSettings.DefaultExecutionMode

    <System.ComponentModel.RefreshProperties(RefreshProperties.All)>
    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Execution mode")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultExecutionMode)>
    Public Property ExecutionMode() As PSScriptSettings.ExecutionModes
        Get
            'Set ReadOnly attribute on property ExecutionInterval
            Util.SetReadOnlyAttribute(GetType(PSScriptSettings), "ExecutionInterval", Not Me.executionModeInternal = ExecutionModes.OnRefreshInterval)

            'Set ReadOnly attribute on property ReExecuteOnlyWhenVisible
            Util.SetReadOnlyAttribute(GetType(PSScriptSettings), "ReExecuteOnlyWhenVisible", Not Me.executionModeInternal = ExecutionModes.OnRefreshInterval)

            Return Me.executionModeInternal
        End Get
        Set(ByVal value As PSScriptSettings.ExecutionModes)
            Me.executionModeInternal = value
        End Set
    End Property
#End Region

#Region "Property ExecutionInterval"
    Private Shared ReadOnly DefaultExecutionInterval As Integer? = Nothing
    Private executionIntervalValue As Integer? = PSScriptSettings.DefaultExecutionInterval

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Execution interval")>
    <System.ComponentModel.Description("Execution interval in milliseconds (if null the application refresh interval will be used)")>
    <System.ComponentModel.ReadOnly(False)>
    Public Property ExecutionInterval As Integer?
        Get
            Return Me.executionIntervalValue
        End Get
        Set(value As Integer?)
            If value.HasValue AndAlso
                (value.Value < PowerTraySettings.MinimumRefreshInterval OrElse
                value.Value < PowerTraySettings.Default.RefreshInterval) Then
                Throw New System.ArgumentOutOfRangeException()
            End If
            Me.executionIntervalValue = value
        End Set
    End Property

    Private Function ShouldSerializeExecutionInterval() As Boolean
        Return Me.executionIntervalValue.HasValue
    End Function
#End Region

#Region "Property ReExecuteOnlyWhenVisible"
    Private Const DefaultReExecuteOnlyWhenVisible As Boolean = True
    Private reExecuteOnlyWhenVisibleValue As Boolean = PSScriptSettings.DefaultReExecuteOnlyWhenVisible

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Re-Execute only when visible")>
    <System.ComponentModel.Description("Re-Execute the script only when output is visible")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultReExecuteOnlyWhenVisible)>
    <System.ComponentModel.ReadOnly(False)>
    Public Property ReExecuteOnlyWhenVisible() As Boolean
        Get
            Return Me.reExecuteOnlyWhenVisibleValue
        End Get
        Set(ByVal value As Boolean)
            Me.reExecuteOnlyWhenVisibleValue = value
        End Set
    End Property
#End Region

#Region "Property Timeout"
    Private Const DefaultTimeout As Integer = 5000
    Private timeoutValue As Integer = PSScriptSettings.DefaultTimeout

    <System.ComponentModel.Category(PSScriptSettings.BehaviourCategory)>
    <System.ComponentModel.DisplayName("Timeout")>
    <System.ComponentModel.Description("Execution timeout in milliseconds")>
    <System.ComponentModel.DefaultValue(PSScriptSettings.DefaultTimeout)>
    Public Property Timeout As Integer
        Get
            Return Me.timeoutValue
        End Get
        Set(value As Integer)
            Me.timeoutValue = value
        End Set
    End Property
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
