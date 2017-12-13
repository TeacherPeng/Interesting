Imports System.Text

Namespace RadialPanel
	Public Class app
		Inherits Application
'INSTANT VB NOTE: The variable mainWindow was renamed since Visual Basic does not allow class members with the same name:
        Private window1 As Window
        Private radialPanel1 As RadialPanel
        Private btn1 As Button
        Private btn2 As Button
        Private btn3 As Button
        Private btn4 As Button
        Private btn5 As Button
        Private border1 As Border

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            MyBase.OnStartup(e)
            CreateAndShowMainWindow()
        End Sub

        Private Sub CreateAndShowMainWindow()
            ' Create the application's main Window and Border 
            ' and instantiate a RadialPanel. RadialPanel is defined
            ' below

            border1 = New Border()
            border1.VerticalAlignment = VerticalAlignment.Top
            border1.HorizontalAlignment = HorizontalAlignment.Left
            border1.BorderThickness = New Thickness(1)
            border1.BorderBrush = Brushes.Purple

            radialPanel1 = New RadialPanel()
            radialPanel1.Width = 500
            radialPanel1.Height = 500
            radialPanel1.VerticalAlignment = VerticalAlignment.Stretch
            radialPanel1.HorizontalAlignment = HorizontalAlignment.Stretch

            ' Add the Button Elements

            btn1 = New Button()
            btn1.Background = Brushes.RoyalBlue
            btn1.Content = "Button 1"
            btn1.BorderBrush = Brushes.Black

            btn2 = New Button()
            btn2.Background = Brushes.Red
            btn2.Content = "Button 2"
            btn2.BorderBrush = Brushes.Black

            btn3 = New Button()
            btn3.Background = Brushes.Green
            btn3.Content = "Button 3"
            btn3.BorderBrush = Brushes.Black

            btn4 = New Button()
            btn4.Background = Brushes.Purple
            btn4.Content = "Button 4"
            btn4.BorderBrush = Brushes.Black

            btn5 = New Button()
            btn5.Background = Brushes.Yellow
            btn5.Content = "Button 5"
            btn5.BorderBrush = Brushes.Black

            ' Add the button elements defined above as
            ' children of the RadialPanel

            radialPanel1.Children.Add(btn1)
            radialPanel1.Children.Add(btn2)
            radialPanel1.Children.Add(btn3)
            radialPanel1.Children.Add(btn4)
            radialPanel1.Children.Add(btn5)

            ' Add the RadialPanel to the Border

            border1.Child = radialPanel1

            ' Add the RadialPanel as a Child of the 
            ' MainWindow and show the Window

            window1 = New Window()
            window1.Content = border1
            window1.Title = "Custom RadialPanel Sample"
            window1.Show()
        End Sub
	End Class
	Public Class RadialPanel
		Inherits Panel
		' This Panel lays its children out in a circle
		' keeping the angular distance from each child
		' equal; MeasureOverride is called before ArrangeOverride.

		Private _maxChildHeight, _perimeter, _radius, _adjustFactor As Double

		Protected Overrides Function MeasureOverride(ByVal availableSize As Size) As Size

			_perimeter = 0
			_maxChildHeight = 0

			' Find the tallest child and determine the perimeter
			' based on the width of all of the children after
			' measuring all of the them and letting them size
			' to content by passing Double.PositiveInfinity as
			' the available size.

			For Each uie As UIElement In Children

				uie.Measure(New Size(Double.PositiveInfinity, Double.PositiveInfinity))
				_perimeter += uie.DesiredSize.Width
				_maxChildHeight = Math.Max(_maxChildHeight, uie.DesiredSize.Height)
			Next uie

			' If the marginal angle is not 0, 90 or 180
			' then the adjustFactor is needed.

			If Children.Count > 2 AndAlso Children.Count <> 4 Then
				_adjustFactor = 10
			End If

			' Determine the radius of the circle layout and determine
			' the RadialPanel's DesiredSize.

			_radius = _perimeter / (2 * Math.PI) + _adjustFactor
			Dim _squareSize As Double = 2 * (_radius + _maxChildHeight)
			Return New Size(_squareSize, _squareSize)
		End Function

		' Perform arranging of children based on 
		' the final size.

		Protected Overrides Function ArrangeOverride(ByVal finalSize As Size) As Size
			' Necessary variables.
			Dim _currentOriginX As Double = 0, _currentOriginY As Double = 0, _currentAngle As Double = 0, _centerX As Double = 0, _centerY As Double = 0, _marginalAngle As Double = 0

			' During measure, an adjustFactor was added to the radius
			' to account for rotated children that might fall outside
			' of the desired size.  Now, during arrange, that extra
			' space isn't needed

			_radius -= _adjustFactor

			' Find center of the circle based on arrange size.
			' DesiredSize is not used because the Panel
			' is potentially being arranged across a larger
			' area from the default alignment values.

			_centerX = finalSize.Width \ 2
			_centerY = finalSize.Height \ 2

			' Determine the marginal angle, the angle between
			' each child on the circle.

			If Children.Count <> 0 Then
				_marginalAngle = 360 \ Children.Count
			End If

			For Each uie As UIElement In Children
				' Find origin from which to arrange 
				' each child of the RadialPanel (its top
				' left corner.)

				_currentOriginX = _centerX - uie.DesiredSize.Width \ 2
				_currentOriginY = _centerY - _radius - uie.DesiredSize.Height

				' Apply a rotation on each child around the center of the
				' RadialPanel.

				uie.RenderTransform = New RotateTransform(_currentAngle)
				uie.Arrange(New Rect(New Point(_currentOriginX, _currentOriginY), New Size(uie.DesiredSize.Width, uie.DesiredSize.Height)))

				' Increment the _currentAngle by the _marginalAngle
				' to advance the next child to the appropriate position.

				_currentAngle += _marginalAngle
			Next uie

			' In this case, the Panel is sizing to the space
			' given, so, return the finalSize which will be used
			' to set the ActualHeight & ActualWidth and for rendering.

			Return finalSize
		End Function
	End Class

	' Run the application
	Friend NotInheritable Class EntryClass
		Private Sub New()
		End Sub
        <STAThread()>
        Public Shared Sub Main()
            Dim app As New app()
            app.Run()
        End Sub
	End Class
End Namespace
