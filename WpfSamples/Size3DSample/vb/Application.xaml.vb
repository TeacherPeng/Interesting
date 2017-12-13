Imports System.Configuration

Namespace Mil3dSize
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application
		Private Sub AppStartingUp(ByVal sender As Object, ByVal e As StartupEventArgs)
			Dim mainWindow As New MainWindow()
			mainWindow.Show()

			' displays variables
			mainWindow.ShowVars()
		End Sub
	End Class
End Namespace
