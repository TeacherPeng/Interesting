Imports System.ComponentModel
Imports System.Text


Imports System.Windows.Forms.Integration
Imports MyControls

Namespace WFHost
	Partial Public Class Form1
		Inherits Form
		Private ctrlHost As ElementHost
		Private wpfAddressCtrl As MyControls.CustomControl1
        Private initFontWeight As System.Windows.FontWeight
        Private initFontSize As Double
        Private initFontStyle As System.Windows.FontStyle
        Private initBackBrush As System.Windows.Media.SolidColorBrush
        Private initForeBrush As System.Windows.Media.SolidColorBrush
        Private initFontFamily As System.Windows.Media.FontFamily

		Public Sub New()
			InitializeComponent()
		End Sub

Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			ctrlHost = New ElementHost()
			ctrlHost.Dock = DockStyle.Fill
			panel1.Controls.Add(ctrlHost)
			wpfAddressCtrl = New MyControls.CustomControl1()
			wpfAddressCtrl.InitializeComponent()
			ctrlHost.Child = wpfAddressCtrl

			AddHandler wpfAddressCtrl.OnButtonClick, AddressOf avAddressCtrl_OnButtonClick
			AddHandler wpfAddressCtrl.Loaded, AddressOf avAddressCtrl_Loaded
End Sub

		Private Sub avAddressCtrl_Loaded(ByVal sender As Object, ByVal e As EventArgs)
            initBackBrush = CType(wpfAddressCtrl.MyControl_Background, System.Windows.Media.SolidColorBrush)
			initForeBrush = wpfAddressCtrl.MyControl_Foreground
			initFontFamily = wpfAddressCtrl.MyControl_FontFamily
			initFontSize = wpfAddressCtrl.MyControl_FontSize
			initFontWeight = wpfAddressCtrl.MyControl_FontWeight
			initFontStyle = wpfAddressCtrl.MyControl_FontStyle
		End Sub

		Private Sub avAddressCtrl_OnButtonClick(ByVal sender As Object, ByVal args As MyControls.MyControlEventArgs)
			If args.IsOK Then
				lblAddress.Text = "Street Address: " & args.MyStreetAddress
				lblCity.Text = "City: " & args.MyCity
				lblName.Text = "Name: " & args.MyName
				lblState.Text = "State: " & args.MyState
				lblZip.Text = "Zip: " & args.MyZip
			Else
				lblAddress.Text = "Street Address: "
				lblCity.Text = "City: "
				lblName.Text = "Name: "
				lblState.Text = "State: "
				lblZip.Text = "Zip: "
			End If
		End Sub

		 Private Sub radioBackgroundOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioBackgroundOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_Background = initBackBrush
		 End Sub

		Private Sub radioBackgroundLightGreen_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioBackgroundLightGreen.CheckedChanged
            wpfAddressCtrl.MyControl_Background = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGreen)
		End Sub

		Private Sub radioBackgroundLightSalmon_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioBackgroundLightSalmon.CheckedChanged
            wpfAddressCtrl.MyControl_Background = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightSalmon)
		End Sub

		Private Sub radioForegroundOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioForegroundOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_Foreground = initForeBrush
		End Sub

		Private Sub radioForegroundRed_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioForegroundRed.CheckedChanged
            wpfAddressCtrl.MyControl_Foreground = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red)
		End Sub

		Private Sub radioForegroundYellow_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioForegroundYellow.CheckedChanged
            wpfAddressCtrl.MyControl_Foreground = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow)
		End Sub

		Private Sub radioFamilyOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioFamilyOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_FontFamily = initFontFamily
		End Sub

		Private Sub radioFamilyTimes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioFamilyTimes.CheckedChanged
            wpfAddressCtrl.MyControl_FontFamily = New System.Windows.Media.FontFamily("Times New Roman")
		End Sub

		Private Sub radioFamilyWingDings_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioFamilyWingDings.CheckedChanged
            wpfAddressCtrl.MyControl_FontFamily = New System.Windows.Media.FontFamily("WingDings")
		End Sub

		Private Sub radioSizeOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioSizeOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_FontSize = initFontSize
		End Sub

		Private Sub radioSizeTen_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioSizeTen.CheckedChanged
			wpfAddressCtrl.MyControl_FontSize = 10
		End Sub

		Private Sub radioSizeTwelve_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioSizeTwelve.CheckedChanged
			wpfAddressCtrl.MyControl_FontSize = 12
		End Sub

		Private Sub radioStyleOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioStyleOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_FontStyle = initFontStyle
		End Sub

		Private Sub radioStyleItalic_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioStyleItalic.CheckedChanged
            wpfAddressCtrl.MyControl_FontStyle = System.Windows.FontStyles.Italic
		End Sub

		Private Sub radioWeightOriginal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioWeightOriginal.CheckedChanged
			wpfAddressCtrl.MyControl_FontWeight = initFontWeight
		End Sub

		Private Sub radioWeightBold_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioWeightBold.CheckedChanged
            wpfAddressCtrl.MyControl_FontWeight = System.Windows.FontWeights.Bold
		End Sub

	End Class
End Namespace
