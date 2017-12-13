Imports System.Text

Namespace WPFBrowserApplication
	''' <summary>
	''' Interaction logic for Page1.xaml
	''' </summary>
	Partial Public Class Page1
		Inherits Page
		Public Sub New()
			AddHandler Loaded, AddressOf HomePage_Loaded
			InitializeComponent()
		End Sub

		Private Sub HomePage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Register the scriptable object
			Me.webBrowser.ObjectForScripting = New ScriptableClass()
		End Sub

		Private Sub sendMessageToSilverlightButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Try
			  ' Call the SendMessageToSilverlightScript method in the SilverlightApplicationTestPage.html
				Me.webBrowser.InvokeScript("SendMessageToSilverlightScript", Me.msgTextBox.Text)

			Catch exc As Exception
				msgTextBox.Text = exc.InnerException.Message.ToString()
			End Try
		End Sub
	End Class
End Namespace
