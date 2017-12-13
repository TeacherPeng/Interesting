Imports System.Text
Imports System.Globalization

Namespace DataBindingLab
	Friend Class FutureDateRule
		Inherits ValidationRule
		Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
			Dim [date] As Date
			Try
				[date] = Date.Parse(value.ToString())
			Catch e1 As FormatException
				Return New ValidationResult(False, "Value is not a valid date.")
			End Try
			If Date.Now.Date > [date] Then
				Return New ValidationResult(False, "Please enter a date in the future.")
			Else
				Return ValidationResult.ValidResult
			End If
		End Function
	End Class
End Namespace


