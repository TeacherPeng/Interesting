Imports System.Windows.Media.Animation

Namespace Transforms


	Partial Public Class InteractiveMatrixTransformExample
		Inherits Page


		Private Sub applyButtonClicked(ByVal sender As Object, ByVal args As EventArgs)
			updateMatrixTransform()

		End Sub


		Private Sub updateMatrixTransform()

			Dim myMatrix As New Matrix()

			myMatrix.M11 = Double.Parse(M11TextBox.Text)
			myMatrix.M12 = Double.Parse(M12TextBox.Text)
			myMatrix.M21 = Double.Parse(M21TextBox.Text)
			myMatrix.M22 = Double.Parse(M22TextBox.Text)
			myMatrix.OffsetX = Double.Parse(OffsetXTextBox.Text)
			myMatrix.OffsetY = Double.Parse(OffsetYTextBox.Text)

			myMatrixTransform.Matrix = myMatrix

		End Sub

	End Class
End Namespace