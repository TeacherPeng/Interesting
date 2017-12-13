' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO

Namespace Microsoft.Samples.Animation.AnimationGallery
	''' <summary>
	''' Interaction logic for GridSampleViewer.xaml
	''' </summary>
	Partial Public Class GridSampleViewer
		Inherits Page
		Private sampleGridOpacityAnimation As DoubleAnimation
		Private sampleGridTranslateTransformAnimation As DoubleAnimation
		Private borderTranslateDoubleAnimation As DoubleAnimation

		Public Sub New()
			InitializeComponent()

			Dim widthBinding As New Binding("ActualWidth")
			widthBinding.Source = Me

			sampleGridOpacityAnimation = New DoubleAnimation()
			sampleGridOpacityAnimation.To = 0
			sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(1))

			sampleGridTranslateTransformAnimation = New DoubleAnimation()
			sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(1)

			BindingOperations.SetBinding(sampleGridTranslateTransformAnimation, DoubleAnimation.ToProperty, widthBinding)
			sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(1))

			borderTranslateDoubleAnimation = New DoubleAnimation()
			borderTranslateDoubleAnimation.To = 0
			borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(1))
			borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(2)
			borderTranslateDoubleAnimation.FillBehavior = FillBehavior.HoldEnd
			BindingOperations.SetBinding(borderTranslateDoubleAnimation, DoubleAnimation.FromProperty, widthBinding)


		End Sub
		Private Shared _packUri As New Uri("pack://application:,,,/")

		Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)

			If TypeOf args.Source Is RadioButton Then

				Dim theButton As RadioButton = CType(args.Source, RadioButton)
				Dim theFrame As Frame = CType(theButton.Content, Frame)

				If theFrame.HasContent Then

					Dim source As Uri = theFrame.CurrentSource
					If (source IsNot Nothing) AndAlso (Not source.IsAbsoluteUri) Then
						source = New Uri(_packUri, source)
					End If
					SampleDisplayFrame.Source = source

					SampleDisplayBorder.Visibility = Visibility.Visible

				End If

			End If

		End Sub

		Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)

			SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
			SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
			SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
			SampleDisplayBorder.Visibility = Visibility.Visible

		End Sub


		Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)

			SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
			SampleDisplayBorder.Visibility = Visibility.Hidden
		End Sub



		Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)

			SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
		End Sub

	End Class

End Namespace
