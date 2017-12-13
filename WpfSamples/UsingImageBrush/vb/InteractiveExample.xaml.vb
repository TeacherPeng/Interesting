Imports System.Windows.Media.Animation

Namespace Microsoft.Samples.Graphics.UsingImageBrush
	''' <summary>
	''' Interaction logic for TextFillsExample.xaml
	''' </summary>

	Partial Public Class InteractiveExample
		Inherits Page
	  Private selectedButton As RadioButton = Nothing

			Private Sub InteractiveExampleLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)

		  loadInteractiveMenus()
		  MyDefaultImageButton.IsChecked = True

		End Sub


		' Initializes the image brush menu options.
		Private Sub loadInteractiveMenus()
			Dim values() As String


			values = System.Enum.GetNames(GetType(Stretch))
			For Each stretchMode As String In values
				stretchSelector.Items.Add(stretchMode)
			Next stretchMode
			stretchSelector.SelectedItem = myImageBrush.Stretch.ToString()

			values = System.Enum.GetNames(GetType(AlignmentX))
			For Each hAlign As String In values
				horizontalAlignmentSelector.Items.Add(hAlign)
			Next hAlign
			horizontalAlignmentSelector.SelectedItem = myImageBrush.AlignmentX.ToString()

			values = System.Enum.GetNames(GetType(AlignmentY))
			For Each vAlign As String In values
				verticalAlignmentSelector.Items.Add(vAlign)
			Next vAlign
			verticalAlignmentSelector.SelectedItem = myImageBrush.AlignmentY.ToString()

			values = System.Enum.GetNames(GetType(TileMode))
			For Each tileMode As String In values
				tileSelector.Items.Add(tileMode)
			Next tileMode
			tileSelector.SelectedItem = myImageBrush.TileMode.ToString()

			values = System.Enum.GetNames(GetType(BrushMappingMode))
			For Each mappingMode As String In values
				viewportUnitsSelector.Items.Add(mappingMode)
				viewboxUnitsSelector.Items.Add(mappingMode)
			Next mappingMode
			viewportUnitsSelector.SelectedItem = myImageBrush.ViewportUnits.ToString()
			viewboxUnitsSelector.SelectedItem = myImageBrush.ViewboxUnits.ToString()
			viewportEntry.Text = myImageBrush.Viewport.ToString()
			viewboxEntry.Text = myImageBrush.Viewbox.ToString()

		End Sub

		Private Sub setSelectedButton(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Me.selectedButton = TryCast(sender, RadioButton)
		End Sub

		' Applies the selected options to the image brush.
		Private Sub updateBrush(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Try

				myImageBrush.ImageSource = (TryCast(Me.selectedButton.Content, Image)).Source
				myImageBrush.Stretch = CType(System.Enum.Parse(GetType(Stretch), CStr(stretchSelector.SelectedItem)), Stretch)
				myImageBrush.AlignmentX = CType(System.Enum.Parse(GetType(AlignmentX), CStr(horizontalAlignmentSelector.SelectedItem)), AlignmentX)
				myImageBrush.AlignmentY = CType(System.Enum.Parse(GetType(AlignmentY), CStr(verticalAlignmentSelector.SelectedItem)), AlignmentY)
				myImageBrush.TileMode = CType(System.Enum.Parse(GetType(TileMode), CStr(tileSelector.SelectedItem)), TileMode)
				myImageBrush.ViewportUnits = CType(System.Enum.Parse(GetType(BrushMappingMode), CStr(viewportUnitsSelector.SelectedItem)), BrushMappingMode)
				myImageBrush.ViewboxUnits = CType(System.Enum.Parse(GetType(BrushMappingMode), CStr(viewboxUnitsSelector.SelectedItem)), BrushMappingMode)

				Dim myRectConverter As New RectConverter()
				Dim parseString As String
				parseString = viewportEntry.Text

				If parseString IsNot Nothing AndAlso parseString <> String.Empty Then
					myImageBrush.Viewport = CType(myRectConverter.ConvertFromString(parseString), Rect)
				Else
					myImageBrush.Viewport = Rect.Empty
					viewportEntry.Text = "Empty"
				End If

				parseString = viewboxEntry.Text

				If parseString IsNot Nothing AndAlso parseString <> String.Empty AndAlso parseString.ToLower() <> "(auto)" Then
					myImageBrush.Viewbox = CType(myRectConverter.ConvertFromString(parseString), Rect)
				Else
					viewboxEntry.Text = "Empty"
					myImageBrush.Viewbox = Rect.Empty
				End If


			Catch invalidOpEx As InvalidOperationException
				MessageBox.Show("Invalid Viewport or Viewbox. " & invalidOpEx.ToString())
			Catch formatEx As FormatException
				MessageBox.Show("Invalid Viewport or Viewbox. " & formatEx.ToString())
			End Try

		End Sub

	End Class
End Namespace