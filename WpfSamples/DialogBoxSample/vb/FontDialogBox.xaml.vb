Imports System
Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.Reflection
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Media

Namespace DialogBoxSample

	Partial Public Class FontDialogBox
		Inherits Window
		Public Sub New()
			InitializeComponent()

			Me.fontFamilyListBox.ItemsSource = FontPropertyLists.FontFaces
			Me.fontStyleListBox.ItemsSource = FontPropertyLists.FontStyles
			Me.fontWeightListBox.ItemsSource = FontPropertyLists.FontWeights
			Me.fontSizeListBox.ItemsSource = FontPropertyLists.FontSizes
		End Sub

		Public Shadows Property FontFamily() As FontFamily
			Get
				Return CType(Me.fontFamilyListBox.SelectedItem, FontFamily)
			End Get
			Set(ByVal value As FontFamily)
				Me.fontFamilyListBox.SelectedItem = value
				Me.fontFamilyListBox.ScrollIntoView(value)
			End Set
		End Property
		Public Shadows Property FontStyle() As FontStyle
			Get
				Return CType(Me.fontStyleListBox.SelectedItem, FontStyle)
			End Get
			Set(ByVal value As FontStyle)
				Me.fontStyleListBox.SelectedItem = value
				Me.fontStyleListBox.ScrollIntoView(value)
			End Set
		End Property
		Public Shadows Property FontWeight() As FontWeight
			Get
				Return CType(Me.fontWeightListBox.SelectedItem, FontWeight)
			End Get
			Set(ByVal value As FontWeight)
				Me.fontWeightListBox.SelectedItem = value
				Me.fontWeightListBox.ScrollIntoView(value)
			End Set
		End Property
		Public Shadows Property FontSize() As Double
			Get
				Return CDbl(Me.fontSizeListBox.SelectedItem)
			End Get
			Set(ByVal value As Double)
				Me.fontSizeListBox.SelectedItem = value
				Me.fontSizeListBox.ScrollIntoView(value)
			End Set
		End Property

		Private Sub okButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Dialog box accepted
			Me.DialogResult = True
		End Sub

		Private Sub fontFamilyTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			' If user enters family text, select family in list if matching item found
			Me.FontFamily = New FontFamily(Me.fontFamilyTextBox.Text)
		End Sub
		Private Sub fontStyleTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			' If user enters style text, select style in list if matching item found
			If FontPropertyLists.CanParseFontStyle(Me.fontStyleTextBox.Text) Then
				Me.FontStyle = FontPropertyLists.ParseFontStyle(Me.fontStyleTextBox.Text)
			End If
		End Sub
		Private Sub fontWeightTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			' If user enters weight text, select weight in list if matching item found
			If FontPropertyLists.CanParseFontWeight(Me.fontWeightTextBox.Text) Then
				Me.FontWeight = FontPropertyLists.ParseFontWeight(Me.fontWeightTextBox.Text)
			End If
		End Sub
		Private Sub fontSizeTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			' If user enters size text, select size in list if matching item found
			Dim fontSize As Double
			If Double.TryParse(Me.fontSizeTextBox.Text, fontSize) Then
				Me.FontSize = fontSize
			End If
		End Sub
	End Class
End Namespace