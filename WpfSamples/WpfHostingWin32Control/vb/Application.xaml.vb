Imports System.Configuration
Imports System.Runtime.InteropServices

Namespace WpfHostingWin32Control
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application
		<DllImport("comctl32.dll", EntryPoint := "InitCommonControls", CharSet := CharSet.Auto)>
		Public Shared Sub InitCommonControls()
		End Sub
		Private Sub ApplicationStartup(ByVal sender As Object, ByVal args As StartupEventArgs)
			InitCommonControls()
			Dim host As New HostWindow()
			host.InitializeComponent()
		End Sub
	End Class
End Namespace
