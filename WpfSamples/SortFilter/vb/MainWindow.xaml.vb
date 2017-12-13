Imports System.Text
Imports System.Collections
Imports System.ComponentModel
Imports SortFilterSample

Namespace SortFilterSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class SortFilter
		Public Function Contains(ByVal de As Object) As Boolean
			Dim order As Order = TryCast(de, Order)
			'Return members whose Orders have not been filled
			Return (order.Filled = "No")
		End Function

		Public myCollectionView As ListCollectionView

		' Object o keeps the currency for the table
		Public o As SortFilterSample.Order

		Public Sub StartHere(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)
			myCollectionView = CType(CollectionViewSource.GetDefaultView(rootElement.DataContext), ListCollectionView)
		End Sub

		Private Sub OnClick(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Dim button As Button = TryCast(sender, Button)
			'Sort the data based on the column selected
			myCollectionView.SortDescriptions.Clear()
			Select Case button.Name.ToString()
				Case "orderButton"
					myCollectionView.SortDescriptions.Add(New SortDescription("OrderItem", ListSortDirection.Ascending))
				Case "customerButton"
					myCollectionView.SortDescriptions.Add(New SortDescription("Customer", ListSortDirection.Ascending))
				Case "nameButton"
					myCollectionView.SortDescriptions.Add(New SortDescription("Name", ListSortDirection.Ascending))
				Case "idButton"
					myCollectionView.SortDescriptions.Add(New SortDescription("Id", ListSortDirection.Ascending))
				Case "filledButton"
					myCollectionView.SortDescriptions.Add(New SortDescription("Filled", ListSortDirection.Ascending))
			End Select
		End Sub

		'OnBrowse is called whenever the Next or Previous buttons
		'are clicked to change the currency
		Private Sub OnBrowse(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Dim b As Button = TryCast(sender, Button)
			Select Case b.Name
				Case "Previous"
					If myCollectionView.MoveCurrentToPrevious() Then
						feedbackText.Text = ""
						o = TryCast(myCollectionView.CurrentItem, Order)
					Else
						myCollectionView.MoveCurrentToFirst()
						feedbackText.Text = "At first record"
					End If
				Case "Next"
					If myCollectionView.MoveCurrentToNext() Then
						feedbackText.Text = ""
						o = TryCast(myCollectionView.CurrentItem, Order)
					Else
						myCollectionView.MoveCurrentToLast()
						feedbackText.Text = "At last record"
					End If
			End Select
		End Sub

		'OnButton is called whenever the Next or Previous buttons
		'are clicked to change the currency

		Private Sub OnFilter(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Dim b As Button = TryCast(sender, Button)
			Select Case b.Name
				Case "Filter"
					myCollectionView.Filter = New Predicate(Of Object)(AddressOf Contains)
				Case "Unfilter"
					myCollectionView.Filter = Nothing
			End Select
		End Sub

	End Class
End Namespace
