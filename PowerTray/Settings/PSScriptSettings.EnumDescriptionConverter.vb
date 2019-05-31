Imports System.ComponentModel
Imports System.Drawing.Design

Partial Class PSScriptSettings

    Public Class EnumDescriptionConverter
        Inherits EnumConverter

        Public Sub New(ByVal type As Type)
            MyBase.New(type)
        End Sub

        Public Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
            Return destinationType = GetType(String)
        End Function

        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
            Dim field = EnumType.GetField([Enum].GetName(EnumType, value))
            Dim descriptionAttribute = DirectCast(Attribute.GetCustomAttribute(field, GetType(DescriptionAttribute)), DescriptionAttribute)

            If descriptionAttribute IsNot Nothing Then
                Return descriptionAttribute.Description
            Else
                Return value.ToString()
            End If
        End Function

        Public Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
            Return sourceType = GetType(String)
        End Function

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As Globalization.CultureInfo, ByVal value As Object) As Object
            For Each field In EnumType.GetFields()
                Dim descriptionAttribute = DirectCast(Attribute.GetCustomAttribute(field, GetType(DescriptionAttribute)), DescriptionAttribute)
                If descriptionAttribute IsNot Nothing AndAlso (value.ToString() = descriptionAttribute.Description) Then
                    Return [Enum].Parse(EnumType, field.Name)
                End If
            Next

            Return [Enum].Parse(EnumType, value.ToString())
        End Function

    End Class
End Class
