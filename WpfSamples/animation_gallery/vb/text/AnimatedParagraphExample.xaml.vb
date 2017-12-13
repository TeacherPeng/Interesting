' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Animation


Namespace Microsoft.Samples.Animation.AnimationGallery


	Partial Public Class AnimatedParagraphExample
		Inherits Page


		Public Sub New()


		End Sub


		' Updates the center of the RotateTransform used to rotate
		' the TextBlock's characters.
		Private Sub textBlockSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
			If args IsNot Nothing AndAlso (Not args.NewSize.IsEmpty) Then

				TextEffectRotateTransform.CenterX = args.NewSize.Width \ 2
				TextEffectRotateTransform.CenterY = args.NewSize.Height \ 2

			End If

		End Sub


	End Class


End Namespace
