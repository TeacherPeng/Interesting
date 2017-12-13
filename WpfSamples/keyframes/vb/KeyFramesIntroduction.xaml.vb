' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


Namespace keyframes_markup


	Partial Public Class KeyFramesIntroduction
		Inherits Page

		Public Sub pageLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
			myVisualBrush.Visual = myImage

		End Sub

        Private Sub exampleCanvasLayoutUpdated(ByVal sender As Object, ByVal args As EventArgs)
            myVisualBrush.Viewbox = New Rect(Canvas.GetLeft(myRectangle), Canvas.GetTop(myRectangle), myRectangle.ActualWidth, myRectangle.ActualHeight)
        End Sub
	End Class
End Namespace