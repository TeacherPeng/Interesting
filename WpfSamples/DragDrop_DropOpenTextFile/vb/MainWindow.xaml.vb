Imports System.Text
Imports System.IO

Namespace DropOpenTextFile
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()

			If CBool(cbWrap.IsChecked) Then
				tbDisplayFileContents.TextWrapping = TextWrapping.Wrap
			Else
				tbDisplayFileContents.TextWrapping = TextWrapping.NoWrap
			End If
		End Sub

		Private Sub clickClear(ByVal sender As Object, ByVal args As RoutedEventArgs)
			tbDisplayFileContents.Clear()
		End Sub

		Private Sub clickWrap(ByVal sender As Object, ByVal args As RoutedEventArgs)
			If CBool(cbWrap.IsChecked) Then
				tbDisplayFileContents.TextWrapping = TextWrapping.Wrap
			Else
				tbDisplayFileContents.TextWrapping = TextWrapping.NoWrap
			End If
		End Sub

		Private Sub ehDragOver(ByVal sender As Object, ByVal args As DragEventArgs)
			' As an arbitrary design decision, we only want to deal with a single file.
			If IsSingleFile(args) IsNot Nothing Then
				args.Effects = DragDropEffects.Copy
			Else
				args.Effects = DragDropEffects.None
			End If

			' Mark the event as handled, so TextBox's native DragOver handler is not called.
			args.Handled = True
		End Sub

		Private Sub ehDrop(ByVal sender As Object, ByVal args As DragEventArgs)
			' Mark the event as handled, so TextBox's native Drop handler is not called.
			args.Handled = True

			Dim fileName As String = IsSingleFile(args)
			If fileName Is Nothing Then
				Return
			End If

			Dim fileToLoad As New StreamReader(fileName)
			tbDisplayFileContents.Text = fileToLoad.ReadToEnd()
			fileToLoad.Close()

			' Set the window title to the loaded file.
			Me.Title = "File Loaded: " & fileName

		End Sub

		' If the data object in args is a single file, this method will return the filename.
		' Otherwise, it returns null.
		Private Function IsSingleFile(ByVal args As DragEventArgs) As String
			' Check for files in the hovering data object.
			If args.Data.GetDataPresent(DataFormats.FileDrop, True) Then
				Dim fileNames() As String = TryCast(args.Data.GetData(DataFormats.FileDrop, True), String())
				' Check fo a single file or folder.
				If fileNames.Length = 1 Then
					' Check for a file (a directory will return false).
					If File.Exists(fileNames(0)) Then
						' At this point we know there is a single file.
						Return fileNames(0)
					End If
				End If
			End If
			Return Nothing
		End Function
	End Class
End Namespace