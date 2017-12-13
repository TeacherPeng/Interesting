' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.IO
Imports System.Collections

Namespace ImageView
	''' <summary>
	''' Interaction logic for default.xaml
	''' </summary>
	Partial Public Class ImageViewExample
		Inherits Window
	Private imageFiles As ArrayList

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As EventArgs)
			imageFiles = GetImageFileInfo()
			imageListBox.DataContext = imageFiles

		End Sub

		Private Sub showImage(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			Dim list As ListBox = (CType(sender, ListBox))
			If list IsNot Nothing Then
				Dim index As Integer = list.SelectedIndex 'Save the selected index
				If index >= 0 Then
					Dim selection As String = list.SelectedItem.ToString()

					If (selection IsNot Nothing) AndAlso (selection.Length <> 0) Then
						'Set currentImage to selected Image
				  Dim selLoc As New Uri(selection)
				  Dim id As New BitmapImage(selLoc)
						Dim currFileInfo As New FileInfo(selection)
						currentImage.Source = id

						'Setup Info Text
						imageSize.Text = id.PixelWidth.ToString() & " x " & id.PixelHeight.ToString()
						imageFormat.Text = id.Format.ToString()
						fileSize.Text = ((currFileInfo.Length + 512) / 1024).ToString() & "k"

					End If
				End If
			End If
		End Sub

		Private Function GetImageFileInfo() As ArrayList
			Dim imageFiles As New ArrayList()
			Dim files() As String

		 'Get directory path of myData (down two directory levels)
		 Dim currDir As String = Directory.GetCurrentDirectory()
		 Dim temp As String = currDir & "\..\..\myData"
			files = Directory.GetFiles(temp, "*.jpg")

			For Each image As String In files
				Dim info As New FileInfo(image)
				imageFiles.Add(info)
			Next image

			Return imageFiles
		End Function
	End Class
End Namespace