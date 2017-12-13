Imports System.Text

Namespace MessageBoxSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub showMessageBoxButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Configure the message box
			Dim owner As Window = (If(CBool(ownerCheckBox.IsChecked), Me, Nothing))
			Dim messageBoxText As String = Me.messageBoxText.Text
			Dim caption As String = Me.caption.Text
			Dim button As MessageBoxButton = CType(System.Enum.Parse(GetType(MessageBoxButton), Me.buttonComboBox.Text), MessageBoxButton)
			Dim icon As MessageBoxImage = CType(System.Enum.Parse(GetType(MessageBoxImage), Me.imageComboBox.Text), MessageBoxImage)
			Dim defaultResult As MessageBoxResult = CType(System.Enum.Parse(GetType(MessageBoxResult), Me.defaultResultComboBox.Text), MessageBoxResult)
			Dim options As MessageBoxOptions = CType(System.Enum.Parse(GetType(MessageBoxOptions), Me.optionsComboBox.Text), MessageBoxOptions)

			' Show message box, passing the window owner if specified
			Dim result As MessageBoxResult
			If owner Is Nothing Then
				result = MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options)
			Else
				result = MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult, options)
			End If

			' Show the result
			resultTextBlock.Text = "Result = " & result.ToString()
		End Sub
	End Class
End Namespace