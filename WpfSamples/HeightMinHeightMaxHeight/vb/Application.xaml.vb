Imports System.Configuration

Namespace Height_MinHeight_MaxHeight_CSharp
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application
		Private Sub AppStartingUp(ByVal sender As Object, ByVal e As StartupEventArgs)
			Dim mainWindow As New MainWindow()
			mainWindow.InitializeComponent()
			mainWindow.Show()
		End Sub
	End Class
End Namespace
