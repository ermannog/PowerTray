'http://www.karmian.org/net-tips-tricks-examples-articles/enum-description-typeconverter

Public Class UtilDescriptionEnumConverter
    Inherits System.ComponentModel.EnumConverter
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Public Overrides Function CanConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, sourceType As Type) As Boolean
        Return sourceType Is GetType(String) OrElse System.ComponentModel.TypeDescriptor.GetConverter(GetType([Enum])).CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Return GetEnumValue(EnumType, DirectCast(value, String))
        End If
        If TypeOf value Is [Enum] Then
            Return GetEnumDescription(DirectCast(value, [Enum]))
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        Return If(TypeOf value Is [Enum] AndAlso destinationType Is GetType(String), GetEnumDescription(DirectCast(value, [Enum])), (If(TypeOf value Is String AndAlso destinationType Is GetType(String), GetEnumDescription(EnumType, DirectCast(value, String)), MyBase.ConvertTo(context, culture, value, destinationType))))
    End Function

    Public Shared Function GetEnumDescription(value As [Enum]) As String
        Dim fieldInfo = value.[GetType]().GetField(value.ToString())
        Dim attributes = DirectCast(fieldInfo.GetCustomAttributes(GetType(System.ComponentModel.DescriptionAttribute), False), System.ComponentModel.DescriptionAttribute())
        Return If((attributes.Length > 0), attributes(0).Description, value.ToString())
    End Function

    Public Shared Function GetEnumDescription(value As Type, name As String) As String
        Dim fieldInfo = value.GetField(name)
        Dim attributes = DirectCast(fieldInfo.GetCustomAttributes(GetType(System.ComponentModel.DescriptionAttribute), False), System.ComponentModel.DescriptionAttribute())
        Return If((attributes.Length > 0), attributes(0).Description, name)
    End Function

    Public Shared Function GetEnumValue(value As Type, description As String) As Object
        Dim fields = value.GetFields()
        For Each fieldInfo As System.Reflection.FieldInfo In fields
            Dim attributes = DirectCast(fieldInfo.GetCustomAttributes(GetType(System.ComponentModel.DescriptionAttribute), False), System.ComponentModel.DescriptionAttribute())
            If attributes.Length > 0 AndAlso attributes(0).Description = description Then
                Return fieldInfo.GetValue(fieldInfo.Name)
            End If
            If fieldInfo.Name = description Then
                Return fieldInfo.GetValue(fieldInfo.Name)
            End If
        Next
        Return description
    End Function
End Class
