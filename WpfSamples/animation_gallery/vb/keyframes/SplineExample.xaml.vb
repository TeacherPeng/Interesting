' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Animation

Namespace Microsoft.Samples.Animation

	Partial Public Class SplineExample
		Inherits Page
		Private controlPoint1 As New Point(0,100)
		Private controlPoint2 As New Point(0,100)

		Private Sub OnSliderChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)



			' Retrieve the name of slider.
			Dim name As String = (CType(sender, Slider)).Name

			Dim args As RoutedPropertyChangedEventArgs(Of Double) = TryCast(e, RoutedPropertyChangedEventArgs(Of Double))

			Select Case name
				Case "SliderControlPoint1X"
					mySplineKeyFrame.KeySpline.ControlPoint1 = New Point(CDbl(args.NewValue), mySplineKeyFrame.KeySpline.ControlPoint1.Y)
					controlPoint1.X = 100 * CDbl(args.NewValue)
				Case "SliderControlPoint1Y"
					mySplineKeyFrame.KeySpline.ControlPoint1 = New Point(mySplineKeyFrame.KeySpline.ControlPoint1.X, CDbl(args.NewValue))
					controlPoint1.Y = 100 - (100 * CDbl(args.NewValue))
				Case "SliderControlPoint2X"
					mySplineKeyFrame.KeySpline.ControlPoint2 = New Point(CDbl(args.NewValue), mySplineKeyFrame.KeySpline.ControlPoint2.Y)
					controlPoint2.X = 100 * CDbl(args.NewValue)
				Case "SliderControlPoint2Y"
					mySplineKeyFrame.KeySpline.ControlPoint2 = New Point(mySplineKeyFrame.KeySpline.ControlPoint2.X, CDbl(args.NewValue))
					controlPoint2.Y = 100 - (100 * CDbl(args.NewValue))


			End Select


			' Update the animations and illustrations.
			myVector3DSplineKeyFrame.KeySpline.ControlPoint1 = mySplineKeyFrame.KeySpline.ControlPoint1
			myVector3DSplineKeyFrame.KeySpline.ControlPoint2 = mySplineKeyFrame.KeySpline.ControlPoint2


			SplineIllustrationSegment.Point1 = controlPoint1
			SplineIllustrationSegment.Point2 = controlPoint2
			SplineControlPoint1Marker.Center = controlPoint1
			SplineControlPoint2Marker.Center = controlPoint2

			keySplineText.Text = "KeySpline=""" & mySplineKeyFrame.KeySpline.ControlPoint1.X.ToString("N") & "," & mySplineKeyFrame.KeySpline.ControlPoint1.Y.ToString("N") & " " & mySplineKeyFrame.KeySpline.ControlPoint2.X.ToString("N") & "," & mySplineKeyFrame.KeySpline.ControlPoint2.Y.ToString("N") & """"

		   ' Determine the storyboard's current time.
		   Dim oldTime? As TimeSpan = CType(ExampleStoryboard.GetCurrentTime(Me), TimeSpan)
		   If oldTime Is Nothing Then
			oldTime = TimeSpan.FromSeconds(0)
		   End If

		   ' Generate new clocks for the animations by calling 
		   ' the Begin method.
		   ExampleStoryboard.Begin(Me, True)

		   ' Because the storyboard was reset, advance it to its previous
		   ' position using the Seek method.
		   ExampleStoryboard.Seek(Me, CType(oldTime, TimeSpan), TimeSeekOrigin.BeginTime)
		End Sub

	End Class
End Namespace