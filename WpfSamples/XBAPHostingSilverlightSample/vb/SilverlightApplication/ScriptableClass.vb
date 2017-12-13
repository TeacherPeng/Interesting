Imports System.Windows.Browser ' ScriptableMemberAttribute

Namespace SilverlightApplication
	<ScriptableType> _
	Public Class ScriptableClass
		' Method that can be called from HTML script
		<ScriptableMember> _
		Public Sub SendMessageToSilverlight(ByVal msg As String)
			msg = "Your Silverlight application has recieved a message: " & msg
			CType(App.Current.RootVisual, MainPage).msgTextBlock.Text = msg

		End Sub


	End Class
End Namespace
