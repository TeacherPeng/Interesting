' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text

Namespace Adorners_ResizingAdorner
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private adornerLayer As AdornerLayer
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			adornerLayer = AdornerLayer.GetAdornerLayer(elementsGrid)

			For Each toAdorn As Panel In elementsGrid.Children
				adornerLayer.Add(New ResizingAdorner(toAdorn.Children(0)))
			Next toAdorn
		End Sub
	End Class
End Namespace
