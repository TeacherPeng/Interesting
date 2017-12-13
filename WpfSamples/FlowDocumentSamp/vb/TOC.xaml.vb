Namespace CustomTOC
	Partial Public Class Page1
		Inherits Page
		Public Sub expandTOC(ByVal sender As Object, ByVal args As SelectionChangedEventArgs)
			If node1.IsSelected=True And lb1.Visibility = Visibility.Collapsed Then
				lb1.Visibility = Visibility.Visible
				node1.IsSelected = False
			ElseIf node1.IsSelected = True And lb1.Visibility = Visibility.Visible Then
				lb1.Visibility = Visibility.Collapsed
				node1.IsSelected = False
			ElseIf node2.IsSelected = True And lb2.Visibility = Visibility.Collapsed Then
				lb2.Visibility = Visibility.Visible
				node2.IsSelected = False
			ElseIf node2.IsSelected = True And lb2.Visibility = Visibility.Visible Then
				lb2.Visibility = Visibility.Collapsed
				node2.IsSelected = False
			End If
		End Sub

	End Class
End Namespace