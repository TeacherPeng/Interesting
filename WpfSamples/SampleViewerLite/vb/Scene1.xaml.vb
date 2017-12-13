Imports System.Text
Imports System.ComponentModel
Imports System.Xml
Imports System.IO
Imports System.Windows.Markup

Namespace SdkXamlBrowser
	''' <summary>
	''' Interaction logic for Scene1.xaml
	''' </summary>
	Partial Public Class Scene1
		Public RealTimeUpdate As Boolean = True

		Public Sub New()
            InitializeComponent()
		End Sub
		Private Sub HandleSelectionChanged(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			If sender Is Nothing Then
				Return
			End If

			Details.DataContext = (TryCast(sender, ListBox)).DataContext
		End Sub

		Protected Sub HandleTextChanged(ByVal sender As Object, ByVal [me] As TextChangedEventArgs)
			If RealTimeUpdate Then
				ParseCurrentBuffer()
			End If
		End Sub

		Private Sub ParseCurrentBuffer()
			Try
			Dim ms As New MemoryStream()
			Dim sw As New StreamWriter(ms)
			Dim str As String = TextBox1.Text
			sw.Write(str)
			sw.Flush()
			ms.Flush()
			ms.Position = 0
				Try
					Dim content As Object = XamlReader.Load(ms)
					If content IsNot Nothing Then

						cc.Children.Clear()
						cc.Children.Add(CType(content, UIElement))
					End If
					TextBox1.Foreground = Brushes.Black
					ErrorText.Text = ""

				Catch xpe As XamlParseException
					TextBox1.Foreground = Brushes.Red
					TextBox1.TextWrapping = TextWrapping.Wrap
					ErrorText.Text = xpe.Message.ToString()
				End Try
			Catch e1 As Exception
				Return
			End Try
		End Sub
		Protected Sub onClickParseButton(ByVal sender As Object, ByVal args As RoutedEventArgs)
			ParseCurrentBuffer()
		End Sub
		Protected Sub ShowPreview(ByVal sender As Object, ByVal args As RoutedEventArgs)
			PreviewRow.Height = New GridLength(1, GridUnitType.Star)
			CodeRow.Height = New GridLength(0)
		End Sub
		Protected Sub ShowCode(ByVal sender As Object, ByVal args As RoutedEventArgs)
			PreviewRow.Height = New GridLength(0)
			CodeRow.Height = New GridLength(1, GridUnitType.Star)
		End Sub
		Protected Sub ShowSplit(ByVal sender As Object, ByVal args As RoutedEventArgs)
			PreviewRow.Height = New GridLength(1, GridUnitType.Star)
			CodeRow.Height = New GridLength(1, GridUnitType.Star)
		End Sub

	End Class
End Namespace
