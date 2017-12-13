Imports System.Globalization

Namespace DialogBoxSample
	Public Class MarginValidationRule
		Inherits ValidationRule
'INSTANT VB NOTE: The variable minMargin was renamed since Visual Basic does not allow class members with the same name:
		Private minMargin_Renamed As Double
'INSTANT VB NOTE: The variable maxMargin was renamed since Visual Basic does not allow class members with the same name:
		Private maxMargin_Renamed As Double

		Public Property MinMargin() As Double
			Get
				Return Me.minMargin_Renamed
			End Get
			Set(ByVal value As Double)
				Me.minMargin_Renamed = value
			End Set
		End Property

		Public Property MaxMargin() As Double
			Get
				Return Me.maxMargin_Renamed
			End Get
			Set(ByVal value As Double)
				Me.maxMargin_Renamed = value
			End Set
		End Property

		Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
			Dim margin As Double

			' Is a number?
			If Not Double.TryParse(CStr(value), margin) Then
				Return New ValidationResult(False, "Not a number.")
			End If

			' Is in range?
			If (margin < Me.minMargin_Renamed) OrElse (margin > Me.maxMargin_Renamed) Then
				Dim msg As String = String.Format("Margin must be between {0} and {1}.", Me.minMargin_Renamed, Me.maxMargin_Renamed)
				Return New ValidationResult(False, msg)
			End If

			' Number is valid
			Return New ValidationResult(True, Nothing)
		End Function
	End Class
End Namespace
