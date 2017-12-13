Imports System.Media
Imports System.Globalization
Imports System.ComponentModel
Imports System.Windows.Threading
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects


Namespace MediaGallery

	Partial Public Class MediaTimelineExample
		Implements INotifyPropertyChanged

		Public Sub New()
			Me.InitializeComponent()
		End Sub

		Private timer As DispatcherTimer

		Public Sub OnWindowLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)

			Dim b2 As New Binding()
			b2.Source = Me
			b2.Path = New PropertyPath("MyProp")

			' Bind to the slider and the textbox
			BindingOperations.SetBinding(ClockSlider, Slider.ValueProperty, b2)
			BindingOperations.SetBinding(PositionTextBox, TextBox.TextProperty, b2)

			timer = New DispatcherTimer()
			timer.Interval = New TimeSpan(0, 0, 0, 0, 100)

			' Every tick, the timer_Tick event handler is fired.
			AddHandler timer.Tick, AddressOf timer_Tick

		End Sub

		Private Sub timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			OnPropertyChanged("MyProp")
		End Sub


		Public Property MyProp() As Double
			Get
				If ClickedBSB.Storyboard.GetCurrentState() <> ClockState.Filling Then
					Return ClickedBSB.Storyboard.GetCurrentTime(DocumentRoot).Value.TotalSeconds

				Else
					Return 0
				End If
			End Get

			Set(ByVal value As Double)
				ClickedBSB.Storyboard.SeekAlignedToLastTick(DocumentRoot, New TimeSpan(CLng(Fix(Math.Floor(value * TimeSpan.TicksPerSecond)))), TimeSeekOrigin.BeginTime)
				OnPropertyChanged("MyProp")
			End Set
		End Property

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Private Sub OnPropertyChanged(ByVal name As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
		End Sub

		Public Sub OnMediaOpened(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If MediaElement1.Clock IsNot Nothing Then
				StatusBar.Text = MediaElement1.Clock.NaturalDuration.ToString()
				ClockSlider.Maximum = MediaElement1.Clock.NaturalDuration.TimeSpan.TotalSeconds + 10
			End If
			timer.Start()
		End Sub

		Private Sub mc_CurrentTimeInvalidated(ByVal sender As Object, ByVal e As EventArgs)
			StatusBar.Text = "CurrentStateInvalidated"
		End Sub


		Public Function Convert(ByVal o As Object, ByVal type As Type, ByVal param As Object, ByVal cul As CultureInfo) As Object
			Dim currPosition As TimeSpan = CType(o, TimeSpan)
			Dim NumMSecs As Double = currPosition.Milliseconds

			Dim NewValue As Double = (NumMSecs / MediaElement1.Clock.NaturalDuration.TimeSpan.TotalMilliseconds) * ClockSlider.Maximum
			Return NewValue
		End Function

		Public Function ConvertBack(ByVal o As Object, ByVal type As Type, ByVal param As Object, ByVal cul As CultureInfo) As Object
			Return Nothing
		End Function
	End Class

	Public Class PositionConverter
		Implements IValueConverter
		Public Function Convert(ByVal o As Object, ByVal type As Type, ByVal param As Object, ByVal cul As CultureInfo) As Object Implements IValueConverter.Convert
			Dim currPosition As TimeSpan = CType(o, TimeSpan)
			Return currPosition.Seconds
		End Function

		Public Function ConvertBack(ByVal o As Object, ByVal type As Type, ByVal param As Object, ByVal cul As CultureInfo) As Object Implements IValueConverter.ConvertBack
			Return Nothing
		End Function
	End Class

End Namespace