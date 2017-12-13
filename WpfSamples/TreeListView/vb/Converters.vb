Imports System.Globalization


Namespace TreeListViewSample
	''' <summary>
	''' Convert Level to left margin
	''' Pass a prarameter if you want a unit length other than 19.0.
	''' </summary>
	Public Class LevelToIndentConverter
		Implements IValueConverter
		Public Function Convert(ByVal o As Object, ByVal type As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
			Return New Thickness(CInt(Fix(o)) * c_IndentSize, 0, 0, 0)
		End Function

		Public Function ConvertBack(ByVal o As Object, ByVal type As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
			Throw New NotSupportedException()
		End Function

		Private Const c_IndentSize As Double = 19.0
	End Class
End Namespace
