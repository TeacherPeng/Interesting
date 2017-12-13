Imports System.Configuration
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Interop


Namespace Microsoft.Samples.Animation.AnimationGallery
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application
		Public Sub New()

		End Sub

		Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
			Dim myWindow As New Window()
			myWindow.Content = New GridSampleViewer()
			myWindow.Show()
			MyBase.OnStartup(e)
		End Sub

	End Class
End Namespace
