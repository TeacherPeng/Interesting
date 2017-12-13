Imports System.Text

Namespace DataBindingLab
	Friend Class SpecialFeaturesConverter
		Implements IMultiValueConverter
		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
			If values Is Nothing OrElse values.Length < 2 Then
				Return False
			End If
			If values(0) Is DependencyProperty.UnsetValue Then
				Return False
			End If
			If values(1) Is DependencyProperty.UnsetValue Then
				Return False
			End If
			Dim rating As Integer = CInt(Fix(values(0)))
			Dim [date] As Date = CDate(values(1))

			' if the user has a good rating (10+) and has been a member for more than a year, special features are available
			If (rating >= 10) AndAlso ([date].Date < (Date.Now.Date - New TimeSpan(365, 0, 0, 0))) Then
				Return True
			End If
			Return False
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Return New Object(1) { Binding.DoNothing, Binding.DoNothing }
		End Function
	End Class
End Namespace
