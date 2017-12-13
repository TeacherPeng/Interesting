Namespace LocalizingWpfInWf
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Me.label1 = New Label()
            Me.elementHost1 = New Integration.ElementHost()
			Me.SuspendLayout()
			' 
			' label1
			' 
			resources.ApplyResources(Me.label1, "label1")
			Me.label1.Name = "label1"
			' 
			' elementHost1
			' 
			resources.ApplyResources(Me.elementHost1, "elementHost1")
			Me.elementHost1.Name = "elementHost1"
			Me.elementHost1.TabStop = False
			Me.elementHost1.Child = Nothing
			' 
			' Form1
			' 
			resources.ApplyResources(Me, "$this")
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.elementHost1)
			Me.Controls.Add(Me.label1)
			Me.Name = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load)
			Me.ResumeLayout(False)

		End Sub

		#End Region

        Private label1 As Label
        Private elementHost1 As Integration.ElementHost
	End Class
End Namespace

