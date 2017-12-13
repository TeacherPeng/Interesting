Imports System.Text


Namespace WpfLayoutHostingWfWithXaml
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()

			Me.InitializeFlowLayoutPanel()
		End Sub

		Private Sub button1Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim b As System.Windows.Forms.Button = TryCast(sender, System.Windows.Forms.Button)

			b.Top = 20
			b.Left = 20
		End Sub

		Private Sub button2Click(ByVal sender As Object, ByVal e As EventArgs)
			Me.host1.Visibility = Visibility.Hidden
		End Sub

		Private Sub button3Click(ByVal sender As Object, ByVal e As EventArgs)
			Me.host1.Visibility = Visibility.Collapsed
		End Sub

		Private Sub InitializeFlowLayoutPanel()
			Dim flp As FlowLayoutPanel = TryCast(Me.flowLayoutHost.Child, FlowLayoutPanel)

			flp.WrapContents = True

			Const numButtons As Integer = 6

			For i As Integer = 0 To numButtons - 1
                Dim b As New System.Windows.Forms.Button()
				b.Text = "Button"
                b.BackColor = System.Drawing.Color.AliceBlue
				b.FlatStyle = FlatStyle.Flat

				flp.Controls.Add(b)
			Next i
		End Sub
	End Class
End Namespace
