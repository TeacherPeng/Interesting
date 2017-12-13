Imports System.Text
Imports System.Windows.Controls.Primitives

Namespace PopupPlacement
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		' The Rect for PlacementRectangle.
		Private placementRect As New Rect(50, 150, 60, 90)

		Public Sub New()
			InitializeComponent()

			popup1.CustomPopupPlacementCallback = New CustomPopupPlacementCallback(AddressOf placePopup)
		End Sub

		' Provide to possible places for the Popup when Placement
		' is set to Custom.
		Public Function placePopup(ByVal popupSize As Size, ByVal targetSize As Size, ByVal offset As Point) As CustomPopupPlacement()
			Dim placement1 As New CustomPopupPlacement(New Point(-50, 100), PopupPrimaryAxis.Vertical)

			Dim placement2 As New CustomPopupPlacement(New Point(10, 20), PopupPrimaryAxis.Horizontal)

			Dim ttplaces() As CustomPopupPlacement = { placement1, placement2}
			Return ttplaces
		End Function

		' Set PlacementRectangle and show a Rectangle with the same
		' dimensions.
		Private Sub showPlacementRectangle(ByVal sender As Object, ByVal e As RoutedEventArgs)
			placementRectArea.Visibility = Visibility.Visible
			popup1.PlacementRectangle = placementRect
		End Sub

		' Clear PlacementRectangle and hide the Rectangle
		Private Sub hidePlacementRectangle(ByVal sender As Object, ByVal e As RoutedEventArgs)
			placementRectArea.Visibility = Visibility.Hidden
			popup1.PlacementRectangle = Rect.Empty
		End Sub

		' Set the Placement property of the Popup.
		Private Sub setPlacement(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim placementChoice As RadioButton = TryCast(e.Source, RadioButton)

			If placementChoice Is Nothing Then
				Return
			End If

			Select Case placementChoice.Name
				Case "placementAbsolute"
					popup1.Placement = PlacementMode.Absolute
				Case "placementAbsolutePoint"
					popup1.Placement = PlacementMode.AbsolutePoint
				Case "placementBottom"
					popup1.Placement = PlacementMode.Bottom
				Case "placementCenter"
					popup1.Placement = PlacementMode.Center
				Case "placementCustom"
					popup1.Placement = PlacementMode.Custom
				Case "placementLeft"
					popup1.Placement = PlacementMode.Left
				Case "placementMouse"
					popup1.Placement = PlacementMode.Mouse
				Case "placementMousePoint"
					popup1.Placement = PlacementMode.MousePoint
				Case "placementRelative"
					popup1.Placement = PlacementMode.Relative
				Case "placementRelativePoint"
					popup1.Placement = PlacementMode.RelativePoint
				Case "placementRight"
					popup1.Placement = PlacementMode.Right
				Case "placementTop"
					popup1.Placement = PlacementMode.Top
			End Select
		End Sub

		' Reset the offsets of the Popup.
		Private Sub resetOffsets_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			popup1.HorizontalOffset = 0
			popup1.VerticalOffset = 0
		End Sub

	End Class
End Namespace