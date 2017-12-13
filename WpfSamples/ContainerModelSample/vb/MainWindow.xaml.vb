Imports System.Text
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Media3D


Namespace ContainerModelSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub

		' When the ContainerUIElement3D that has the two cubes as its children gets the
		' routed click event, spin the cubes in a 360 degree circle
		Private Sub ContainerMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			e.Handled = True

			' spin the cubes around
			Dim doubleAnimation As New DoubleAnimation(0, 360, New Duration(TimeSpan.FromSeconds(0.5)))
			containerRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimation)
		End Sub

		' Change the color of the first cube from Blue to Red, or vice versa, when it is clicked
		Private Sub Cube1MouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Equals(cube1Material.Brush, Brushes.Blue) Then
                cube1Material.Brush = Brushes.Red
            Else
                cube1Material.Brush = Brushes.Blue
            End If
		End Sub

		' Change the color of the second cube from Green to Yellow, or vice versa, when it is clicked
        Private Sub Cube2MouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Equals(cube2Material.Brush, Brushes.Green) Then
                cube2Material.Brush = Brushes.Yellow
            Else
                cube2Material.Brush = Brushes.Green
            End If
        End Sub
	End Class
End Namespace

