Namespace UsingVisualBrush


	Partial Public Class MagnifyingGlassExample
		Inherits Page

		Private Shared ReadOnly distanceFromMouse As Double = 5

       


		Private Sub updateMagnifyingGlass(ByVal sender As Object, ByVal args As MouseEventArgs)
			Mouse.SetCursor(Cursors.Cross)

			' Get the current position of the mouse pointer.
			Dim currentMousePosition As Point = args.GetPosition(Me)

			' Determine whether the magnifying glass should be shown to the
			' the left or right of the mouse pointer.
			If Me.ActualWidth - currentMousePosition.X > magnifyingGlassEllipse.Width + distanceFromMouse Then
				Canvas.SetLeft(magnifyingGlassEllipse, currentMousePosition.X + distanceFromMouse)
			Else
				Canvas.SetLeft(magnifyingGlassEllipse, currentMousePosition.X - distanceFromMouse - magnifyingGlassEllipse.Width)
			End If

			' Determine whether the magnifying glass should be shown 
			' above or below the mouse pointer.
			If Me.ActualHeight - currentMousePosition.Y > magnifyingGlassEllipse.Height + distanceFromMouse Then
				Canvas.SetTop(magnifyingGlassEllipse, currentMousePosition.Y + distanceFromMouse)
			Else
				Canvas.SetTop(magnifyingGlassEllipse, currentMousePosition.Y - distanceFromMouse - magnifyingGlassEllipse.Height)
			End If


			' Update the visual brush's Viewbox to magnify a 20 by 20 rectangle,
			' centered on the current mouse position.
			myVisualBrush.Viewbox = New Rect(currentMousePosition.X - 10, currentMousePosition.Y - 10, 20, 20)

		End Sub

	End Class
End Namespace