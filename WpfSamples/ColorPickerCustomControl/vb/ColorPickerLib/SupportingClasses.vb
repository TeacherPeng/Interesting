' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

'
' SupportingClasses.cs 
'
' 
Imports System.Windows.Media.Animation
Imports System.Windows.Ink
Imports System.Windows.Markup

Namespace ColorPickerLib



	#Region "SpectrumSlider"

	Public Class SpectrumSlider
		Inherits Slider


		Shared Sub New()

			DefaultStyleKeyProperty.OverrideMetadata(GetType(SpectrumSlider), New FrameworkPropertyMetadata(GetType(SpectrumSlider)))
		End Sub


		#Region "Public Properties"
		Public Property SelectedColor() As Color
			Get

				Return CType(GetValue(SelectedColorProperty), Color)
			End Get
			Set(ByVal value As Color)
				SetValue(SelectedColorProperty, value)
			End Set

		End Property
		#End Region 


		#Region "Dependency Property Fields"
		Public Shared ReadOnly SelectedColorProperty As DependencyProperty = DependencyProperty.Register ("SelectedColor", GetType(Color), GetType(SpectrumSlider), New PropertyMetadata(Colors.Transparent))

		#End Region


		#Region "Public Methods"

		Public Overrides Sub OnApplyTemplate()

			MyBase.OnApplyTemplate()
			m_spectrumDisplay = TryCast(GetTemplateChild(SpectrumDisplayName), Rectangle)
			updateColorSpectrum()
			OnValueChanged(Double.NaN, Value)

		End Sub

		#End Region


		#Region "Protected Methods"
		Protected Overrides Sub OnValueChanged(ByVal oldValue As Double, ByVal newValue As Double)

			MyBase.OnValueChanged(oldValue, newValue)
			Dim theColor As Color = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1)
			SetValue(SelectedColorProperty, theColor)
		End Sub
		#End Region


		#Region "Private Methods"

		Private Sub updateColorSpectrum()
			If m_spectrumDisplay IsNot Nothing Then
				createSpectrum()
			End If
		End Sub



		Private Sub createSpectrum()

			pickerBrush = New LinearGradientBrush()
			pickerBrush.StartPoint = New Point(0.5, 0)
			pickerBrush.EndPoint = New Point(0.5, 1)
			pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation


			Dim colorsList As List(Of Color) = ColorUtilities.GenerateHsvSpectrum()
			Dim stopIncrement As Double = CDbl(1) / colorsList.Count

			Dim i As Integer
			For i = 0 To colorsList.Count - 1
				pickerBrush.GradientStops.Add(New GradientStop(colorsList(i), i * stopIncrement))
			Next i

			pickerBrush.GradientStops(i - 1).Offset = 1.0
			m_spectrumDisplay.Fill = pickerBrush

		End Sub
		#End Region


		#Region "Private Fields"
		Private Shared SpectrumDisplayName As String = "PART_SpectrumDisplay"
		Private m_spectrumDisplay As Rectangle
		Private pickerBrush As LinearGradientBrush
		#End Region

	End Class

	#End Region ' SpectrumSlider


	#Region "ColorUtilities"

	Friend NotInheritable Class ColorUtilities

		' Converts an RGB color to an HSV color.
		Private Sub New()
		End Sub
		Public Shared Function ConvertRgbToHsv(ByVal r As Integer, ByVal b As Integer, ByVal g As Integer) As HsvColor

			Dim delta, min As Double
			Dim h As Double = 0, s As Double, v As Double

			min = Math.Min(Math.Min(r, g), b)
			v = Math.Max(Math.Max(r, g), b)
			delta = v - min

			If v = 0.0 Then
				s = 0

			Else
				s = delta / v
			End If

			If s = 0 Then
				h = 0.0

			Else
				If r = v Then
					h = (g - b) / delta
				ElseIf g = v Then
					h = 2 + (b - r) / delta
				ElseIf b = v Then
					h = 4 + (r - g) / delta
				End If

				h *= 60
				If h < 0.0 Then
					h = h + 360
				End If

			End If

			Dim hsvColor As New HsvColor()
			hsvColor.H = h
			hsvColor.S = s
			hsvColor.V = v / 255

			Return hsvColor

		End Function

		' Converts an HSV color to an RGB color.
		Public Shared Function ConvertHsvToRgb(ByVal h As Double, ByVal s As Double, ByVal v As Double) As Color

			Dim r As Double = 0, g As Double = 0, b As Double = 0

			If s = 0 Then
				r = v
				g = v
				b = v
			Else
				Dim i As Integer
				Dim f, p, q, t As Double


				If h = 360 Then
					h = 0
				Else
					h = h / 60
				End If

				i = CInt(Fix(Math.Truncate(h)))
				f = h - i

				p = v * (1.0 - s)
				q = v * (1.0 - (s * f))
				t = v * (1.0 - (s * (1.0 - f)))

				Select Case i
					Case 0
						r = v
						g = t
						b = p

					Case 1
						r = q
						g = v
						b = p

					Case 2
						r = p
						g = v
						b = t

					Case 3
						r = p
						g = q
						b = v

					Case 4
						r = t
						g = p
						b = v

					Case Else
						r = v
						g = p
						b = q
				End Select

			End If



			Return Color.FromArgb(255, CByte(r * 255), CByte(g * 255), CByte(b * 255))

		End Function

		' Generates a list of colors with hues ranging from 0 360
		' and a saturation and value of 1. 
		Public Shared Function GenerateHsvSpectrum() As List(Of Color)

			Dim colorsList As New List(Of Color)(8)


			For i As Integer = 0 To 28

				colorsList.Add(ColorUtilities.ConvertHsvToRgb(i * 12, 1, 1))

			Next i
			colorsList.Add(ColorUtilities.ConvertHsvToRgb(0, 1, 1))


			Return colorsList

		End Function

	End Class

	#End Region ' ColorUtilities


	' Describes a color in terms of
	' Hue, Saturation, and Value (brightness)
	#Region "HsvColor"
	Friend Structure HsvColor

		Public H As Double
		Public S As Double
		Public V As Double

		Public Sub New(ByVal h As Double, ByVal s As Double, ByVal v As Double)
			Me.H = h
			Me.S = s
			Me.V = v

		End Sub
	End Structure
	#End Region ' HsvColor

	#Region "ColorThumb"
	Public Class ColorThumb
		Inherits System.Windows.Controls.Primitives.Thumb

		Shared Sub New()

			DefaultStyleKeyProperty.OverrideMetadata(GetType(ColorThumb), New FrameworkPropertyMetadata(GetType(ColorThumb)))
		End Sub


		Public Shared ReadOnly ThumbColorProperty As DependencyProperty = DependencyProperty.Register ("ThumbColor", GetType(Color), GetType(ColorThumb), New FrameworkPropertyMetadata(Colors.Transparent))

		Public Shared ReadOnly PointerOutlineThicknessProperty As DependencyProperty = DependencyProperty.Register ("PointerOutlineThickness", GetType(Double), GetType(ColorThumb), New FrameworkPropertyMetadata(1.0))

		Public Shared ReadOnly PointerOutlineBrushProperty As DependencyProperty = DependencyProperty.Register ("PointerOutlineBrush", GetType(Brush), GetType(ColorThumb), New FrameworkPropertyMetadata(Nothing))



		Public Property ThumbColor() As Color
			Get
				Return CType(GetValue(ThumbColorProperty), Color)
			End Get
			Set(ByVal value As Color)

				SetValue(ThumbColorProperty, value)
			End Set
		End Property

		Public Property PointerOutlineThickness() As Double
			Get
				Return CDbl(GetValue(PointerOutlineThicknessProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(PointerOutlineThicknessProperty, value)
			End Set
		End Property

		Public Property PointerOutlineBrush() As Brush
			Get
				Return CType(GetValue(PointerOutlineBrushProperty), Brush)
			End Get
			Set(ByVal value As Brush)
				SetValue(PointerOutlineBrushProperty, value)
			End Set
		End Property


	End Class
	#End Region ' ColorThumb


End Namespace