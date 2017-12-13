' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.Animation

Namespace GeometrySample
	''' <summary>
	''' Interaction logic for SampleViewer.xaml
	''' </summary>
	Partial Public Class SampleViewer
		Inherits Page
	   Private examples() As Page
	   Private sampleIndex As Integer

	   Public Sub New()
			InitializeComponent()
			examples = New Page(4){}

			examples(0) = New GeometryUsageExample()
			examples(1) = New ShapeGeometriesExample()
			examples(2) = New PathGeometryExample()
			examples(3) = New CombiningGeometriesExample()
			examples(4) = New GeometryAttributeSyntaxExample()

	   End Sub

	   Private Sub pageLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)

			Example1RadioButton.IsChecked = True
	   End Sub


	   Private Sub zoomOutStoryboardCompleted(ByVal sender As Object, ByVal args As EventArgs)

			mainFrame.Navigate(examples(sampleIndex))

	   End Sub

	   Private Sub frameContentRendered(ByVal sender As Object, ByVal args As EventArgs)

			Dim s As Storyboard = CType(Me.Resources("ZoomInStoryboard"), Storyboard)
			s.Begin(Me)
	   End Sub

	   Private Sub zoomInStoryboardCompleted(ByVal sender As Object, ByVal e As EventArgs)
			scrollViewerBorder.Visibility = Visibility.Visible

	   End Sub


	   Private Sub sampleSelected(ByVal sender As Object, ByVal args As RoutedEventArgs)


		 Dim points As New Point3DCollection()

		 Dim ratio As Double = myScrollViewer.ActualWidth / myScrollViewer.ActualHeight

		 points.Add(New Point3D(5, -5 * ratio, 0))
		 points.Add(New Point3D(5, 5 * ratio, 0))
		 points.Add(New Point3D(-5, 5 * ratio, 0))

		 points.Add(New Point3D(-5, 5 * ratio, 0))
		 points.Add(New Point3D(-5, -5 * ratio, 0))
		 points.Add(New Point3D(5, -5 * ratio, 0))

		 points.Add(New Point3D(-5, 5 * ratio, 0))
		 points.Add(New Point3D(-5, -5 * ratio, 0))
		 points.Add(New Point3D(5, -5 * ratio, 0))

		 points.Add(New Point3D(5, -5 * ratio, 0))
		 points.Add(New Point3D(5, 5 * ratio, 0))
		 points.Add(New Point3D(-5, 5 * ratio, 0))

		 myGeometry.Positions = points
		 myViewport3D.Width = 100
		 myViewport3D.Height = 100 * ratio

		 scrollViewerBorder.Visibility = Visibility.Hidden

		 Dim button As RadioButton = TryCast(sender, RadioButton)

		 If button IsNot Nothing Then

		   If button.Content.ToString() = "Geometry Usage" Then
			 sampleIndex = 0
		   ElseIf button.Content.ToString() = "Shape Geometries" Then
			sampleIndex = 1

		   ElseIf button.Content.ToString() = "PathGeometry" Then
			sampleIndex = 2

		   ElseIf button.Content.ToString() = "Combining Geometries Example" Then
			 sampleIndex = 3
		   ElseIf button.Content.ToString() = "Geometry Attribute Syntax Example" Then
			sampleIndex = 4
		   End If

		 End If
	   End Sub
	End Class
End Namespace
