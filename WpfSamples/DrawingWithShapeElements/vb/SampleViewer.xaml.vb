' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text

Namespace DrawingWithShapeElements
	''' <summary>
	''' Interaction logic for SampleViewer.xaml
	''' </summary>
	Partial Public Class SampleViewer
		Inherits Window

		Private navigationArray() As Page

		Public Sub New()

			navigationArray = New Page(10){}
			navigationArray(0) = New ShapeTypes()
			navigationArray(1) = New LineExample()
			navigationArray(2) = New RectangleExample()
			navigationArray(3) = New EllipseExample()
			navigationArray(4) = New PolylineExample()
			navigationArray(5) = New PolygonExample()
			navigationArray(6) = New PathExample()
			navigationArray(7) = New FillRuleExample()
			navigationArray(8) = New LineCapsAndJoinsExample()
			navigationArray(9) = New MiterLimitExample()
			   navigationArray(10) = New StretchExample()
			InitializeComponent()

		End Sub

		Private Sub sampleSelected(ByVal sender As Object, ByVal args As RoutedEventArgs)

		End Sub

	End Class
End Namespace
