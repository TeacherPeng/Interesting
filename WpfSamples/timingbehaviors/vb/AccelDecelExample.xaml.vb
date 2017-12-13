Imports System.Windows.Media.Animation


Namespace Animation_TimingBehaviors


	Partial Public Class AccelDecelExample
		Inherits Page

		Private Sub stateInvalidated(ByVal sender As Object, ByVal args As EventArgs)
			If sender IsNot Nothing Then
				elapsedTime.Clock = CType(sender, Clock)
			End If
		End Sub

	End Class

End Namespace
