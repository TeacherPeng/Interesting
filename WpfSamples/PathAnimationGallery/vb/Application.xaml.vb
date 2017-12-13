Imports System.Configuration
Imports System.Windows.Media.Animation
Imports System.IO

Namespace PathAnimationGallery
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application

		Private Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal args As UnhandledExceptionEventArgs)

			Try
				Dim wr As New StreamWriter("error.txt")
				wr.Write(args.ExceptionObject.ToString())
				wr.Close()

			Catch

			End Try


			MessageBox.Show("Unhandled exception: " & args.ExceptionObject.ToString())
		End Sub

	End Class


End Namespace
