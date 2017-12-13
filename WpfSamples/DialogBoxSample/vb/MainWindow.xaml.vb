Imports System.Text
Imports System.ComponentModel ' CancelEventArgs
Imports Microsoft.Win32 ' OpenFileDialog

Namespace DialogBoxSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
Private needsToBeSaved As Boolean
		Public Sub New()
			InitializeComponent()
		End Sub
' Closing
		Private Sub mainWindow_Closing(ByVal sender As Object, ByVal e As CancelEventArgs)
			' If the document needs to be saved
			If Me.needsToBeSaved Then
				' Configure the message box
				Dim messageBoxText As String = "This document needs to be saved. Click Yes to save and exit, No to exit without saving, or Cancel to not exit."
				Dim caption As String = "Word Processor"
				Dim button As MessageBoxButton = MessageBoxButton.YesNoCancel
				Dim icon As MessageBoxImage = MessageBoxImage.Warning

				' Display message box
				Dim messageBoxResult As MessageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon)

				' Process message box results
				Select Case messageBoxResult
					Case MessageBoxResult.Yes ' Save document and exit
						SaveDocument()
					Case MessageBoxResult.No ' Exit without saving
					Case MessageBoxResult.Cancel ' Don't exit
						e.Cancel = True
				End Select
			End If
		End Sub

		Private Sub fileOpen_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			OpenDocument()
		End Sub
		Private Sub fileSave_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			SaveDocument()
		End Sub
		Private Sub filePrint_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			PrintDocument()
		End Sub
		Private Sub fileExit_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Me.Close()
		End Sub

		Private Sub editFindMenuItem_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Instantiate the dialog box
			Dim dlg As New FindDialogBox(Me.documentTextBox)

			' Configure the dialog box
			dlg.Owner = Me
			AddHandler dlg.TextFound, AddressOf dlg_TextFound

			' Open the dialog box modally
			dlg.Show()
		End Sub

		Private Sub formatMarginsMenuItem_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Instantiate the dialog box
			Dim dlg As New MarginsDialogBox()

			' Configure the dialog box
			dlg.Owner = Me
			dlg.DocumentMargin = Me.documentTextBox.Margin

			' Open the dialog box modally 
			dlg.ShowDialog()

			' Process data entered by user if dialog box is accepted
			If dlg.DialogResult = True Then
				' Update fonts
				Me.documentTextBox.Margin = dlg.DocumentMargin
			End If

		End Sub


		Private Sub formatFontMenuItem_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Instantiate the dialog box
			Dim dlg As New FontDialogBox()

			' Configure the dialog box
			dlg.Owner = Me
			dlg.FontFamily = Me.documentTextBox.FontFamily
			dlg.FontSize = Me.documentTextBox.FontSize
			dlg.FontWeight = Me.documentTextBox.FontWeight
			dlg.FontStyle = Me.documentTextBox.FontStyle

			' Open the dialog box modally 
			dlg.ShowDialog()

			' Process data entered by user if dialog box is accepted
			If dlg.DialogResult = True Then
				' Update fonts
				Me.documentTextBox.FontFamily = dlg.FontFamily
				Me.documentTextBox.FontSize = dlg.FontSize
				Me.documentTextBox.FontWeight = dlg.FontWeight
				Me.documentTextBox.FontStyle = dlg.FontStyle
			End If
		End Sub

		' Detect when document has been altered
		Private Sub documentTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			Me.needsToBeSaved = True
		End Sub

		Private Sub OpenDocument()
			' Instantiate the dialog box
			Dim dlg As New OpenFileDialog()

			' Configure open file dialog box
			dlg.FileName = "Document" ' Default file name
            dlg.DefaultExt = ".txt" ' Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt" ' Filter files by extension

			' Open the dialog box modally
			Dim result? As Boolean = dlg.ShowDialog()

			' Process open file dialog box results
			If result = True Then
				' Open document
				Dim filename As String = dlg.FileName
			End If
		End Sub
		Private Sub SaveDocument()
			' Configure save file dialog
			Dim dlg As New SaveFileDialog()
			dlg.FileName = "Document" ' Default file name
            dlg.DefaultExt = ".txt" ' Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt" ' Filter files by extension

			' Show save file dialog
			Dim result? As Boolean = dlg.ShowDialog()

			' Process save file dialog results
			If result = True Then
				' Save document
				Dim filename As String = dlg.FileName
			End If
		End Sub
		Private Sub PrintDocument()
			' Configure printer dialog
			Dim dlg As New PrintDialog()
			dlg.PageRangeSelection = PageRangeSelection.AllPages
			dlg.UserPageRangeEnabled = True

			' Show save file dialog
			Dim result? As Boolean = dlg.ShowDialog()

			' Process save file dialog results
			If result = True Then
				' Print document
			End If
		End Sub


		Private Sub dlg_TextFound(ByVal sender As Object, ByVal e As EventArgs)
			' Get the find dialog box that raised the event
			Dim dlg As FindDialogBox = CType(sender, FindDialogBox)

			' Get find results and select found text
			Me.documentTextBox.Select(dlg.Index, dlg.Length)
			Me.documentTextBox.Focus()
		End Sub

	End Class
End Namespace

