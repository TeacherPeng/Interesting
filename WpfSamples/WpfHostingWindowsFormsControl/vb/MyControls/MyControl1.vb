Imports System.Collections
Imports System.ComponentModel

Namespace MyControls
	''' <summary>
	''' Summary description for UserControl1.
	''' </summary>
	Public Class MyControl1
		Inherits UserControl
		Private label1 As Label
		Private label2 As Label
		Private label3 As Label
		Private label4 As Label
		Private label5 As Label
		Private txtName As TextBox
		Private txtAddress As TextBox
		Private txtCity As TextBox
		Private txtState As TextBox
		Private txtZip As TextBox
		Private label6 As Label
		Private WithEvents btnOK As Button
		Private WithEvents btnCancel As Button
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		Public Sub New()
			' This call is required by the Windows.Forms Form Designer.
			InitializeComponent()

			' TODO: Add any initialization after the InitComponent call

		End Sub

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.label1 = New Label()
			Me.label2 = New Label()
			Me.label3 = New Label()
			Me.label4 = New Label()
			Me.label5 = New Label()
			Me.txtName = New TextBox()
			Me.txtAddress = New TextBox()
			Me.txtCity = New TextBox()
			Me.txtState = New TextBox()
			Me.txtZip = New TextBox()
			Me.btnOK = New Button()
			Me.btnCancel = New Button()
			Me.label6 = New Label()
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.Location = New Point(20, 46)
			Me.label1.Margin = New Padding(3, 3, 3, 1)
			Me.label1.Name = "label1"
			Me.label1.Size = New Size(65, 16)
			Me.label1.TabIndex = 8
			Me.label1.Text = "Name"
			Me.label1.TextAlign = ContentAlignment.MiddleLeft
			' 
			' label2
			' 
			Me.label2.Location = New Point(20, 88)
			Me.label2.Name = "label2"
			Me.label2.Size = New Size(94, 13)
			Me.label2.TabIndex = 9
			Me.label2.Text = "Street Address"
			Me.label2.TextAlign = ContentAlignment.MiddleLeft
			' 
			' label3
			' 
			Me.label3.Location = New Point(20, 127)
			Me.label3.Margin = New Padding(3, 3, 3, 2)
			Me.label3.Name = "label3"
			Me.label3.Size = New Size(49, 13)
			Me.label3.TabIndex = 10
			Me.label3.Text = "City"
			Me.label3.TextAlign = ContentAlignment.MiddleLeft
			' 
			' label4
			' 
			Me.label4.Location = New Point(246, 127)
			Me.label4.Margin = New Padding(3, 0, 3, 3)
			Me.label4.Name = "label4"
			Me.label4.Size = New Size(47, 13)
			Me.label4.TabIndex = 11
			Me.label4.Text = "State"
			Me.label4.TextAlign = ContentAlignment.MiddleLeft
			' 
			' label5
			' 
			Me.label5.Location = New Point(23, 167)
			Me.label5.Margin = New Padding(3, 3, 2, 3)
			Me.label5.Name = "label5"
			Me.label5.Size = New Size(46, 13)
			Me.label5.TabIndex = 12
			Me.label5.Text = "Zip"
			Me.label5.TextAlign = ContentAlignment.MiddleLeft
			' 
			' txtName
			' 
			Me.txtName.Location = New Point(135, 44)
			Me.txtName.Margin = New Padding(3, 2, 3, 3)
			Me.txtName.Name = "txtName"
			Me.txtName.Size = New Size(199, 20)
			Me.txtName.TabIndex = 0
			' 
			' txtAddress
			' 
			Me.txtAddress.Location = New Point(136, 84)
			Me.txtAddress.Margin = New Padding(3, 1, 3, 3)
			Me.txtAddress.Name = "txtAddress"
			Me.txtAddress.Size = New Size(198, 20)
			Me.txtAddress.TabIndex = 1
			' 
			' txtCity
			' 
			Me.txtCity.Location = New Point(136, 123)
			Me.txtCity.Name = "txtCity"
			Me.txtCity.TabIndex = 2
			' 
			' txtState
			' 
			Me.txtState.Location = New Point(300, 123)
			Me.txtState.Name = "txtState"
			Me.txtState.Size = New Size(33, 20)
			Me.txtState.TabIndex = 3
			' 
			' txtZip
			' 
			Me.txtZip.Location = New Point(135, 163)
			Me.txtZip.Margin = New Padding(1, 3, 3, 3)
			Me.txtZip.Name = "txtZip"
			Me.txtZip.TabIndex = 4
			' 
			' btnOK
			' 
			Me.btnOK.Location = New Point(23, 207)
			Me.btnOK.Margin = New Padding(3, 3, 3, 1)
			Me.btnOK.Name = "btnOK"
			Me.btnOK.TabIndex = 5
			Me.btnOK.Text = "OK"
'			Me.btnOK.Click += New System.EventHandler(Me.OKButton_Click)
			' 
			' btnCancel
			' 
			Me.btnCancel.Location = New Point(157, 207)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.TabIndex = 6
			Me.btnCancel.Text = "Cancel"
'			Me.btnCancel.Click += New System.EventHandler(Me.CancelButton_Click)
			' 
			' label6
			' 
			Me.label6.Location = New Point(66, 12)
			Me.label6.Name = "label6"
			Me.label6.Size = New Size(226, 23)
			Me.label6.TabIndex = 13
			Me.label6.Text = "Simple Windows Forms Control"
			Me.label6.TextAlign = ContentAlignment.MiddleCenter
			' 
			' MyControl1
			' 
			Me.Controls.Add(Me.label6)
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnOK)
			Me.Controls.Add(Me.txtZip)
			Me.Controls.Add(Me.txtState)
			Me.Controls.Add(Me.txtCity)
			Me.Controls.Add(Me.txtAddress)
			Me.Controls.Add(Me.txtName)
			Me.Controls.Add(Me.label5)
			Me.Controls.Add(Me.label4)
			Me.Controls.Add(Me.label3)
			Me.Controls.Add(Me.label2)
			Me.Controls.Add(Me.label1)
			Me.Name = "MyControl1"
			Me.Size = New Size(359, 244)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		#End Region

		Public Delegate Sub MyControlEventHandler(ByVal sender As Object, ByVal args As MyControlEventArgs)
		Public Event OnButtonClick As MyControlEventHandler

		Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

			Dim retvals As New MyControlEventArgs(True, txtName.Text, txtAddress.Text, txtCity.Text, txtState.Text, txtZip.Text)
			RaiseEvent OnButtonClick(Me, retvals)
		End Sub

		Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
			Dim retvals As New MyControlEventArgs(False, txtName.Text, txtAddress.Text, txtCity.Text, txtState.Text, txtZip.Text)
			RaiseEvent OnButtonClick(Me, retvals)
		End Sub
	End Class

	Public Class MyControlEventArgs
		Inherits EventArgs
		Private _Name As String
		Private _StreetAddress As String
		Private _City As String
		Private _State As String
		Private _Zip As String
		Private _IsOK As Boolean

		Public Sub New(ByVal result As Boolean, ByVal name As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal zip As String)
			_IsOK = result
			_Name = name
			_StreetAddress = address
			_City = city
			_State = state
			_Zip = zip
		End Sub

		Public Property MyName() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
			End Set
		End Property
		Public Property MyStreetAddress() As String
			Get
				Return _StreetAddress
			End Get
			Set(ByVal value As String)
				_StreetAddress = value
			End Set
		End Property
		Public Property MyCity() As String
			Get
				Return _City
			End Get
			Set(ByVal value As String)
				_City = value
			End Set
		End Property
		Public Property MyState() As String
			Get
				Return _State
			End Get
			Set(ByVal value As String)
				_State = value
			End Set
		End Property
		Public Property MyZip() As String
			Get
				Return _Zip
			End Get
			Set(ByVal value As String)
				_Zip = value
			End Set
		End Property
		Public Property IsOK() As Boolean
			Get
				Return _IsOK
			End Get
			Set(ByVal value As Boolean)
				_IsOK = value
			End Set
		End Property
	End Class
End Namespace
