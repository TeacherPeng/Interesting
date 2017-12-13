Imports System.Text
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Namespace EditingCollectionsSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
	   Private Sub edit_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If itemsControl.SelectedItem Is Nothing Then
				MessageBox.Show("No item is selected")
				Return
			End If

			Dim editableCollectionView As IEditableCollectionView = TryCast(itemsControl.Items, IEditableCollectionView)

			' Create a window that prompts the user to edit an item.
			Dim win As New ChangeItemWindow()
			editableCollectionView.EditItem(itemsControl.SelectedItem)
			win.DataContext = itemsControl.SelectedItem

			' If the user submits the new item, commit the changes.
			' If the user cancels the edits, discard the changes. 
			If CBool(win.ShowDialog()) Then
				editableCollectionView.CommitEdit()
			Else
				editableCollectionView.CancelEdit()
			End If
	   End Sub

		Private Sub add_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim editableCollectionView As IEditableCollectionView = TryCast(itemsControl.Items, IEditableCollectionView)

			If Not editableCollectionView.CanAddNew Then
				MessageBox.Show("You cannot add items to the list.")
				Return
			End If

			' Create a window that prompts the user to enter a new
			' item to sell.
			Dim win As New ChangeItemWindow()

			'Create a new item to be added to the collection.
			win.DataContext = editableCollectionView.AddNew()

			' If the user submits the new item, commit the new
			' object to the collection.  If the user cancels 
			' adding the new item, discard the new item.
			If CBool(win.ShowDialog()) Then
				editableCollectionView.CommitNew()
			Else
				editableCollectionView.CancelNew()
			End If

		End Sub

		Private Sub remove_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim item As PurchaseItem = TryCast(itemsControl.SelectedItem, PurchaseItem)

			If item Is Nothing Then
				MessageBox.Show("No Item is selected")
				Return
			End If

			Dim editableCollectionView As IEditableCollectionView = TryCast(itemsControl.Items, IEditableCollectionView)

			If Not editableCollectionView.CanRemove Then
				MessageBox.Show("You cannot remove items from the list.")
				Return
			End If

			If MessageBox.Show("Are you sure you want to remove " & item.Description, "Remove Item", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
				editableCollectionView.Remove(itemsControl.SelectedItem)
			End If
		End Sub
	End Class

	' PurchaseItem implements INotifyPropertyChanged so that the 
	' application is notified when a property changes.  It 
	' implements IEditableObject so that pending changes can be discarded.
	Public Class PurchaseItem
		Implements INotifyPropertyChanged, IEditableObject
		Private Structure ItemData
			Friend Description As String
			Friend Price As Double
			Friend OfferExpires As Date
		End Structure
		Private copyData As ItemData
		Private currentData As ItemData

		Public Sub New()
			Me.New("New item", 0, Date.Now)
		End Sub

		Public Sub New(ByVal desc As String, ByVal price As Double, ByVal endDate As Date)
			Description = desc
			Me.Price = price
			OfferExpires = endDate
		End Sub

		Public Overrides Function ToString() As String
			Return String.Format("{0}, {1:c}, {2:D}", Description, Price, OfferExpires)
		End Function

		Public Property Description() As String
			Get
				Return currentData.Description
			End Get
			Set(ByVal value As String)
				If currentData.Description <> value Then
					currentData.Description = value
					NotifyPropertyChanged("Description")
				End If
			End Set
		End Property

		Public Property Price() As Double
			Get
				Return currentData.Price
			End Get
			Set(ByVal value As Double)
				If currentData.Price <> value Then
					currentData.Price = value
					NotifyPropertyChanged("Price")
				End If
			End Set
		End Property

		Public Property OfferExpires() As Date
			Get
				Return currentData.OfferExpires
			End Get
			Set(ByVal value As Date)
                If value <> currentData.OfferExpires Then
                    currentData.OfferExpires = value
                    NotifyPropertyChanged("OfferExpires")
                End If
			End Set
		End Property

		#Region "INotifyPropertyChanged Members"

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Private Sub NotifyPropertyChanged(ByVal info As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
		End Sub

		#End Region

		#Region "IEditableObject Members"

		Public Sub BeginEdit() Implements IEditableObject.BeginEdit
			copyData = currentData
		End Sub

		Public Sub CancelEdit() Implements IEditableObject.CancelEdit
			currentData = copyData
			NotifyPropertyChanged("")

		End Sub

		Public Sub EndEdit() Implements IEditableObject.EndEdit
			copyData = New ItemData()

		End Sub

		#End Region

	End Class

	Public Class ItemsForSale
		Inherits ObservableCollection(Of PurchaseItem)
		Public Sub New()
			Add((New PurchaseItem("Snowboard and bindings", 120, New Date(2009, 1, 1))))
			Add((New PurchaseItem("Inside C#, second edition", 10, New Date(2009, 2, 2))))
			Add((New PurchaseItem("Laptop - only 1 year old", 499.99, New Date(2009, 2, 28))))
			Add((New PurchaseItem("Set of 6 chairs", 120, New Date(2009, 2, 28))))
			Add((New PurchaseItem("My DVD Collection", 15, New Date(2009, 1, 1))))
			Add((New PurchaseItem("TV Drama Series", 39.985, New Date(2009, 1, 1))))
			Add((New PurchaseItem("Squash racket", 60, New Date(2009, 2, 28))))
		End Sub

	End Class
End Namespace
