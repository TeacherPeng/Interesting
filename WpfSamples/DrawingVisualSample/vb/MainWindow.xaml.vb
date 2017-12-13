' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Globalization

Namespace DrawingVisualSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As EventArgs)
			Dim visualHost As New MyVisualHost()
			MyCanvas.Children.Add(visualHost)
		End Sub
	End Class

	Public Class MyVisualHost
		Inherits FrameworkElement
		' Create a collection of child visual objects.
		Private _children As VisualCollection

		Public Sub New()
			_children = New VisualCollection(Me)
			_children.Add(CreateDrawingVisualRectangle())
			_children.Add(CreateDrawingVisualText())
			_children.Add(CreateDrawingVisualEllipses())

			' Add the event handler for MouseLeftButtonUp.
			AddHandler MouseLeftButtonUp, AddressOf MyVisualHost_MouseLeftButtonUp
		End Sub

		' Capture the mouse event and hit test the coordinate point value against
		' the child visual objects.
		Private Sub MyVisualHost_MouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			' Retreive the coordinates of the mouse button event.
			Dim pt As Point = e.GetPosition(CType(sender, UIElement))

			' Initiate the hit test by setting up a hit test result callback method.
			VisualTreeHelper.HitTest(Me, Nothing, New HitTestResultCallback(AddressOf myCallback), New PointHitTestParameters(pt))
		End Sub

		' If a child visual object is hit, toggle its opacity to visually indicate a hit.
		Public Function myCallback(ByVal result As HitTestResult) As HitTestResultBehavior
			If result.VisualHit.GetType() Is GetType(DrawingVisual) Then
				If (CType(result.VisualHit, DrawingVisual)).Opacity = 1.0 Then
					CType(result.VisualHit, DrawingVisual).Opacity = 0.4
				Else
					CType(result.VisualHit, DrawingVisual).Opacity = 1.0
				End If
			End If

			' Stop the hit test enumeration of objects in the visual tree.
			Return HitTestResultBehavior.Stop
		End Function

		' Create a DrawingVisual that contains a rectangle.
		Private Function CreateDrawingVisualRectangle() As DrawingVisual
			Dim drawingVisual As New DrawingVisual()

			' Retrieve the DrawingContext in order to create new drawing content.
			Dim drawingContext As DrawingContext = drawingVisual.RenderOpen()

			' Create a rectangle and draw it in the DrawingContext.
			Dim rect As New Rect(New Point(160, 100), New Size(320, 80))
			drawingContext.DrawRectangle(Brushes.LightBlue, CType(Nothing, Pen), rect)

			' Persist the drawing content.
			drawingContext.Close()

			Return drawingVisual
		End Function

		' Create a DrawingVisual that contains text.
		Private Function CreateDrawingVisualText() As DrawingVisual
			' Create an instance of a DrawingVisual.
			Dim drawingVisual As New DrawingVisual()

			' Retrieve the DrawingContext from the DrawingVisual.
			Dim drawingContext As DrawingContext = drawingVisual.RenderOpen()

			' Draw a formatted text string into the DrawingContext.
			drawingContext.DrawText(New FormattedText("Click Me!", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, New Typeface("Verdana"), 36, Brushes.Black), New Point(200, 116))

			' Close the DrawingContext to persist changes to the DrawingVisual.
			drawingContext.Close()

			Return drawingVisual
		End Function

		' Create a DrawingVisual that contains an ellipse.
		Private Function CreateDrawingVisualEllipses() As DrawingVisual
			Dim drawingVisual As New DrawingVisual()
			Dim drawingContext As DrawingContext = drawingVisual.RenderOpen()

			drawingContext.DrawEllipse(Brushes.Maroon, Nothing, New Point(430, 136), 20, 20)
			drawingContext.Close()

			Return drawingVisual
		End Function


		' Provide a required override for the VisualChildrenCount property.
		Protected Overrides ReadOnly Property VisualChildrenCount() As Integer
			Get
				Return _children.Count
			End Get
		End Property

		' Provide a required override for the GetVisualChild method.
		Protected Overrides Function GetVisualChild(ByVal index As Integer) As Visual
			If index < 0 OrElse index >= _children.Count Then
				Throw New ArgumentOutOfRangeException()
			End If

			Return _children(index)
		End Function
	End Class
End Namespace
