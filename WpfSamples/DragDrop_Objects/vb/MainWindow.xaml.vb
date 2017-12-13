Imports System.Text
Imports System.Windows.Media.Animation

Namespace DragDrop_Objects
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Sub OnPageLoad(ByVal sender As Object, ByVal e As RoutedEventArgs)
			MyCanvas = New Canvas()

			Dim rect1 As New Rectangle()
			rect1.Width = 32
			rect1.Height = rect1.Width
			rect1.Fill = Brushes.Blue

			Canvas.SetTop(rect1, 8)
			Canvas.SetLeft(rect1, 8)


			Dim tb As New TextBox()
			tb.Text = "This is a TextBox. Drag and drop me"
			Canvas.SetTop(tb, 100)
			Canvas.SetLeft(tb, 100)

			MyCanvas.Children.Add(rect1)
			MyCanvas.Children.Add(tb)

			AddHandler MyCanvas.PreviewMouseLeftButtonDown, AddressOf MyCanvas_PreviewMouseLeftButtonDown
			AddHandler MyCanvas.PreviewMouseMove, AddressOf MyCanvas_PreviewMouseMove
			AddHandler MyCanvas.PreviewMouseLeftButtonUp, AddressOf MyCanvas_PreviewMouseLeftButtonUp
			AddHandler PreviewKeyDown, AddressOf window1_PreviewKeyDown

			myStackPanel.Children.Add(MyCanvas)

		End Sub


		Private Sub window1_PreviewKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape AndAlso isDraggingValue Then
                DragFinished(True)
            End If
        End Sub

        Private Sub MyCanvas_PreviewMouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If isDownValue Then
                DragFinished(False)
                e.Handled = True
            End If
        End Sub

        Private Sub DragFinished(ByVal cancelled As Boolean)
            Mouse.Capture(Nothing)
            If isDraggingValue Then
                AdornerLayer.GetAdornerLayer(overlayElementValue.AdornedElement).Remove(overlayElementValue)

                If cancelled = False Then
                    Canvas.SetTop(originalElementValue, originalTopValue + overlayElementValue.TopOffset)
                    Canvas.SetLeft(originalElementValue, originalLeftValue + overlayElementValue.LeftOffset)
                End If
                overlayElementValue = Nothing

            End If
            isDraggingValue = False
            isDownValue = False
        End Sub

        Private Sub MyCanvas_PreviewMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            If isDownValue Then
                If (isDraggingValue = False) AndAlso ((Math.Abs(e.GetPosition(MyCanvas).X - startPointValue.X) > SystemParameters.MinimumHorizontalDragDistance) OrElse (Math.Abs(e.GetPosition(MyCanvas).Y - startPointValue.Y) > SystemParameters.MinimumVerticalDragDistance)) Then
                    DragStarted()
                End If
                If isDraggingValue Then
                    DragMoved()
                End If
            End If
        End Sub

        Private Sub DragStarted()
            isDraggingValue = True
            originalLeftValue = Canvas.GetLeft(originalElementValue)
            originalTopValue = Canvas.GetTop(originalElementValue)

            overlayElementValue = New SimpleCircleAdorner(originalElementValue)
            Dim layer As AdornerLayer = AdornerLayer.GetAdornerLayer(originalElementValue)
            layer.Add(overlayElementValue)

        End Sub

        Private Sub DragMoved()
            Dim CurrentPosition As Point = Mouse.GetPosition(MyCanvas)

            overlayElementValue.LeftOffset = CurrentPosition.X - startPointValue.X
            overlayElementValue.TopOffset = CurrentPosition.Y - startPointValue.Y

        End Sub

        Private Sub MyCanvas_PreviewMouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If e.Source Is MyCanvas Then
            Else
                isDownValue = True
                startPointValue = e.GetPosition(MyCanvas)
                originalElementValue = TryCast(e.Source, UIElement)
                MyCanvas.CaptureMouse()
                e.Handled = True
            End If
        End Sub

        Private startPointValue As Point
        Private originalLeftValue As Double
        Private originalTopValue As Double
        Private isDownValue As Boolean
        Private isDraggingValue As Boolean
        Private originalElementValue As UIElement
        Private overlayElementValue As SimpleCircleAdorner

		Private MyCanvas As Canvas

	End Class

	' Adorners must subclass the abstract base class Adorner.
	Public Class SimpleCircleAdorner
		Inherits Adorner
		' Be sure to call the base class constructor.
		Public Sub New(ByVal adornedElement As UIElement)
			MyBase.New(adornedElement)
			Dim _brush As New VisualBrush(adornedElement)

            childValue = New Rectangle()
            childValue.Width = adornedElement.RenderSize.Width
            childValue.Height = adornedElement.RenderSize.Height


            Dim animation As New DoubleAnimation(0.3, 1, New Duration(TimeSpan.FromSeconds(1)))
            animation.AutoReverse = True
            animation.RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever
            _brush.BeginAnimation(Brush.OpacityProperty, animation)

            childValue.Fill = _brush
        End Sub

        ' A common way to implement an adorner's rendering behavior is to override the OnRender
        ' method, which is called by the layout subsystem as part of a rendering pass.
        Protected Overrides Sub OnRender(ByVal drawingContext As DrawingContext)
            ' Get a rectangle that represents the desired size of the rendered element
            ' after the rendering pass.  This will be used to draw at the corners of the 
            ' adorned element.
            Dim adornedElementRect As New Rect(Me.AdornedElement.DesiredSize)

            ' Some arbitrary drawing implements.
            Dim renderBrush As New SolidColorBrush(Colors.Green)
            renderBrush.Opacity = 0.2
            Dim renderPen As New Pen(New SolidColorBrush(Colors.Navy), 1.5)
            Dim renderRadius As Double = 5.0

            ' Just draw a circle at each corner.
            drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect)
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius)
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius)
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius)
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius)
        End Sub

        Protected Overrides Function MeasureOverride(ByVal constraint As Size) As Size
            childValue.Measure(constraint)
            Return childValue.DesiredSize
        End Function

        Protected Overrides Function ArrangeOverride(ByVal finalSize As Size) As Size
            childValue.Arrange(New Rect(finalSize))
            Return finalSize
        End Function

        Protected Overrides Function GetVisualChild(ByVal index As Integer) As Visual
            Return childValue
        End Function

        Protected Overrides ReadOnly Property VisualChildrenCount() As Integer
            Get
                Return 1
            End Get
        End Property

        Public Property LeftOffset() As Double
            Get
                Return leftOffsetValue
            End Get
            Set(ByVal value As Double)
                leftOffsetValue = value
                UpdatePosition()
            End Set
        End Property

        Public Property TopOffset() As Double
            Get
                Return topOffsetValue
            End Get
            Set(ByVal value As Double)
                topOffsetValue = value
                UpdatePosition()

            End Set
        End Property

        Private Sub UpdatePosition()
            Dim adornerLayer As AdornerLayer = TryCast(Me.Parent, AdornerLayer)
            If adornerLayer IsNot Nothing Then
                adornerLayer.Update(AdornedElement)
            End If
        End Sub

        Public Overrides Function GetDesiredTransform(ByVal transform As GeneralTransform) As GeneralTransform
            Dim result As New GeneralTransformGroup()
            result.Children.Add(MyBase.GetDesiredTransform(transform))
            result.Children.Add(New TranslateTransform(leftOffsetValue, topOffsetValue))
            Return result
        End Function

        Private childValue As Rectangle = Nothing
        Private leftOffsetValue As Double = 0
        Private topOffsetValue As Double = 0
	End Class
End Namespace

