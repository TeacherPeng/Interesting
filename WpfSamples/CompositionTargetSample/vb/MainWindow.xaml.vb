' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text

Namespace CompositionTargetSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private _pt As Point
		Private _stopwatch As New Stopwatch()
		Private _frameCounter As Double = 0

		Public Sub New()
			InitializeComponent()
			' Add an event handler to update canvas background color just before it is rendered.
			AddHandler CompositionTarget.Rendering, AddressOf UpdateColor
		End Sub

		' Called just before frame is rendered to allow custom drawing.
		Protected Sub UpdateColor(ByVal sender As Object, ByVal e As EventArgs)

            If _frameCounter = 0 Then
                ' Starting timing.
                _stopwatch.Start()
            End If

            _frameCounter = _frameCounter + 1

			' Determine frame rate in fps (frames per second).
			Dim frameRate As Long = CLng(Fix(_frameCounter / Me._stopwatch.Elapsed.TotalSeconds))
			If frameRate > 0 Then
				' Update elapsed time, number of frames, and frame rate.
				myStopwatchLabel.Content = _stopwatch.Elapsed.ToString()
				myFrameCounterLabel.Content = _frameCounter.ToString()
				myFrameRateLabel.Content = frameRate.ToString()
			End If

			' Update the background of the canvas by converting MouseMove info to RGB info.
			Dim redColor As Byte = CByte(_pt.X / 3.0)
			Dim blueColor As Byte = CByte(_pt.Y / 2.0)
			myCanvas.Background = New SolidColorBrush(Color.FromRgb(redColor, &H0, blueColor))
		End Sub

		Public Sub MouseMoveHandler(ByVal sender As Object, ByVal e As MouseEventArgs)
			' Retreive the coordinates of the mouse button event.
			_pt = e.GetPosition(CType(sender, UIElement))
		End Sub
	End Class
End Namespace