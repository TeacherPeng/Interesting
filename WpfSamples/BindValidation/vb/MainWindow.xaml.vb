Imports System.Text

Namespace BindValidation
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub UseCustomHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim myBindingExpression As BindingExpression = textBox3.GetBindingExpression(TextBox.TextProperty)
			Dim myBinding As Binding = myBindingExpression.ParentBinding
            myBinding.UpdateSourceExceptionFilter = New UpdateSourceExceptionFilterCallback(AddressOf ReturnExceptionHandler)
			myBindingExpression.UpdateSource()
		End Sub

		Private Sub DisableCustomHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' textBox3 is an instance of a TextBox
			' the TextProperty is the data-bound dependency property
			Dim myBinding As Binding = BindingOperations.GetBinding(textBox3, TextBox.TextProperty)
            myBinding.UpdateSourceExceptionFilter = Nothing
            BindingOperations.GetBindingExpression(textBox3, TextBox.TextProperty).UpdateSource()
		End Sub

		Private Function ReturnExceptionHandler(ByVal bindingExpression As Object, ByVal exception As Exception) As Object
			Return "This is from the UpdateSourceExceptionFilterCallBack."
		End Function

	End Class
End Namespace
