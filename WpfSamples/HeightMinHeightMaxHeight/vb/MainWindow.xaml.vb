' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text

Namespace Height_MinHeight_MaxHeight_CSharp
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private Sub changeHeight(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.Height = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualHeight is set to " & rect1.ActualHeight
			txt2.Text = "Height is set to " & rect1.Height
			txt3.Text = "MinHeight is set to " & rect1.MinHeight
			txt4.Text = "MaxHeight is set to " & rect1.MaxHeight
		End Sub
		Private Sub changeMinHeight(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.MinHeight = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualHeight is set to " & rect1.ActualHeight
			txt2.Text = "Height is set to " & rect1.Height
			txt3.Text = "MinHeight is set to " & rect1.MinHeight
			txt4.Text = "MaxHeight is set to " & rect1.MaxHeight
		End Sub
		Private Sub changeMaxHeight(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim li As ListBoxItem = (TryCast((TryCast(sender, ListBox)).SelectedItem, ListBoxItem))
			Dim sz1 As Double = Double.Parse(li.Content.ToString())
			rect1.MaxHeight = sz1
			rect1.UpdateLayout()
			txt1.Text = "ActualHeight is set to " & rect1.ActualHeight
			txt2.Text = "Height is set to " & rect1.Height
			txt3.Text = "MinHeight is set to " & rect1.MinHeight
			txt4.Text = "MaxHeight is set to " & rect1.MaxHeight
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