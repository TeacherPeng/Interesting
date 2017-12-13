Imports System.Text

Namespace Width_MinWidth_MaxWidth_CSharp
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private Sub changeWidth(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.Width = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualWidth is set to " & rect1.ActualWidth
			txt2.Text = "Width is set to " & rect1.Width
			txt3.Text = "MinWidth is set to " & rect1.MinWidth
			txt4.Text = "MaxWidth is set to " & rect1.MaxWidth
		End Sub
		Private Sub changeMinWidth(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.MinWidth = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualWidth is set to " & rect1.ActualWidth
			txt2.Text = "Width is set to " & rect1.Width
			txt3.Text = "MinWidth is set to " & rect1.MinWidth
			txt4.Text = "MaxWidth is set to " & rect1.MaxWidth
		End Sub
		Private Sub changeMaxWidth(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.MaxWidth = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualWidth is set to " & rect1.ActualWidth
			txt2.Text = "Width is set to " & rect1.Width
			txt3.Text = "MinWidth is set to " & rect1.MinWidth
			txt4.Text = "MaxWidth is set to " & rect1.MaxWidth
		End Sub
		Private Sub clipRect(ByVal sender As Object, ByVal args As RoutedEventArgs)
			myCanvas.ClipToBounds = True
			txt5.Text = "Canvas.ClipToBounds is set to " & myCanvas.ClipToBounds
		End Sub
		Private Sub unclipRect(ByVal sender As Object, ByVal args As RoutedEventArgs)
			myCanvas.ClipToBounds = False
			txt5.Text = "Canvas.ClipToBounds is set to " & myCanvas.ClipToBounds
		End Sub
	End Class
End Namespace
