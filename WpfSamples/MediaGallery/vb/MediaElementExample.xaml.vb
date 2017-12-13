Namespace MediaGallery

	Partial Public Class MediaElementExample
		Inherits Page

		' Play the media.
		Private Sub OnMouseDownPlayMedia(ByVal sender As Object, ByVal args As MouseButtonEventArgs)

			' The Play method will begin the media if it is not currently active or 
			' resume media if it is paused. This has no effect if the media is
			' already running.
			myMediaElement.Play()

			' Initialize the MediaElement property values.
			InitializePropertyValues()

		End Sub

		' Pause the media.
		Private Sub OnMouseDownPauseMedia(ByVal sender As Object, ByVal args As MouseButtonEventArgs)

			' The Pause method pauses the media if it is currently running.
			' The Play method can be used to resume.
			myMediaElement.Pause()

		End Sub

		' Stop the media.
		Private Sub OnMouseDownStopMedia(ByVal sender As Object, ByVal args As MouseButtonEventArgs)

			' The Stop method stops and resets the media to be played from
			' the beginning.
			myMediaElement.Stop()

		End Sub

		' Change the volume of the media.
		Public Sub ChangeMediaVolume(ByVal sender As Object, ByVal args As RoutedPropertyChangedEventArgs(Of Double))
			myMediaElement.Volume = CDbl(volumeSlider.Value)
		End Sub


		' When the media opens, initialize the "Seek To" slider maximum value
		' to the total number of miliseconds in the length of the media clip.
		Public Sub Element_MediaOpened(ByVal sender As Object, ByVal e As EventArgs)
			timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds
		End Sub

		' Jump to different parts of the media (seek to). 
		Public Sub SeekToMediaPosition(ByVal sender As Object, ByVal args As RoutedPropertyChangedEventArgs(Of Double))
			Dim SliderValue As Integer = CInt(Fix(timelineSlider.Value))

			' Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
			' Create a TimeSpan with miliseconds equal to the slider value.
			Dim ts As New TimeSpan(0,0,0,0, SliderValue)
			myMediaElement.Position = ts
		End Sub

		Private Sub InitializePropertyValues()
			' Set the media's starting Volume to the current value of the
			' its slider control.
			myMediaElement.Volume = CDbl(volumeSlider.Value)
		End Sub

	End Class
End Namespace