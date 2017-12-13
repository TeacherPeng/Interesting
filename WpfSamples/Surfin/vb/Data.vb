Imports System.Collections.ObjectModel ' ObserverableCollection
Imports System.IO ' DirectoryInfo

Namespace DataTemplatingLab
	' MyVideos is a collection of MyVideo objects
	' This class has a Directory string property
	' The Update() method takes all .wmv files from the specified directory
	' and adds them as MyVideo objects into the collection
	Public Class MyVideos
		Inherits ObservableCollection(Of MyVideo)
		Private _directory As DirectoryInfo

		Public Sub New()
		End Sub
		Public Sub New(ByVal directory As String)
			Me.Directory = directory
		End Sub

		Public Property Directory() As String
			Set(ByVal value As String)
				' Don't set path if directory is invalid
				If Not System.IO.Directory.Exists(value) Then
					MessageBox.Show("No Such Directory")
				End If

				_directory = New DirectoryInfo(value)

				Update()
			End Set
			Get
				Return _directory.FullName
			End Get
		End Property

		Private Sub Update()
			' Don't update if no directory to get files from
			If _directory Is Nothing Then
				Return
			End If

			' Remove all MyVideo objects from this collection
			Me.Clear()

			' Create MyVideo objects
			For Each f As FileInfo In _directory.GetFiles("*.wmv")
				Add(New MyVideo(f.FullName, f.Name))
			Next f
		End Sub
	End Class

	' MyVideo class
	' Properties: VideoTitle and Source
	Public Class MyVideo
		Private _name As String
		Private _path As String
		Private _source As Uri

		Public Sub New(ByVal path As String)
			_path = path
			_source = New Uri(path)
		End Sub

		Public Sub New(ByVal path As String, ByVal name As String)
			_name = name
			_path = path
			_source = New Uri(path)
		End Sub

		Public Property VideoTitle() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				If _name <> value Then
					_name = value
				End If
			End Set
		End Property

		Public ReadOnly Property Source() As String
			Get
				Return _path
			End Get
		End Property
	End Class
End Namespace