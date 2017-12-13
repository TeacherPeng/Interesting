Namespace DialogBoxSample
	Partial Public Class MarginsDialogBox
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub

		Public Property DocumentMargin() As Thickness
			Get
				Return CType(Me.DataContext, Thickness)
			End Get
			Set(ByVal value As Thickness)
				Me.DataContext = value
			End Set
		End Property

		Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Dialog box canceled
			Me.DialogResult = False
		End Sub

		Private Sub okButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Don't accept the dialog box if there is invalid data
			If Not IsValid(Me) Then
				Return
			End If

			' Dialog box accepted
			Me.DialogResult = True
		End Sub

		' Validate all dependency objects in a window
		Private Function IsValid(ByVal node As DependencyObject) As Boolean
			' Check if dependency object was passed
			If node IsNot Nothing Then
				' Check if dependency object is valid.
				' NOTE: Validation.GetHasError works for controls that have validation rules attached 
                Dim boolIsValid As Boolean = Not Validation.GetHasError(node)
                If Not boolIsValid Then
                    ' If the dependency object is invalid, and it can receive the focus,
                    ' set the focus
                    If TypeOf node Is IInputElement Then
                        Keyboard.Focus(CType(node, IInputElement))
                    End If
                    Return False
                End If
			End If

			' If this dependency object is valid, check all child dependency objects
			For Each subnode As Object In LogicalTreeHelper.GetChildren(node)
				If TypeOf subnode Is DependencyObject Then
					' If a child dependency object is invalid, return false immediately,
					' otherwise keep checking
					If IsValid(CType(subnode, DependencyObject)) = False Then
						Return False
					End If
				End If
			Next subnode

			' All dependency objects are valid
			Return True
		End Function
	End Class
End Namespace
