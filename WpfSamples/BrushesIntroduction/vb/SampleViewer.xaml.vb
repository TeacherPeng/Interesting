' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.Reflection
Imports System.Xml

Namespace BrushesIntroduction
	''' <summary>
	''' Interaction logic for SampleViewer.xaml
	''' </summary>
	Partial Public Class SampleViewer
		Inherits Page
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub transitionAnimationStateChanged(ByVal sender As Object, ByVal args As EventArgs)
			Dim transitionAnimationClock As AnimationClock = CType(sender, AnimationClock)

			If transitionAnimationClock.CurrentState = ClockState.Filling Then
				fadeEnded()
			End If
		End Sub


		Private Sub myFrameNavigated(ByVal sender As Object, ByVal args As NavigationEventArgs)
			Dim myFadeInAnimation As DoubleAnimation = CType(Me.Resources("MyFadeInAnimationResource"), DoubleAnimation)
			myFrame.BeginAnimation(Frame.OpacityProperty, myFadeInAnimation, HandoffBehavior.SnapshotAndReplace)
		End Sub

        Private Sub fadeEnded()
            Dim el As XmlElement = CType(myPageList.SelectedItem, XmlElement)
            Dim att As XmlAttribute = el.Attributes("Uri")
            If att IsNot Nothing Then
                myFrame.Navigate(New Uri(att.Value, UriKind.Relative))
            Else
                myFrame.Content = Nothing
            End If
        End Sub

		Public Shared ExitCommand As New RoutedUICommand("Exit", "Exit", GetType(SampleViewer))

		Private Sub executeExitCommand(ByVal sender As Object, ByVal e As ExecutedRoutedEventArgs)
			Application.Current.Shutdown()
        End Sub
    End Class
End Namespace