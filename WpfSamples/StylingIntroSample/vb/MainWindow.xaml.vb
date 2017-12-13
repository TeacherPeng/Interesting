Imports System.Text

Namespace StylingIntroSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private Photos As PhotoList

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Photos = CType((TryCast(Me.Resources("MyPhotos"), ObjectDataProvider)).Data, PhotoList)
			Photos.Path = "...\...\Images"
		End Sub
	End Class
End Namespace