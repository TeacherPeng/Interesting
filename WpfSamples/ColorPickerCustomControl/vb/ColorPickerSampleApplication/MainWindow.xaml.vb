Imports System.Text

Namespace ColorPickerSampleApplication
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
	   #Region "Dependency Property Fields"

		Public Shared ReadOnly SelectedShapeProperty As DependencyProperty = DependencyProperty.Register ("SelectedShape", GetType(Shape), GetType(MainWindow), New PropertyMetadata(Nothing, AddressOf selectedShape_Changed))

		Public Shared ReadOnly DrawingModeProperty As DependencyProperty = DependencyProperty.Register ("DrawingMode", GetType(DrawingMode), GetType(MainWindow), New PropertyMetadata(DrawingMode.Select))

		Public Shared ReadOnly FillColorProperty As DependencyProperty = DependencyProperty.Register ("FillColor", GetType(Color), GetType(MainWindow), New PropertyMetadata(Colors.LightGray))

		Public Shared ReadOnly StrokeColorProperty As DependencyProperty = DependencyProperty.Register ("StrokeColor", GetType(Color), GetType(MainWindow), New PropertyMetadata(Colors.Black))

		Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = DependencyProperty.Register ("StrokeThickness", GetType(Double), GetType(MainWindow), New PropertyMetadata(1.0, AddressOf strokeThickness_Changed))

		#End Region


		#Region "Public Properties"

		Public Property DrawingMode() As DrawingMode
			Get
				Return CType(GetValue(DrawingModeProperty), DrawingMode)
			End Get
			Set(ByVal value As DrawingMode)
				SetValue(DrawingModeProperty, value)
			End Set
		End Property


		Public Property FillColor() As Color
			Get
				Return CType(GetValue(FillColorProperty), Color)
			End Get
			Set(ByVal value As Color)
				SetValue(FillColorProperty, value)
			End Set
		End Property

		Public Property StrokeColor() As Color
			Get
				Return CType(GetValue(StrokeColorProperty), Color)
			End Get
			Set(ByVal value As Color)
				SetValue(StrokeColorProperty, value)
			End Set
		End Property

		Public Property StrokeThickness() As Double
			Get
				Return CDbl(GetValue(StrokeThicknessProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(StrokeThicknessProperty, value)
			End Set
		End Property

		#End Region




		Private Shared Sub selectedShape_Changed(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)

			Dim sViewer As MainWindow = CType(sender, MainWindow)
			sViewer.OnSelectedShapeChanged(CType(e.OldValue, Shape), CType(e.NewValue, Shape))

		End Sub

		Protected Sub OnSelectedShapeChanged(ByVal oldShape As Shape, ByVal newShape As Shape)
			If newShape IsNot Nothing Then
				FillColor = (CType(newShape.Fill, SolidColorBrush)).Color
				StrokeColor = (CType(newShape.Stroke, SolidColorBrush)).Color
				StrokeThickness = newShape.StrokeThickness
			End If
		End Sub

		Private Shared Sub strokeThickness_Changed(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)

			Dim sViewer As MainWindow = CType(sender, MainWindow)
			sViewer.OnStrokeThicknessChanged(CDbl(e.OldValue), CDbl(e.NewValue))

		End Sub

		Protected Sub OnStrokeThicknessChanged(ByVal oldThickness As Double, ByVal newThickness As Double)
			Dim currentShape As Shape = CType(GetValue(SelectedShapeProperty), Shape)
			If currentShape IsNot Nothing Then
				currentShape.StrokeThickness = newThickness
			End If
		End Sub


		Private Sub OnDrawingCanvasMouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			Dim clickPoint As Point = e.GetPosition(DrawingCanvas)
			If DrawingMode = DrawingMode.Select Then
				If TypeOf e.OriginalSource Is Shape Then
					Dim s As Shape = CType(e.OriginalSource, Shape)
					SetValue(SelectedShapeProperty, s)
					shapeClickPoint = e.GetPosition(s)
				Else
					SetValue(SelectedShapeProperty, Nothing)
				End If
			ElseIf DrawingMode = DrawingMode.DrawRectangle AndAlso e.LeftButton = MouseButtonState.Pressed Then
				newRectangle = New Rectangle()
				newRectangle.Stroke = New SolidColorBrush(StrokeColor)
				newRectangle.StrokeThickness = StrokeThickness
				newRectangle.Fill = New SolidColorBrush(FillColor)
				Canvas.SetLeft(newRectangle, clickPoint.X)
				Canvas.SetTop(newRectangle, clickPoint.Y)
				DrawingCanvas.Children.Add(newRectangle)
				SetValue(SelectedShapeProperty, newRectangle)
			ElseIf DrawingMode = DrawingMode.DrawEllipse AndAlso e.LeftButton = MouseButtonState.Pressed Then
				newEllipse = New Ellipse()
				newEllipse.Stroke = New SolidColorBrush(StrokeColor)
				newEllipse.StrokeThickness = StrokeThickness
				newEllipse.Fill = New SolidColorBrush(FillColor)
				Canvas.SetLeft(newEllipse, clickPoint.X)
				Canvas.SetTop(newEllipse, clickPoint.Y)
				DrawingCanvas.Children.Add(newEllipse)
				SetValue(SelectedShapeProperty, newEllipse)
			ElseIf DrawingMode = DrawingMode.DrawLine AndAlso e.LeftButton = MouseButtonState.Pressed Then
				newLine = New Line()
				newLine.Stroke = New SolidColorBrush(StrokeColor)
				newLine.Fill = New SolidColorBrush(FillColor)

				newLine.X1 = clickPoint.X
				newLine.Y1 = clickPoint.Y
				newLine.StrokeThickness = StrokeThickness
				DrawingCanvas.Children.Add(newLine)
				SetValue(SelectedShapeProperty, newLine)
			End If
		End Sub

		Private Sub OnDrawingCanvasMouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)

			If DrawingMode = DrawingMode.DrawRectangle Then
				newRectangle = Nothing

			ElseIf DrawingMode = DrawingMode.DrawEllipse Then
				newEllipse = Nothing

			ElseIf DrawingMode = DrawingMode.DrawLine Then
				newLine = Nothing

			End If
		End Sub

		Private Sub OnDrawingCanvasMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim dropPoint As Point = e.GetPosition(DrawingCanvas)
			If DrawingMode = DrawingMode.Select Then
				Dim s As Shape = CType(GetValue(SelectedShapeProperty), Shape)
				If s IsNot Nothing AndAlso e.LeftButton = MouseButtonState.Pressed Then

					Canvas.SetLeft(s, dropPoint.X - shapeClickPoint.X)
					Canvas.SetTop(s, dropPoint.Y - shapeClickPoint.Y)
					s.Effect = Nothing
				End If
			ElseIf DrawingMode = DrawingMode.DrawRectangle Then
				If newRectangle IsNot Nothing Then
					If dropPoint.X > Canvas.GetLeft(newRectangle) Then
						newRectangle.Width = dropPoint.X - Canvas.GetLeft(newRectangle)
					End If
					If dropPoint.Y > Canvas.GetTop(newRectangle) Then
						newRectangle.Height = dropPoint.Y - Canvas.GetTop(newRectangle)
					End If
				End If
			ElseIf DrawingMode = DrawingMode.DrawEllipse Then
				If newEllipse IsNot Nothing Then
					If dropPoint.X > Canvas.GetLeft(newEllipse) Then
						newEllipse.Width = dropPoint.X - Canvas.GetLeft(newEllipse)
					End If
					If dropPoint.Y > Canvas.GetTop(newEllipse) Then
						newEllipse.Height = dropPoint.Y - Canvas.GetTop(newEllipse)
					End If
				End If
			ElseIf DrawingMode = DrawingMode.DrawLine Then
				If newLine IsNot Nothing Then
					newLine.X2 = dropPoint.X
					newLine.Y2 = dropPoint.Y
				End If
			End If
		End Sub

		Private Sub OnDrawingCanvasKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)

			Dim s As Shape = CType(GetValue(SelectedShapeProperty), Shape)
			If e.Key = Key.Delete AndAlso s IsNot Nothing Then
				DrawingCanvas.Children.Remove(s)
				SetValue(SelectedShapeProperty, Nothing)
			End If
		End Sub


		Private Sub SetStroke(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim selectedShape As Shape = CType(GetValue(SelectedShapeProperty), Shape)



				Dim cPicker As New ColorPickerLib.ColorPickerDialog()

				cPicker.StartingColor = StrokeColor
				cPicker.Owner = Me

				Dim dialogResult? As Boolean = cPicker.ShowDialog()
				If dialogResult IsNot Nothing AndAlso CBool(dialogResult) = True Then

					If selectedShape IsNot Nothing Then
						selectedShape.Stroke = New SolidColorBrush(cPicker.SelectedColor)
					End If
					StrokeColor = cPicker.SelectedColor

				End If

		End Sub


		Private Sub SetFill(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim selectedShape As Shape = CType(GetValue(SelectedShapeProperty), Shape)

				Dim cPicker As New ColorPickerLib.ColorPickerDialog()
				cPicker.StartingColor = FillColor
				cPicker.Owner = Me

				Dim dialogResult? As Boolean = cPicker.ShowDialog()
				If dialogResult IsNot Nothing AndAlso CBool(dialogResult) = True Then

					If selectedShape IsNot Nothing Then
						selectedShape.Fill = New SolidColorBrush(cPicker.SelectedColor)
					End If
					FillColor = cPicker.SelectedColor

				End If

		End Sub

		Private Sub drawingModeChanged(ByVal sender As Object, ByVal e As EventArgs)

			Dim r As RadioButton = CType(sender, RadioButton)
			SetValue(DrawingModeProperty, System.Enum.Parse(GetType(DrawingMode), r.Name))

		End Sub


		Private Sub exitMenuItemClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Application.Current.Shutdown()
		End Sub



		#Region "Private Fields"
		Private shapeClickPoint As Point
		Private newRectangle As Rectangle
		Private newLine As Line
		Private newEllipse As Ellipse

		#End Region

	End Class

	Public Enum DrawingMode
		[Select]=0
		DrawLine=1
		DrawRectangle=2
		DrawEllipse=3

	End Enum
End Namespace
