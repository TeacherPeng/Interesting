Imports System.Text

Namespace FlowDocument_Viewer
	''' <summary>
	''' Interaction logic for Default.xaml
	''' </summary>
	Partial Public Class Page1
		Inherits Page
			   Private app As Application

		Public Sub menuExit(ByVal sender As Object, ByVal args As RoutedEventArgs)
				app = CType(Application.Current, Application)
				app.Shutdown()
		End Sub

		Public Sub menuAbout(ByVal sender As Object, ByVal args As RoutedEventArgs)
			frame2.Source = New Uri("about.xaml", UriKind.Relative)
		End Sub
	End Class

End Namespace