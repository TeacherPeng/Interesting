' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Animation

Namespace ColorPickerLib
	''' <summary>
	''' Interaction logic for ColorPickerDialog.xaml
	''' </summary>

	Partial Public Class ColorPickerDialog
		Inherits Window


		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub okButtonClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)

			OKButton.IsEnabled = False
			m_color = cPicker.SelectedColor
			DialogResult = True
			Hide()

		End Sub


		Private Sub cancelButtonClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)

			OKButton.IsEnabled = False
			DialogResult = False

		End Sub

		Private Sub onSelectedColorChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Color))

			If e.NewValue <> m_color Then

				OKButton.IsEnabled = True
			End If
		End Sub

		Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)

			OKButton.IsEnabled = False
			MyBase.OnClosing(e)
		End Sub


		Private m_color As New Color()
'INSTANT VB NOTE: The variable startingColor was renamed since Visual Basic does not allow class members with the same name:
		Private startingColor_Renamed As New Color()

		Public ReadOnly Property SelectedColor() As Color
			Get
				Return m_color
			End Get

		End Property

		Public Property StartingColor() As Color
			Get
				Return startingColor_Renamed
			End Get
			Set(ByVal value As Color)
				cPicker.SelectedColor = value
				OKButton.IsEnabled = False

			End Set

		End Property


	End Class
End Namespace