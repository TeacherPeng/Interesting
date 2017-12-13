Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace DataBindingLab
	Public Class AuctionItem
		Implements INotifyPropertyChanged

        Private descriptionValue As String

        Private startPriceValue As Integer

        Private startDateValue As Date

        Private categoryValue As ProductCategory

        Private ownerValue As User

        Private specialFeaturesValue As SpecialFeatures

        Private bidsValue As ObservableCollection(Of Bid)

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#Region "Properties Getters and Setters"
        Public Property Description() As String
            Get
                Return Me.descriptionValue
            End Get
            Set(ByVal value As String)
                Me.descriptionValue = value
                OnPropertyChanged("Description")
            End Set
        End Property

        Public Property StartPrice() As Integer
            Get
                Return Me.startPriceValue
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then
                    Throw New ArgumentException("Price must be positive")
                End If
                Me.startPriceValue = value
                OnPropertyChanged("StartPrice")
                OnPropertyChanged("CurrentPrice")
            End Set
        End Property

        Public Property StartDate() As Date
            Get
                Return Me.startDateValue
            End Get
            Set(ByVal value As Date)
                Me.startDateValue = value
                OnPropertyChanged("StartDate")
            End Set
        End Property

        Public Property Category() As ProductCategory
            Get
                Return Me.categoryValue
            End Get
            Set(ByVal value As ProductCategory)
                Me.categoryValue = value
                OnPropertyChanged("Category")
            End Set
        End Property

        Public ReadOnly Property Owner() As User
            Get
                Return Me.ownerValue
            End Get
        End Property

        Public Property SpecialFeatures() As SpecialFeatures
            Get
                Return Me.specialFeaturesValue
            End Get
            Set(ByVal value As SpecialFeatures)
                Me.specialFeaturesValue = value
                OnPropertyChanged("SpecialFeatures")
            End Set
        End Property

        Public ReadOnly Property Bids() As ReadOnlyObservableCollection(Of Bid)
            Get
                Return New ReadOnlyObservableCollection(Of Bid)(Me.bidsValue)
            End Get
        End Property

        Public ReadOnly Property CurrentPrice() As Integer
            Get
                Dim price As Integer = 0
                ' There is at least on bid on this product
                If Me.bidsValue.Count > 0 Then
                    ' Get the amount of the last bid
                    Dim lastBid As Bid = Me.bidsValue(Me.bidsValue.Count - 1)
                    price = lastBid.Amount
                    ' No bids on this product yet
                Else
                    price = Me.startPriceValue
                End If
                Return price
            End Get
        End Property
#End Region

        Public Sub New(ByVal description As String, ByVal category As ProductCategory, ByVal startPrice As Integer, ByVal startDate As Date, ByVal owner As User, ByVal specialFeatures As SpecialFeatures)
            Me.descriptionValue = description
            Me.categoryValue = category
            Me.startPriceValue = startPrice
            Me.startDateValue = startDate
            Me.ownerValue = owner
            Me.specialFeaturesValue = specialFeatures
            Me.bidsValue = New ObservableCollection(Of Bid)()
        End Sub

        ' Exposing Bids as a ReadOnlyObservableCollection and adding an AddBid method so that CurrentPrice 
        ' is updated when a new Bid is added
        Public Sub AddBid(ByVal bid As Bid)
            Me.bidsValue.Add(bid)
            OnPropertyChanged("CurrentPrice")
        End Sub

		Protected Sub OnPropertyChanged(ByVal name As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
		End Sub
	End Class

	Public Enum ProductCategory
		Books
		Computers
		DVDs
		Electronics
		Home
		Sports
	End Enum

	Public Enum SpecialFeatures
		None
		Color
		Highlight
	End Enum

End Namespace

