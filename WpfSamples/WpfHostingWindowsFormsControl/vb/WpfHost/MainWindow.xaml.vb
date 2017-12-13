Imports System.Text
Imports MyControls

Namespace WpfHost
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

        Private app As System.Windows.Application
		Private myWindow As Window
		Private initFontWeight As FontWeight
		Private initFontSize As Double
		Private initFontStyle As FontStyle
		Private initBackBrush As SolidColorBrush
		Private initForeBrush As SolidColorBrush
		Private initFontFamily As FontFamily
		Private UIIsReady As Boolean = False

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Init(ByVal sender As Object, ByVal e As EventArgs)
            app = System.Windows.Application.Current
			myWindow = CType(app.MainWindow, Window)
			myWindow.SizeToContent = SizeToContent.WidthAndHeight
			wfh.TabIndex = 10
			initFontSize = wfh.FontSize
			initFontWeight = wfh.FontWeight
			initFontFamily = wfh.FontFamily
			initFontStyle = wfh.FontStyle
			initBackBrush = CType(wfh.Background, SolidColorBrush)
			initForeBrush = CType(wfh.Foreground, SolidColorBrush)
			AddHandler TryCast(wfh.Child, MyControl1).OnButtonClick, AddressOf Pane1_OnButtonClick
			UIIsReady = True
		End Sub

		Private Sub BackColorChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnBackGreen Then
				wfh.Background = New SolidColorBrush(Colors.LightGreen)
			ElseIf sender Is rdbtnBackSalmon Then
				wfh.Background = New SolidColorBrush(Colors.LightSalmon)
			ElseIf UIIsReady = True Then
				wfh.Background = initBackBrush
			End If
		End Sub

		Private Sub ForeColorChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnForeRed Then
				wfh.Foreground = New SolidColorBrush(Colors.Red)
			ElseIf sender Is rdbtnForeYellow Then
				wfh.Foreground = New SolidColorBrush(Colors.Yellow)
			ElseIf UIIsReady = True Then
				wfh.Foreground = initForeBrush
			End If
		End Sub

		Private Sub FontChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnTimes Then
				wfh.FontFamily = New FontFamily("Times New Roman")
			ElseIf sender Is rdbtnWingdings Then
				wfh.FontFamily = New FontFamily("Wingdings")
			ElseIf UIIsReady = True Then
				wfh.FontFamily = initFontFamily
			End If
		End Sub
		Private Sub FontSizeChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnTen Then
				wfh.FontSize = 10
			ElseIf sender Is rdbtnTwelve Then
				wfh.FontSize = 12
			ElseIf UIIsReady = True Then
				wfh.FontSize = initFontSize
			End If
		End Sub
		Private Sub StyleChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnItalic Then
				wfh.FontStyle = FontStyles.Italic
			ElseIf UIIsReady = True Then
				wfh.FontStyle = initFontStyle
			End If
		End Sub
		Private Sub WeightChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If sender Is rdbtnBold Then
				wfh.FontWeight = FontWeights.Bold
			ElseIf UIIsReady = True Then
				wfh.FontWeight = initFontWeight
			End If
		End Sub

		'Handle button clicks on the Windows Form control
		Private Sub Pane1_OnButtonClick(ByVal sender As Object, ByVal args As MyControlEventArgs)
			txtName.Inlines.Clear()
			txtAddress.Inlines.Clear()
			txtCity.Inlines.Clear()
			txtState.Inlines.Clear()
			txtZip.Inlines.Clear()

			If args.IsOK Then
				txtName.Inlines.Add(" " & args.MyName)
				txtAddress.Inlines.Add(" " & args.MyStreetAddress)
				txtCity.Inlines.Add(" " & args.MyCity)
				txtState.Inlines.Add(" " & args.MyState)
				txtZip.Inlines.Add(" " & args.MyZip)
			End If
		End Sub
	End Class
End Namespace
