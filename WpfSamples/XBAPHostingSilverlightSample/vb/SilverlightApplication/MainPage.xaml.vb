Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Browser

Namespace SilverlightApplication
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
			Dim scClass As New ScriptableClass()
			HtmlPage.RegisterScriptableObject("scrptClass", scClass)
		End Sub


		Private Sub sendMessageToWPFButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Try
				'Call the SendMessageToWPFScript method in the SilverlightApplicationTestPage.html
			   HtmlPage.Window.Invoke("SendMessageToWPFScript", Me.msgTextBox.Text)

			Catch exc As Exception
				msgTextBlock.Text = exc.Message.ToString()
			End Try
		End Sub
	End Class
End Namespace
