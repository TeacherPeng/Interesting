Imports System.Text
Imports System.ComponentModel
Imports System.Globalization

Namespace DataBindingLab
	<ValueConversion(GetType(Date), GetType(String))>
	Public Class DateConverter
		Implements IValueConverter
		Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
			Dim [date] As Date = CDate(value)
			Return [date].ToShortDateString()
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
			Dim strValue As String = TryCast(value, String)
			Dim resultDateTime As Date
			If Date.TryParse(strValue, resultDateTime) Then
				Return resultDateTime
			End If
			Return DependencyProperty.UnsetValue
		End Function
	End Class
End Namespace
