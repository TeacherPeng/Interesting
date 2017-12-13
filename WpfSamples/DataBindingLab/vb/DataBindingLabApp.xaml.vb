Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Configuration

Namespace DataBindingLab
	Partial Public Class DataBindingLabApp
		Inherits Application
'INSTANT VB NOTE: The variable currentUser was renamed since Visual Basic does not allow class members with the same name:
		Private currentUser_Renamed As User
'INSTANT VB NOTE: The variable auctionItems was renamed since Visual Basic does not allow class members with the same name:
		Private auctionItems_Renamed As New ObservableCollection(Of AuctionItem)()

		Private Sub AppStartup(ByVal sender As Object, ByVal args As StartupEventArgs)
			LoadAuctionData()
			Dim mainWindow As New MainWindow()
			mainWindow.Show()
			
		End Sub

		Public Property CurrentUser() As User
			Get
				Return Me.currentUser_Renamed
			End Get
			Set(ByVal value As User)
				Me.currentUser_Renamed = value
			End Set
		End Property

		Public Property AuctionItems() As ObservableCollection(Of AuctionItem)
			Get
				Return Me.auctionItems_Renamed
			End Get
			Set(ByVal value As ObservableCollection(Of AuctionItem))
				Me.auctionItems_Renamed = value
			End Set
		End Property

		Private Sub LoadAuctionData()
			CurrentUser = New User("John", 12, New Date(2003, 4, 20))

'			#Region "Add Products to the auction"
			Dim userMary As New User("Mary", 10, New Date(2000, 5, 2))
			Dim userAnna As New User("Anna", 5, New Date(2001, 9, 13))
			Dim userMike As New User("Mike", 13, New Date(1999, 11, 23))
			Dim userMark As New User("Mark", 15, New Date(2004, 6, 3))

			Dim camera As New AuctionItem("Digital camera - good condition", ProductCategory.Electronics, 300, New Date(2005, 8, 23), userAnna, SpecialFeatures.None)
			camera.AddBid(New Bid(310, userMike))
			camera.AddBid(New Bid(312, userMark))
			camera.AddBid(New Bid(314, userMike))
			camera.AddBid(New Bid(320, userMark))

			Dim snowBoard As New AuctionItem("Snowboard and bindings", ProductCategory.Sports, 120, New Date(2005, 7, 12), userMike, SpecialFeatures.Highlight)
			snowBoard.AddBid(New Bid(140, userAnna))
			snowBoard.AddBid(New Bid(142, userMary))
			snowBoard.AddBid(New Bid(150, userAnna))

			Dim insideCSharp As New AuctionItem("Inside C#, second edition", ProductCategory.Books, 10, New Date(2005, 5, 29), Me.currentUser_Renamed, SpecialFeatures.Color)
			insideCSharp.AddBid(New Bid(11, userMark))
			insideCSharp.AddBid(New Bid(13, userAnna))
			insideCSharp.AddBid(New Bid(14, userMary))
			insideCSharp.AddBid(New Bid(15, userAnna))

			Dim laptop As New AuctionItem("Laptop - only 1 year old", ProductCategory.Computers, 500, New Date(2005, 8, 15), userMark, SpecialFeatures.Highlight)
			laptop.AddBid(New Bid(510, Me.currentUser_Renamed))

			Dim setOfChairs As New AuctionItem("Set of 6 chairs", ProductCategory.Home, 120, New Date(2005, 2, 20), userMike, SpecialFeatures.Color)

			Dim myDVDCollection As New AuctionItem("My DVD Collection", ProductCategory.DVDs, 5, New Date(2005, 8, 3), userMary, SpecialFeatures.Highlight)
			myDVDCollection.AddBid(New Bid(6, userMike))
			myDVDCollection.AddBid(New Bid(8, Me.currentUser_Renamed))

			Dim tvDrama As New AuctionItem("TV Drama Series", ProductCategory.DVDs, 40, New Date(2005, 7, 28), userAnna, SpecialFeatures.None)
			tvDrama.AddBid(New Bid(42, userMike))
			tvDrama.AddBid(New Bid(45, userMark))
			tvDrama.AddBid(New Bid(50, userMike))
			tvDrama.AddBid(New Bid(51, Me.currentUser_Renamed))

			Dim squashRacket As New AuctionItem("Squash racket", ProductCategory.Sports, 60, New Date(2005, 4, 4), userMark, SpecialFeatures.Highlight)
			squashRacket.AddBid(New Bid(62, userMike))
			squashRacket.AddBid(New Bid(65, userAnna))

			Me.AuctionItems.Add(camera)
			Me.AuctionItems.Add(snowBoard)
			Me.AuctionItems.Add(insideCSharp)
			Me.AuctionItems.Add(laptop)
			Me.AuctionItems.Add(setOfChairs)
			Me.AuctionItems.Add(myDVDCollection)
			Me.AuctionItems.Add(tvDrama)
			Me.AuctionItems.Add(squashRacket)
'			#End Region
		End Sub

	End Class
End Namespace