Imports System.Globalization

Namespace BindValidation
	Public Class AgeRangeRule
		Inherits ValidationRule
		Private _min As Integer
		Private _max As Integer

		Public Sub New()
		End Sub

		Public Property Min() As Integer
			Get
				Return _min
			End Get
			Set(ByVal value As Integer)
				_min = value
			End Set
		End Property

		Public Property Max() As Integer
			Get
				Return _max
			End Get
			Set(ByVal value As Integer)
				_max = value
			End Set
		End Property

		Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
			Dim age As Integer = 0

			Try
				If (CStr(value)).Length > 0 Then
					age = Int32.Parse(CType(value, String))
				End If
			Catch e As Exception
				Return New ValidationResult(False, "Illegal characters or " & e.Message)
			End Try

			If (age < Min) OrElse (age > Max) Then
				Return New ValidationResult(False, "Please enter an age in the range: " & Min & " - " & Max & ".")
			Else
				Return New ValidationResult(True, Nothing)
			End If
		End Function
	End Class
End Namespace
