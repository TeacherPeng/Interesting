Imports System.Text
Imports System.Windows.Controls.Primitives

Namespace Thumb_wcp
	''' <summary>
	''' Interaction logic for Pane1.xaml
	''' </summary>
	Partial Public Class Pane1
		Inherits Canvas
		Private Sub onDragDelta(ByVal sender As Object, ByVal e As DragDeltaEventArgs)
			'Move the Thumb to the mouse position during the drag operation
			Dim yadjust As Double = myCanvasStretch.Height + e.VerticalChange
			Dim xadjust As Double = myCanvasStretch.Width + e.HorizontalChange
			If (xadjust >= 0) AndAlso (yadjust >= 0) Then
				myCanvasStretch.Width = xadjust
				myCanvasStretch.Height = yadjust
				Canvas.SetLeft(myThumb, Canvas.GetLeft(myThumb) + e.HorizontalChange)
				Canvas.SetTop(myThumb, Canvas.GetTop(myThumb) + e.VerticalChange)
				changes.Text = "Size: " & myCanvasStretch.Width.ToString() & ", " & myCanvasStretch.Height.ToString()
			End If
		End Sub

		Private Sub onDragStarted(ByVal sender As Object, ByVal e As DragStartedEventArgs)
			myThumb.Background = Brushes.Orange
		End Sub

		Private Sub onDragCompleted(ByVal sender As Object, ByVal e As DragCompletedEventArgs)
			myThumb.Background = Brushes.Blue
		End Sub

	End Class
End Namespace