' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Animation

Namespace Microsoft.Samples.Animation.AnimationGallery.CustomAnimations
	''' <summary>
	''' BounceDoubleAnimation
	''' </summary>
	Public Class BounceDoubleAnimation
		Inherits DoubleAnimationBase
		Public Enum EdgeBehaviorEnum
			EaseIn
			EaseOut
			EaseInOut
		End Enum

		Public Shared ReadOnly EdgeBehaviorProperty As DependencyProperty = DependencyProperty.Register("EdgeBehavior", GetType(EdgeBehaviorEnum), GetType(BounceDoubleAnimation), New PropertyMetadata(EdgeBehaviorEnum.EaseInOut))

		Public Shared ReadOnly BouncesProperty As DependencyProperty = DependencyProperty.Register("Bounces", GetType(Integer), GetType(BounceDoubleAnimation), New PropertyMetadata(5))

		Public Shared ReadOnly BouncinessProperty As DependencyProperty = DependencyProperty.Register("Bounciness", GetType(Double), GetType(BounceDoubleAnimation), New PropertyMetadata(3.0))

		Public Shared ReadOnly FromProperty As DependencyProperty = DependencyProperty.Register("From", GetType(Double?), GetType(BounceDoubleAnimation), New PropertyMetadata(Nothing))

		Public Shared ReadOnly ToProperty As DependencyProperty = DependencyProperty.Register("To", GetType(Double?), GetType(BounceDoubleAnimation), New PropertyMetadata(Nothing))




		''' <summary>
		''' Specifies which side of the transition gets the "bounce" effect.
		''' </summary>
		Public Property EdgeBehavior() As EdgeBehaviorEnum
			Get
				Return CType(GetValue(EdgeBehaviorProperty), EdgeBehaviorEnum)
			End Get
			Set(ByVal value As EdgeBehaviorEnum)
				SetValue(EdgeBehaviorProperty, value)
			End Set
		End Property
		''' <summary>
		'''  Number of bounces in the effect
		''' </summary>
		Public Property Bounces() As Integer
			Get
				Return CInt(Fix(GetValue(BouncesProperty)))
			End Get
			Set(ByVal value As Integer)
				If value > 0 Then
					SetValue(BouncesProperty, value)
				Else
					Throw New ArgumentException("can't set the bounces to " & value)
				End If
			End Set
		End Property

		''' <summary>
		''' Specifies the amount by which the element springs back.
		''' </summary>
		Public Property Bounciness() As Double
			Get
				Return CDbl(GetValue(BouncinessProperty))
			End Get
			Set(ByVal value As Double)
				If value > 0 Then
					SetValue(BouncinessProperty, value)
				Else
					Throw New ArgumentException("can't set the bounciness to " & value)
				End If
			End Set
		End Property

		''' <summary>
		''' Specifies the starting value of the animation.
		''' </summary>
		Public Property [From]() As Double?
			Get
				Return CType(GetValue(FromProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(FromProperty, value)

			End Set
		End Property

		''' <summary>
		''' Specifies the ending value of the animation.
		''' </summary>
		Public Property [To]() As Double?
			Get
				Return CType(GetValue(ToProperty), Double?)
			End Get
			Set(ByVal value? As Double)
				SetValue(ToProperty, value)
			End Set
		End Property

		Protected Overrides Function GetCurrentValueCore(ByVal defaultOriginValue As Double, ByVal defaultDestinationValue As Double, ByVal clock As AnimationClock) As Double
			Dim returnValue As Double
            Dim start As Double
            Dim delta As Double

            If ([From] IsNot Nothing) Then
                start = CType([From], Double)
            Else
                start = defaultOriginValue
            End If

            If ([To] IsNot Nothing) Then
                delta = CType([To], Double) - start
            Else
                delta = defaultOriginValue - start
            End If

            Select Case Me.EdgeBehavior
                Case EdgeBehaviorEnum.EaseIn
                    returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Bounciness, Bounces)
                Case EdgeBehaviorEnum.EaseOut
                    returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Bounciness, Bounces)
                Case Else
                    returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Bounciness, Bounces)
            End Select
            Return returnValue
        End Function

		Protected Overrides Function CreateInstanceCore() As Freezable

			Return New BounceDoubleAnimation()
		End Function

		Private Shared Function easeOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal bounciness As Double, ByVal bounces As Integer) As Double
			Dim returnValue As Double = 0.0

			' math magic: The cosine gives us the right wave, the timeFraction is the frequency of the wave, 
			' the absolute value keeps every value positive (so it "bounces" off the midpoint of the cosine 
			' wave, and the amplitude (the exponent) makes the sine wave get smaller and smaller at the end.
			returnValue = Math.Abs(Math.Pow((1 - timeFraction), bounciness) * Math.Cos(2 * Math.PI * timeFraction * bounces))
			returnValue = delta - (returnValue * delta)
			returnValue += start
			Return returnValue

		End Function
		Private Shared Function easeIn(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal bounciness As Double, ByVal bounces As Integer) As Double
			Dim returnValue As Double = 0.0
			' math magic: The cosine gives us the right wave, the timeFraction is the amplitude of the wave, 
			' the absolute value keeps every value positive (so it "bounces" off the midpoint of the cosine 
			' wave, and the amplitude (the exponent) makes the sine wave get bigger and bigger towards the end.
			returnValue = Math.Abs(Math.Pow((timeFraction), bounciness) * Math.Cos(2 * Math.PI * timeFraction * bounces))
			returnValue = returnValue * delta
			returnValue += start
			Return returnValue
		End Function
		Private Shared Function easeInOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal bounciness As Double, ByVal bounces As Integer) As Double
			Dim returnValue As Double = 0.0

			' we cut each effect in half by multiplying the time fraction by two and halving the distance.
			If timeFraction <= 0.5 Then
				returnValue = easeIn(timeFraction * 2, start, delta / 2, bounciness, bounces)
			Else
				returnValue = easeOut((timeFraction - 0.5) * 2, start, delta / 2, bounciness, bounces)
				returnValue += delta / 2
			End If
			Return returnValue
		End Function
	End Class

	''' <summary>
	''' ElasticDoubleAnimation - like something attached to a rubber band
	''' </summary>
	Public Class ElasticDoubleAnimation
		Inherits DoubleAnimationBase
		Public Enum EdgeBehaviorEnum
			EaseIn
			EaseOut
			EaseInOut
		End Enum

		Public Shared ReadOnly EdgeBehaviorProperty As DependencyProperty = DependencyProperty.Register("EdgeBehavior", GetType(EdgeBehaviorEnum), GetType(ElasticDoubleAnimation), New PropertyMetadata(EdgeBehaviorEnum.EaseIn))

		Public Shared ReadOnly SpringinessProperty As DependencyProperty = DependencyProperty.Register("Springiness", GetType(Double), GetType(ElasticDoubleAnimation), New PropertyMetadata(3.0))

		Public Shared ReadOnly OscillationsProperty As DependencyProperty = DependencyProperty.Register("Oscillations", GetType(Double), GetType(ElasticDoubleAnimation), New PropertyMetadata(10.0))

		Public Shared ReadOnly FromProperty As DependencyProperty = DependencyProperty.Register("From", GetType(Double?), GetType(ElasticDoubleAnimation), New PropertyMetadata(Nothing))

		Public Shared ReadOnly ToProperty As DependencyProperty = DependencyProperty.Register("To", GetType(Double?), GetType(ElasticDoubleAnimation), New PropertyMetadata(Nothing))

		''' <summary>
		''' which side gets the effect
		''' </summary>
		Public Property EdgeBehavior() As EdgeBehaviorEnum
			Get
				Return CType(GetValue(EdgeBehaviorProperty), EdgeBehaviorEnum)
			End Get
			Set(ByVal value As EdgeBehaviorEnum)
				SetValue(EdgeBehaviorProperty, value)
			End Set
		End Property

		''' <summary>
		''' how much springiness is there in the effect
		''' </summary>
		Public Property Springiness() As Double
			Get
				Return CDbl(GetValue(SpringinessProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(SpringinessProperty, value)
			End Set
		End Property

		''' <summary>
		''' number of oscillations in the effect
		''' </summary>
		Public Property Oscillations() As Double
			Get
				Return CDbl(GetValue(OscillationsProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(OscillationsProperty, value)
			End Set
		End Property


		''' <summary>
		''' Specifies the starting value of the animation.
		''' </summary>
		Public Property [From]() As Double?
			Get
				Return CType(GetValue(FromProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(FromProperty, value)

			End Set
		End Property

		''' <summary>
		''' Specifies the ending value of the animation.
		''' </summary>
		Public Property [To]() As Double?
			Get
				Return CType(GetValue(ToProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(ToProperty, value)

			End Set
		End Property


		Protected Overrides Function GetCurrentValueCore(ByVal defaultOriginValue As Double, ByVal defaultDestinationValue As Double, ByVal clock As AnimationClock) As Double
			Dim returnValue As Double
            Dim start As Double
            Dim delta As Double

            If ([From] IsNot Nothing) Then
                start = CType([From], Double)
            Else
                start = defaultOriginValue
            End If

            If ([To] IsNot Nothing) Then
                delta = CType([To], Double) - start
            Else
                delta = defaultOriginValue - start
            End If

			Select Case Me.EdgeBehavior
				Case EdgeBehaviorEnum.EaseIn
					returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations)
				Case EdgeBehaviorEnum.EaseOut
					returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations)
				Case Else
					returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations)
			End Select
			Return returnValue
		End Function

		Protected Overrides Function CreateInstanceCore() As Freezable
			Return New ElasticDoubleAnimation()
		End Function


		Private Shared Function easeOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal springiness As Double, ByVal oscillations As Double) As Double
			Dim returnValue As Double = 0.0

			' math magic: The cosine gives us the right wave, the timeFraction * the # of oscillations is the 
			' frequency of the wave, and the amplitude (the exponent) makes the wave get smaller at the end
			' by the "springiness" factor. This is extremely similar to the bounce equation.
			returnValue = Math.Pow((1 - timeFraction), springiness) * Math.Cos(2 * Math.PI * timeFraction * oscillations)
			returnValue = delta - (returnValue * delta)
			returnValue += start
			Return returnValue
		End Function
		Private Shared Function easeIn(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal springiness As Double, ByVal oscillations As Double) As Double
			Dim returnValue As Double = 0.0
			' math magic: The cosine gives us the right wave, the timeFraction * the # of oscillations is the 
			' frequency of the wave, and the amplitude (the exponent) makes the wave get smaller at the beginning
			' by the "springiness" factor. This is extremely similar to the bounce equation. 
			returnValue = Math.Pow((timeFraction), springiness) * Math.Cos(2 * Math.PI * timeFraction * oscillations)
			returnValue = returnValue * delta
			returnValue += start
			Return returnValue
		End Function
		Private Shared Function easeInOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal springiness As Double, ByVal oscillations As Double) As Double
			Dim returnValue As Double = 0.0

			' we cut each effect in half by multiplying the time fraction by two and halving the distance.
			If timeFraction <= 0.5 Then
				Return easeIn(timeFraction * 2, start, delta / 2, springiness, oscillations)
			Else
				returnValue = easeOut((timeFraction - 0.5) * 2, start, delta / 2, springiness, oscillations)
				returnValue += (delta / 2)
			End If
			Return returnValue
		End Function
	End Class





	''' <summary>
	''' BackDoubleAnimation: goes in the opposite direction first
	''' </summary>
	Public Class BackDoubleAnimation
		Inherits DoubleAnimationBase
		Public Enum EdgeBehaviorEnum
			EaseIn
			EaseOut
			EaseInOut
		End Enum

		Public Shared ReadOnly EdgeBehaviorProperty As DependencyProperty = DependencyProperty.Register("EdgeBehavior", GetType(EdgeBehaviorEnum), GetType(BackDoubleAnimation), New PropertyMetadata(EdgeBehaviorEnum.EaseIn))

		Public Shared ReadOnly AmplitudeProperty As DependencyProperty = DependencyProperty.Register("Amplitude", GetType(Double), GetType(BackDoubleAnimation), New PropertyMetadata(4.0))

		Public Shared ReadOnly SuppressionProperty As DependencyProperty = DependencyProperty.Register("Suppression", GetType(Double), GetType(BackDoubleAnimation), New PropertyMetadata(2.0))

		Public Shared ReadOnly FromProperty As DependencyProperty = DependencyProperty.Register("From", GetType(Double?), GetType(BackDoubleAnimation), New PropertyMetadata(Nothing))

		Public Shared ReadOnly ToProperty As DependencyProperty = DependencyProperty.Register("To", GetType(Double?), GetType(BackDoubleAnimation), New PropertyMetadata(Nothing))



		''' <summary>
		''' which side gets the effect
		''' </summary>
		Public Property EdgeBehavior() As EdgeBehaviorEnum
			Get
				Return CType(GetValue(EdgeBehaviorProperty), EdgeBehaviorEnum)
			End Get
			Set(ByVal value As EdgeBehaviorEnum)
				SetValue(EdgeBehaviorProperty, value)
			End Set
		End Property

		''' <summary>
		''' how much backwards motion is there in the effect
		''' </summary>
		Public Property Amplitude() As Double
			Get
				Return CDbl(GetValue(AmplitudeProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(AmplitudeProperty, value)
			End Set
		End Property

		''' <summary>
		''' how quickly the effect drops off vs. the entire timeline
		''' </summary>
		Public Property Suppression() As Double
			Get
				Return CDbl(GetValue(SuppressionProperty))
			End Get
			Set(ByVal value As Double)
				SetValue(SuppressionProperty, value)
			End Set
		End Property

		''' <summary>
		''' Specifies the starting value of the animation.
		''' </summary>
		Public Property [From]() As Double?
			Get
				Return CType(GetValue(FromProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(FromProperty, value)

			End Set
		End Property

		''' <summary>
		''' Specifies the ending value of the animation.
		''' </summary>
		Public Property [To]() As Double?
			Get
				Return CType(GetValue(ToProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(ToProperty, value)

			End Set
		End Property


		Protected Overrides Function GetCurrentValueCore(ByVal defaultOriginValue As Double, ByVal defaultDestinationValue As Double, ByVal clock As AnimationClock) As Double
			Dim returnValue As Double
            Dim start As Double
            Dim delta As Double

            If ([From] IsNot Nothing) Then
                start = CType([From], Double)
            Else
                start = defaultOriginValue
            End If

            If ([To] IsNot Nothing) Then
                delta = CType([To], Double) - start
            Else
                delta = defaultOriginValue - start
            End If

			Select Case Me.EdgeBehavior
				Case EdgeBehaviorEnum.EaseIn
					returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression)
				Case EdgeBehaviorEnum.EaseOut
					returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression)
				Case Else
					returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression)
			End Select
			Return returnValue
		End Function

		Protected Overrides Function CreateInstanceCore() As Freezable

			Return New BackDoubleAnimation()
		End Function


		Private Shared Function easeOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal amplitude As Double, ByVal suppression As Double) As Double
			Dim returnValue As Double = 0.0
			Dim frequency As Double = 0.5

			' math magic: The sine gives us the right wave, the timeFraction * 0.5 (frequency) gives us only half 
			' of the full wave, the amplitude gives us the relative height of the peak, and the exponent makes the 
			' effect drop off more quickly by the "suppression" factor. 
			returnValue = Math.Pow((timeFraction), suppression) * amplitude * Math.Sin(2 * Math.PI * timeFraction * frequency) + timeFraction
			returnValue = (returnValue * delta)
			returnValue += start
			Return returnValue
		End Function

		Private Shared Function easeIn(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal amplitude As Double, ByVal suppression As Double) As Double
			Dim returnValue As Double = 0.0
			Dim frequency As Double = 0.5

			' math magic: The sine gives us the right wave, the timeFraction * 0.5 (frequency) gives us only half 
			' of the full wave (flipped by multiplying by -1 so that we go "backwards" first), the amplitude gives 
			' us the relative height of the peak, and the exponent makes the effect start later by the "suppression" 
			' factor. 
			returnValue = Math.Pow((1 - timeFraction), suppression) * amplitude * Math.Sin(2 * Math.PI * timeFraction * frequency) * -1 + timeFraction
			returnValue = (returnValue * delta)
			returnValue += start
			Return returnValue
		End Function

		Private Shared Function easeInOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal amplitude As Double, ByVal suppression As Double) As Double
			Dim returnValue As Double = 0.0

			' we cut each effect in half by multiplying the time fraction by two and halving the distance.
			If timeFraction <= 0.5 Then
				Return easeIn(timeFraction * 2, start, delta / 2, amplitude, suppression)
			Else
				returnValue = easeOut((timeFraction - 0.5) * 2, start, delta / 2, amplitude, suppression)
				returnValue += (delta / 2)
			End If
			Return returnValue
		End Function
	End Class







	''' <summary>
	''' ExponentialDoubleAnimation - gets exponentially faster / slower
	''' </summary>
	Public Class ExponentialDoubleAnimation
		Inherits DoubleAnimationBase
		Public Enum EdgeBehaviorEnum
			EaseIn
			EaseOut
			EaseInOut
		End Enum

		Public Shared ReadOnly EdgeBehaviorProperty As DependencyProperty = DependencyProperty.Register("EdgeBehavior", GetType(EdgeBehaviorEnum), GetType(ExponentialDoubleAnimation), New PropertyMetadata(EdgeBehaviorEnum.EaseIn))

		Public Shared ReadOnly PowerProperty As DependencyProperty = DependencyProperty.Register("Power", GetType(Double), GetType(ExponentialDoubleAnimation), New PropertyMetadata(2.0))


		Public Shared ReadOnly FromProperty As DependencyProperty = DependencyProperty.Register("From", GetType(Double?), GetType(ExponentialDoubleAnimation), New PropertyMetadata(Nothing))

		Public Shared ReadOnly ToProperty As DependencyProperty = DependencyProperty.Register("To", GetType(Double?), GetType(ExponentialDoubleAnimation), New PropertyMetadata(Nothing))


		''' <summary>
		''' which side gets the effect
		''' </summary>
		Public Property EdgeBehavior() As EdgeBehaviorEnum
			Get
				Return CType(GetValue(EdgeBehaviorProperty), EdgeBehaviorEnum)
			End Get
			Set(ByVal value As EdgeBehaviorEnum)
				SetValue(EdgeBehaviorProperty, value)
			End Set
		End Property

		''' <summary>
		''' exponential rate of growth
		''' </summary>
		Public Property Power() As Double
			Get
				Return CDbl(GetValue(PowerProperty))
			End Get
			Set(ByVal value As Double)
				If value > 0.0 Then
					SetValue(PowerProperty, value)
				Else
					Throw New ArgumentException("cannot set power to less than 0.0. Value: " & value)
				End If
			End Set
		End Property

		''' <summary>
		''' Specifies the starting value of the animation.
		''' </summary>
		Public Property [From]() As Double?
			Get
				Return CType(GetValue(FromProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(FromProperty, value)

			End Set
		End Property

		''' <summary>
		''' Specifies the ending value of the animation.
		''' </summary>
		Public Property [To]() As Double?
			Get
				Return CType(GetValue(ToProperty), Double?)
			End Get
			Set(ByVal value? As Double)

				SetValue(ToProperty, value)

			End Set
		End Property



		Protected Overrides Function GetCurrentValueCore(ByVal defaultOriginValue As Double, ByVal defaultDestinationValue As Double, ByVal clock As AnimationClock) As Double
			Dim returnValue As Double
            Dim start As Double
            Dim delta As Double

            If ([From] IsNot Nothing) Then
                start = CType([From], Double)
            Else
                start = defaultOriginValue
            End If

            If ([To] IsNot Nothing) Then
                delta = CType([To], Double) - start
            Else
                delta = defaultOriginValue - start
            End If

			Select Case Me.EdgeBehavior
				Case EdgeBehaviorEnum.EaseIn
					returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Power)
				Case EdgeBehaviorEnum.EaseOut
					returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Power)
				Case Else
					returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Power)
			End Select
			Return returnValue
		End Function

		Protected Overrides Function CreateInstanceCore() As Freezable
			Return New ExponentialDoubleAnimation()
		End Function

		Private Shared Function easeIn(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal power As Double) As Double
			Dim returnValue As Double = 0.0

			' math magic: simple exponential growth
			returnValue = Math.Pow(timeFraction, power)
			returnValue *= delta
			returnValue = returnValue + start
			Return returnValue
		End Function
		Private Shared Function easeOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal power As Double) As Double
			Dim returnValue As Double = 0.0

			' math magic: simple exponential decay
			returnValue = Math.Pow(timeFraction, 1 / power)
			returnValue *= delta
			returnValue = returnValue + start
			Return returnValue
		End Function
		Private Shared Function easeInOut(ByVal timeFraction As Double, ByVal start As Double, ByVal delta As Double, ByVal power As Double) As Double
			Dim returnValue As Double = 0.0

			' we cut each effect in half by multiplying the time fraction by two and halving the distance.
			If timeFraction <= 0.5 Then
				returnValue = easeOut(timeFraction * 2, start, delta / 2, power)
			Else
				returnValue = easeIn((timeFraction - 0.5) * 2, start, delta / 2, power)
				returnValue += (delta / 2)
			End If
			Return returnValue
		End Function
	End Class



	''' <summary>
	''' CircleAnimation: calculates polar coordinates as a function of time. 
	''' Use two of these (XDirection and YDirection) to move an element in an elliptical manner
	''' </summary>
	Public Class CircleAnimation
		Inherits DoubleAnimationBase
		Public Enum DirectionEnum
			XDirection
			YDirection
		End Enum

		Public Shared ReadOnly DirectionProperty As DependencyProperty = DependencyProperty.Register("Direction", GetType(DirectionEnum), GetType(CircleAnimation), New PropertyMetadata(DirectionEnum.XDirection))

		Public Shared ReadOnly RadiusProperty As DependencyProperty = DependencyProperty.Register("Radius", GetType(Double), GetType(CircleAnimation), New PropertyMetadata(CDbl(10)))


		''' <summary>
		''' distance from origin to polar coordinate
		''' </summary>
		Public Property Radius() As Double
			Get
				Return CDbl(GetValue(RadiusProperty))
			End Get
			Set(ByVal value As Double)
				If value > 0.0 Then
					SetValue(RadiusProperty, value)
				Else
					Throw New ArgumentException("a radius of " & value & " is not allowed!")
				End If
			End Set
		End Property

		''' <summary>
		''' are we measuring in the X or Y direction?
		''' </summary>
		Public Property Direction() As DirectionEnum
			Get
				Return CType(GetValue(DirectionProperty), DirectionEnum)
			End Get
			Set(ByVal value As DirectionEnum)
				SetValue(DirectionProperty, value)
			End Set
		End Property




		Protected Overrides Function GetCurrentValueCore(ByVal defaultOriginValue As Double, ByVal defaultDestinationValue As Double, ByVal clock As AnimationClock) As Double
			Dim returnValue As Double
			Dim time As Double = clock.CurrentProgress.Value

			' math magic: calculate new coordinates using polar coordinate equations. This requires two 
			' animations to be wired up in order to move in a circle, since we don't make any assumptions
			' about what we're animating (e.g. a TranslateTransform). 
			If Direction = DirectionEnum.XDirection Then
				returnValue = Math.Cos(2 * Math.PI * time)
			Else
				returnValue = Math.Sin(2 * Math.PI * time)
			End If

			' Need to add the defaultOriginValue so that composition works.
			Return returnValue * Radius + defaultOriginValue
		End Function

		Protected Overrides Function CreateInstanceCore() As Freezable
			Return New CircleAnimation()
		End Function

	End Class
End Namespace