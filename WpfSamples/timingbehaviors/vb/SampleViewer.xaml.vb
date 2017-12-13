Imports System.Text
Imports System.Windows.Media.Animation

Namespace Animation_TimingBehaviors
	''' <summary>
	''' Interaction logic for SampleViewer.xaml
	''' </summary>
	Partial Public Class SampleViewer
		Inherits Page
		Public Sub New()
			InitializeComponent()
		End Sub
	End Class

	Public Class ElapsedTimeControl
		Inherits Control

		Private theClock As Clock
		Private previousTime? As TimeSpan

		Public Sub New()

		End Sub

		Public Property Clock() As Clock
			Get
				Return theClock
			End Get
			Set(ByVal value As Clock)
				If theClock IsNot Nothing Then
					RemoveHandler theClock.CurrentTimeInvalidated, AddressOf onClockTimeInvalidated

				End If

				theClock = value

				If theClock IsNot Nothing Then
					AddHandler theClock.CurrentTimeInvalidated, AddressOf onClockTimeInvalidated

				End If

			End Set

		End Property

		Private Sub onClockTimeInvalidated(ByVal sender As Object, ByVal args As EventArgs)


			SetValue(CurrentTimeProperty, theClock.CurrentTime)

		End Sub


		Public Shared ReadOnly CurrentTimeProperty As DependencyProperty = DependencyProperty.Register("CurrentTime", GetType(TimeSpan?), GetType(ElapsedTimeControl), New FrameworkPropertyMetadata(CType(Nothing, TimeSpan?), New PropertyChangedCallback(AddressOf currentTime_Changed)))


		Private Shared Sub currentTime_Changed(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)

			CType(d, ElapsedTimeControl).SetValue(CurrentTimeAsStringProperty, args.NewValue.ToString())
		End Sub


		Public Shared ReadOnly CurrentTimeAsStringProperty As DependencyProperty = DependencyProperty.Register("CurrentTimeAsString", GetType(String), GetType(ElapsedTimeControl))

		Public Property CurrentTimeAsString() As String
			Get
				Return TryCast(Me.GetValue(CurrentTimeAsStringProperty), String)
			End Get
			Set(ByVal value As String)
				Me.SetValue(CurrentTimeAsStringProperty, value)
			End Set
		End Property


	End Class

End Namespace
