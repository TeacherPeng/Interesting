Namespace TreeListViewSample
	Public Class TreeListView
		Inherits TreeView
		Protected Overrides Function GetContainerForItemOverride() As DependencyObject
			Return New TreeListViewItem()
		End Function

		Protected Overrides Function IsItemItsOwnContainerOverride(ByVal item As Object) As Boolean
			Return TypeOf item Is TreeListViewItem
		End Function
	End Class

	Public Class TreeListViewItem
		Inherits TreeViewItem
		''' <summary>
		''' Item's hierarchy in the tree
		''' </summary>
        Public ReadOnly Property Level As Integer
            Get
                If levelValue = -1 Then
                    Dim parent As TreeListViewItem = TryCast(ItemsControl.ItemsControlFromItemContainer(Me), TreeListViewItem)
                    levelValue = If((parent IsNot Nothing), parent.Level + 1, 0)
                End If
                Return levelValue
            End Get
        End Property


        Protected Overrides Function GetContainerForItemOverride() As DependencyObject
            Return New TreeListViewItem()
        End Function

        Protected Overrides Function IsItemItsOwnContainerOverride(ByVal item As Object) As Boolean
            Return TypeOf item Is TreeListViewItem
        End Function

        Private levelValue As Integer = -1
	End Class

End Namespace
