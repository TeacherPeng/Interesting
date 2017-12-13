Imports System.Text

Namespace PopupPlacement
	''' <summary>
	''' Interaction logic for NumericUpDownControl.xaml
	''' </summary>

	Partial Public Class NumericUpDownControl
		Inherits UserControl
		''' <summary>
		''' Initializes a new instance of the NumericUpDownControl.
		''' </summary>
		Public Sub New()
			InitializeComponent()

			UpdateTextBlock()
		End Sub

		''' <summary>
		''' Identifies the DecreaseButtonContent property.
		''' </summary>
		Public Shared ReadOnly DecreaseButtonContentProperty As DependencyProperty = DependencyProperty.Register("DecreaseButtonContent", GetType(Object), GetType(NumericUpDownControl), New PropertyMetadata(New PropertyChangedCallback(AddressOf OnDecreaseTextChanged)))


		Private Shared Sub OnDecreaseTextChanged(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
			Dim control As NumericUpDownControl = CType(obj, NumericUpDownControl)
			control.downButton.Content = args.NewValue


		End Sub

		''' <summary>
		''' Gets or sets the content in the Button that
		''' Decreases Value.
		''' </summary>
		Public Property DecreaseButtonContent() As Object
			Get
				Return GetValue(DecreaseButtonContentProperty)
			End Get
			Set(ByVal value As Object)
				SetValue(DecreaseButtonContentProperty, value)
			End Set
		End Property



		'//////////////////////////////////////////////////////////////
		''' <summary>
		''' Identifies the IncreaseButtonContent property.
		''' </summary>
		Public Shared ReadOnly IncreaseButtonContentProperty As DependencyProperty = DependencyProperty.Register("IncreaseButtonContent", GetType(Object), GetType(NumericUpDownControl), New PropertyMetadata(New PropertyChangedCallback(AddressOf OnIncreaseTextChanged)))


		Private Shared Sub OnIncreaseTextChanged(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
			Dim control As NumericUpDownControl = CType(obj, NumericUpDownControl)
			control.upButton.Content = args.NewValue


		End Sub

		''' <summary>
		''' Gets or sets the content in the Button that
		''' increases Value.
		''' </summary>
		Public Property IncreaseButtonContent() As Object
			Get
				Return GetValue(IncreaseButtonContentProperty)
			End Get
			Set(ByVal value As Object)
				SetValue(IncreaseButtonContentProperty, value)
			End Set
		End Property


		''' <summary>
		''' Gets or sets the value assigned to the control.
		''' </summary>
		Public Property Value() As Decimal
			Get
				Return CDec(GetValue(ValueProperty))
			End Get
			Set(ByVal value As Decimal)
				SetValue(ValueProperty, value)
			End Set
		End Property

		''' <summary>
		''' Identifies the Value dependency property.
		''' </summary>
		Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Decimal), GetType(NumericUpDownControl), New FrameworkPropertyMetadata(New PropertyChangedCallback(AddressOf OnValueChanged)))

		Private Shared Sub OnValueChanged(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
			Dim control As NumericUpDownControl = CType(obj, NumericUpDownControl)
			control.UpdateTextBlock()

			Dim e As New RoutedPropertyChangedEventArgs(Of Decimal)(CDec(args.OldValue), CDec(args.NewValue), ValueChangedEvent)
			control.OnValueChanged(e)
		End Sub

		''' <summary>
		''' Identifies the ValueChanged routed event.
		''' </summary>
		Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of Decimal)), GetType(NumericUpDownControl))

		''' <summary>
		''' Occurs when the Value property changes.
		''' </summary>
		Public Custom Event ValueChanged As RoutedPropertyChangedEventHandler(Of Decimal)
			AddHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Decimal))
				MyBase.AddHandler(ValueChangedEvent, value)
			End AddHandler
			RemoveHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Decimal))
				MyBase.RemoveHandler(ValueChangedEvent, value)
			End RemoveHandler
			RaiseEvent(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal))
			End RaiseEvent
		End Event

		''' <summary>
		''' Raises the ValueChanged event.
		''' </summary>
		''' <param name="args">Arguments associated with the ValueChanged event.</param>
		Protected Overridable Sub OnValueChanged(ByVal args As RoutedPropertyChangedEventArgs(Of Decimal))
			MyBase.RaiseEvent(args)
		End Sub

		Private Sub upButton_Click(ByVal sender As Object, ByVal e As EventArgs)
			Value += 1
		End Sub

		Private Sub downButton_Click(ByVal sender As Object, ByVal e As EventArgs)
			Value -= 1
		End Sub

		Private Sub UpdateTextBlock()
			valueText.Text = Value.ToString()
		End Sub

	End Class
End Namespace