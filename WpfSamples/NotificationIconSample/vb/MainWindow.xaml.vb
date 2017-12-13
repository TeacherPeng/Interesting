Imports System.Text


Namespace NotificationIconSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Private notifyIcon As NotifyIcon

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' Configure and show a notification icon in the system tray
			Me.notifyIcon = New NotifyIcon()
			Me.notifyIcon.BalloonTipText = "Hello, NotifyIcon!"
			Me.notifyIcon.Text = "Hello, NotifyIcon!"
			Me.notifyIcon.Icon = New Icon("NotifyIcon.ico")
			Me.notifyIcon.Visible = True
			Me.notifyIcon.ShowBalloonTip(1000)
		End Sub
	End Class
End Namespace