' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Animation


Namespace Microsoft.Samples.Animation.AnimationGallery


	Partial Public Class InteractiveAnimationsExample
		Inherits Page

		Private selectedTransition As AnimationTransitionType

		Public Sub New()

		End Sub


		' Computes the target point when the user clicks the Canvas
		' and starts the appropriate animation.
		Private Sub canvas_MouseLeftButtonDown(ByVal sender As Object, ByVal args As MouseButtonEventArgs)
			Dim targetPoint As Point = args.GetPosition(ContainerCanvas)
			targetPoint.X = targetPoint.X - (MyAnimatedObject.ActualWidth / 2)
			targetPoint.Y = targetPoint.Y - (MyAnimatedObject.ActualHeight / 2)


			Select Case selectedTransition
				Case AnimationTransitionType.Linear
				animateToDestinationUsingLinearAnimation(targetPoint)

				Case AnimationTransitionType.Bounce
				animateToDestinationUsingBounceAnimation(targetPoint)

				Case AnimationTransitionType.Elastic
				animateToDestinationUsingElasticAnimation(targetPoint)

			End Select
		End Sub

		' Animates to the target point using a standard
		' DoubleAnimation.
		Private Sub animateToDestinationUsingLinearAnimation(ByVal targetPoint As Point)
			Dim xAnimation As New DoubleAnimation()
			xAnimation.To = targetPoint.X
			xAnimation.Duration = New Duration(TimeSpan.FromSeconds(5))
			Dim yAnimation As New DoubleAnimation()
			yAnimation.To = targetPoint.Y
			yAnimation.Duration = New Duration(TimeSpan.FromSeconds(5))
			MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, xAnimation)
			MyAnimatedObject.BeginAnimation(Canvas.TopProperty, yAnimation)
		End Sub


		' Animates to the target point using a custom
		' BouncAnimation.
		Private Sub animateToDestinationUsingBounceAnimation(ByVal targetPoint As Point)

			Dim bounceXAnimation As New CustomAnimations.BounceDoubleAnimation()
			bounceXAnimation.From = Canvas.GetLeft(MyAnimatedObject)
			bounceXAnimation.To = targetPoint.X
			bounceXAnimation.Duration = TimeSpan.FromSeconds(5)
			bounceXAnimation.EdgeBehavior = CustomAnimations.BounceDoubleAnimation.EdgeBehaviorEnum.EaseIn
			MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, bounceXAnimation)

			Dim bounceYAnimation As New CustomAnimations.BounceDoubleAnimation()
			bounceYAnimation.From = Canvas.GetTop(MyAnimatedObject)
			bounceYAnimation.To = targetPoint.Y
			bounceYAnimation.Duration = TimeSpan.FromSeconds(5)
			bounceYAnimation.EdgeBehavior = CustomAnimations.BounceDoubleAnimation.EdgeBehaviorEnum.EaseIn
			MyAnimatedObject.BeginAnimation(Canvas.TopProperty, bounceYAnimation)

		End Sub

		' Animates to the target point using a custom
		' ElasticAnimation.        
		Private Sub animateToDestinationUsingElasticAnimation(ByVal targetPoint As Point)

			Dim elasticXAnimation As New CustomAnimations.ElasticDoubleAnimation()
			elasticXAnimation.From = Canvas.GetLeft(MyAnimatedObject)
			elasticXAnimation.To = targetPoint.X
			elasticXAnimation.Duration = TimeSpan.FromSeconds(5)
			elasticXAnimation.EdgeBehavior = CustomAnimations.ElasticDoubleAnimation.EdgeBehaviorEnum.EaseIn
			MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, elasticXAnimation)

			Dim elasticYAnimation As New CustomAnimations.ElasticDoubleAnimation()
			elasticYAnimation.From = Canvas.GetTop(MyAnimatedObject)
			elasticYAnimation.To = targetPoint.Y
			elasticYAnimation.Duration = TimeSpan.FromSeconds(5)
			elasticYAnimation.EdgeBehavior = CustomAnimations.ElasticDoubleAnimation.EdgeBehaviorEnum.EaseIn
			MyAnimatedObject.BeginAnimation(Canvas.TopProperty, elasticYAnimation)

		End Sub

		' Sets the default animation transition mode.
		Private Sub pageLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)

			LinearTransitionRadioButton.IsChecked = True
		End Sub

		' Updates the cached animation transition.
		Private Sub selectedTransitionChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)
			Dim value As String = CType((CType(args.Source, RadioButton)).Content, String)
			selectedTransition = CType(System.Enum.Parse(GetType(AnimationTransitionType), value), AnimationTransitionType)

		End Sub


	End Class

	Public Enum AnimationTransitionType
		Linear
		Bounce
		Elastic

	End Enum
End Namespace
