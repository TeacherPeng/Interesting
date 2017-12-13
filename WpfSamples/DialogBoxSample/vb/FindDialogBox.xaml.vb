Imports System.Text.RegularExpressions ' Regex

Namespace DialogBoxSample
	Partial Public Class FindDialogBox
		Inherits Window
		Public Event TextFound As TextFoundEventHandler
		Protected Overridable Sub OnTextFound()
			Dim textFound As TextFoundEventHandler = Me.TextFoundEvent
			If TextFoundEvent IsNot Nothing Then
				TextFoundEvent(Me, EventArgs.Empty)
			End If
		End Sub

		Public Sub New(ByVal textBoxToSearch As TextBox)
			InitializeComponent()

			Me.textBoxToSearch = textBoxToSearch

			' If text box that's being searched is changed, reset search
			AddHandler Me.textBoxToSearch.TextChanged, AddressOf textBoxToSearch_TextChanged
		End Sub

		' Text to search
		Private textBoxToSearch As TextBox

		' Find results
		Private matches As MatchCollection
		Private matchIndex As Integer = 0

		' Search results
        Private mIndex As Integer = 0
        Private mLength As Integer = 0

        Public Property Index() As Integer
            Get
                Return Me.mIndex
            End Get
            Set(ByVal value As Integer)
                Me.mIndex = value
            End Set
        End Property

        Public Property Length() As Integer
            Get
                Return Me.mLength
            End Get
            Set(ByVal value As Integer)
                Me.mLength = value
            End Set
        End Property

        Private Sub findNextButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)

            ' Find matches
            If Me.matches Is Nothing Then
                Dim pattern As String = Me.findWhatTextBox.Text

                ' Match whole word?
                If CBool(Me.matchWholeWordCheckBox.IsChecked) Then
                    pattern = "(?<=\W{0,1})" & pattern & "(?=\W)"
                End If

                ' Case sensitive
                If Not CBool(Me.caseSensitiveCheckBox.IsChecked) Then
                    pattern = "(?i)" & pattern
                End If

                ' Find matches
                Me.matches = Regex.Matches(Me.textBoxToSearch.Text, pattern)
                Me.matchIndex = 0

                ' Word not found?
                If Me.matches.Count = 0 Then
                    MessageBox.Show("'" & Me.findWhatTextBox.Text & "' not found.", "Find")
                    Me.matches = Nothing
                    Return
                End If
            End If

            ' Start at beginning of matches if the last find selected the last match
            If Me.matchIndex = Me.matches.Count Then
                Dim result As MessageBoxResult = MessageBox.Show("Nmore matches found. Start at beginning?", "Find", MessageBoxButton.YesNo)
                If result = MessageBoxResult.No Then
                    Return
                End If

                ' Reset
                Me.matchIndex = 0
            End If

            ' Return match details to client so it can select the text
            Dim match As Match = Me.matches(Me.matchIndex)
            If TextFoundEvent IsNot Nothing Then
                ' Text found
                Me.mIndex = match.Index
                Me.mLength = match.Length
                OnTextFound()
            End If
            Me.matchIndex += 1
        End Sub

		Private Sub textBoxToSearch_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			ResetFind()
		End Sub

		Private Sub findWhatTextBox_TextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
			ResetFind()
		End Sub

		Private Sub criteria_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ResetFind()
		End Sub

		Private Sub ResetFind()
			Me.findNextButton.IsEnabled = True
			Me.matches = Nothing
		End Sub

		Private Sub closeButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Close dialog box
			Me.Close()
		End Sub

	End Class
End Namespace
