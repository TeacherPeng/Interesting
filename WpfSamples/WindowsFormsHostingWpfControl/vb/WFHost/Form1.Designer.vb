Namespace WFHost
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
			Me.panel1 = New Panel()
			Me.groupBox1 = New GroupBox()
			Me.radioBackgroundLightSalmon = New RadioButton()
			Me.radioBackgroundLightGreen = New RadioButton()
			Me.radioBackgroundOriginal = New RadioButton()
			Me.radioForegroundYellow = New RadioButton()
			Me.radioForegroundRed = New RadioButton()
			Me.groupBox2 = New GroupBox()
			Me.radioForegroundOriginal = New RadioButton()
			Me.radioFamilyWingDings = New RadioButton()
			Me.radioFamilyTimes = New RadioButton()
			Me.groupBox3 = New GroupBox()
			Me.radioFamilyOriginal = New RadioButton()
			Me.radioSizeTwelve = New RadioButton()
			Me.radioSizeTen = New RadioButton()
			Me.groupBox4 = New GroupBox()
			Me.radioSizeOriginal = New RadioButton()
			Me.radioStyleItalic = New RadioButton()
			Me.groupBox5 = New GroupBox()
			Me.radioStyleOriginal = New RadioButton()
			Me.radioWeightBold = New RadioButton()
			Me.groupBox6 = New GroupBox()
			Me.radioWeightOriginal = New RadioButton()
			Me.lblName = New Label()
			Me.lblAddress = New Label()
			Me.lblState = New Label()
			Me.lblZip = New Label()
			Me.lblCity = New Label()
			Me.groupBox7 = New GroupBox()
			Me.tableLayoutPanel1 = New TableLayoutPanel()
			Me.groupBox1.SuspendLayout()
			Me.groupBox2.SuspendLayout()
			Me.groupBox3.SuspendLayout()
			Me.groupBox4.SuspendLayout()
			Me.groupBox5.SuspendLayout()
			Me.groupBox6.SuspendLayout()
			Me.groupBox7.SuspendLayout()
			Me.tableLayoutPanel1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' panel1
			' 
			Me.panel1.Dock = DockStyle.Fill
			Me.panel1.Location = New Point(333, 3)
			Me.panel1.Name = "panel1"
			Me.tableLayoutPanel1.SetRowSpan(Me.panel1, 3)
			Me.panel1.Size = New Size(325, 285)
			Me.panel1.TabIndex = 0
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.radioBackgroundLightSalmon)
			Me.groupBox1.Controls.Add(Me.radioBackgroundLightGreen)
			Me.groupBox1.Controls.Add(Me.radioBackgroundOriginal)
			Me.groupBox1.Dock = DockStyle.Fill
			Me.groupBox1.Location = New Point(3, 3)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New Size(324, 91)
			Me.groupBox1.TabIndex = 0
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Background Color"
			' 
			' radioBackgroundLightSalmon
			' 
			Me.radioBackgroundLightSalmon.AutoSize = True
			Me.radioBackgroundLightSalmon.Location = New Point(11, 66)
			Me.radioBackgroundLightSalmon.Name = "radioBackgroundLightSalmon"
			Me.radioBackgroundLightSalmon.Size = New Size(94, 17)
			Me.radioBackgroundLightSalmon.TabIndex = 2
			Me.radioBackgroundLightSalmon.Text = "LightSalmon"
'			Me.radioBackgroundLightSalmon.CheckedChanged += New System.EventHandler(Me.radioBackgroundLightSalmon_CheckedChanged)
			' 
			' radioBackgroundLightGreen
			' 
			Me.radioBackgroundLightGreen.AutoSize = True
			Me.radioBackgroundLightGreen.Location = New Point(11, 43)
			Me.radioBackgroundLightGreen.Name = "radioBackgroundLightGreen"
			Me.radioBackgroundLightGreen.Size = New Size(87, 17)
			Me.radioBackgroundLightGreen.TabIndex = 1
			Me.radioBackgroundLightGreen.Text = "LightGreen"
'			Me.radioBackgroundLightGreen.CheckedChanged += New System.EventHandler(Me.radioBackgroundLightGreen_CheckedChanged)
			' 
			' radioBackgroundOriginal
			' 
			Me.radioBackgroundOriginal.AutoSize = True
			Me.radioBackgroundOriginal.Location = New Point(11, 20)
			Me.radioBackgroundOriginal.Name = "radioBackgroundOriginal"
			Me.radioBackgroundOriginal.Size = New Size(68, 17)
			Me.radioBackgroundOriginal.TabIndex = 0
			Me.radioBackgroundOriginal.Text = "Original"
'			Me.radioBackgroundOriginal.CheckedChanged += New System.EventHandler(Me.radioBackgroundOriginal_CheckedChanged)
			' 
			' radioForegroundYellow
			' 
			Me.radioForegroundYellow.AutoSize = True
			Me.radioForegroundYellow.Location = New Point(11, 66)
			Me.radioForegroundYellow.Name = "radioForegroundYellow"
			Me.radioForegroundYellow.Size = New Size(62, 17)
			Me.radioForegroundYellow.TabIndex = 2
			Me.radioForegroundYellow.Text = "Yellow"
'			Me.radioForegroundYellow.CheckedChanged += New System.EventHandler(Me.radioForegroundYellow_CheckedChanged)
			' 
			' radioForegroundRed
			' 
			Me.radioForegroundRed.AutoSize = True
			Me.radioForegroundRed.Location = New Point(11, 43)
			Me.radioForegroundRed.Name = "radioForegroundRed"
			Me.radioForegroundRed.Size = New Size(48, 17)
			Me.radioForegroundRed.TabIndex = 1
			Me.radioForegroundRed.Text = "Red"
'			Me.radioForegroundRed.CheckedChanged += New System.EventHandler(Me.radioForegroundRed_CheckedChanged)
			' 
			' groupBox2
			' 
			Me.groupBox2.Controls.Add(Me.radioForegroundYellow)
			Me.groupBox2.Controls.Add(Me.radioForegroundRed)
			Me.groupBox2.Controls.Add(Me.radioForegroundOriginal)
			Me.groupBox2.Dock = DockStyle.Fill
			Me.groupBox2.Location = New Point(3, 100)
			Me.groupBox2.Name = "groupBox2"
			Me.groupBox2.Size = New Size(324, 91)
			Me.groupBox2.TabIndex = 1
			Me.groupBox2.TabStop = False
			Me.groupBox2.Text = "Foreground Color"
			' 
			' radioForegroundOriginal
			' 
			Me.radioForegroundOriginal.AutoSize = True
			Me.radioForegroundOriginal.Location = New Point(11, 19)
			Me.radioForegroundOriginal.Name = "radioForegroundOriginal"
			Me.radioForegroundOriginal.Size = New Size(68, 17)
			Me.radioForegroundOriginal.TabIndex = 0
			Me.radioForegroundOriginal.Text = "Original"
'			Me.radioForegroundOriginal.CheckedChanged += New System.EventHandler(Me.radioForegroundOriginal_CheckedChanged)
			' 
			' radioFamilyWingDings
			' 
			Me.radioFamilyWingDings.AutoSize = True
			Me.radioFamilyWingDings.Location = New Point(11, 66)
			Me.radioFamilyWingDings.Name = "radioFamilyWingDings"
			Me.radioFamilyWingDings.Size = New Size(86, 17)
			Me.radioFamilyWingDings.TabIndex = 2
			Me.radioFamilyWingDings.Text = "WingDings"
'			Me.radioFamilyWingDings.CheckedChanged += New System.EventHandler(Me.radioFamilyWingDings_CheckedChanged)
			' 
			' radioFamilyTimes
			' 
			Me.radioFamilyTimes.AutoSize = True
			Me.radioFamilyTimes.Location = New Point(11, 43)
			Me.radioFamilyTimes.Name = "radioFamilyTimes"
			Me.radioFamilyTimes.Size = New Size(130, 17)
			Me.radioFamilyTimes.TabIndex = 1
			Me.radioFamilyTimes.Text = "Times New Roman"
'			Me.radioFamilyTimes.CheckedChanged += New System.EventHandler(Me.radioFamilyTimes_CheckedChanged)
			' 
			' groupBox3
			' 
			Me.groupBox3.Controls.Add(Me.radioFamilyWingDings)
			Me.groupBox3.Controls.Add(Me.radioFamilyTimes)
			Me.groupBox3.Controls.Add(Me.radioFamilyOriginal)
			Me.groupBox3.Dock = DockStyle.Fill
			Me.groupBox3.Location = New Point(3, 294)
			Me.groupBox3.Name = "groupBox3"
			Me.groupBox3.Size = New Size(324, 91)
			Me.groupBox3.TabIndex = 2
			Me.groupBox3.TabStop = False
			Me.groupBox3.Text = "Font Family"
			' 
			' radioFamilyOriginal
			' 
			Me.radioFamilyOriginal.AutoSize = True
			Me.radioFamilyOriginal.Location = New Point(11, 20)
			Me.radioFamilyOriginal.Name = "radioFamilyOriginal"
			Me.radioFamilyOriginal.Size = New Size(68, 17)
			Me.radioFamilyOriginal.TabIndex = 0
			Me.radioFamilyOriginal.Text = "Original"
'			Me.radioFamilyOriginal.CheckedChanged += New System.EventHandler(Me.radioFamilyOriginal_CheckedChanged)
			' 
			' radioSizeTwelve
			' 
			Me.radioSizeTwelve.AutoSize = True
			Me.radioSizeTwelve.Location = New Point(11, 66)
			Me.radioSizeTwelve.Name = "radioSizeTwelve"
			Me.radioSizeTwelve.Size = New Size(39, 17)
			Me.radioSizeTwelve.TabIndex = 2
			Me.radioSizeTwelve.Text = "12"
'			Me.radioSizeTwelve.CheckedChanged += New System.EventHandler(Me.radioSizeTwelve_CheckedChanged)
			' 
			' radioSizeTen
			' 
			Me.radioSizeTen.AutoSize = True
			Me.radioSizeTen.Location = New Point(11, 43)
			Me.radioSizeTen.Name = "radioSizeTen"
			Me.radioSizeTen.Size = New Size(39, 17)
			Me.radioSizeTen.TabIndex = 1
			Me.radioSizeTen.Text = "10"
'			Me.radioSizeTen.CheckedChanged += New System.EventHandler(Me.radioSizeTen_CheckedChanged)
			' 
			' groupBox4
			' 
			Me.groupBox4.Controls.Add(Me.radioSizeTwelve)
			Me.groupBox4.Controls.Add(Me.radioSizeTen)
			Me.groupBox4.Controls.Add(Me.radioSizeOriginal)
			Me.groupBox4.Dock = DockStyle.Fill
			Me.groupBox4.Location = New Point(3, 197)
			Me.groupBox4.Name = "groupBox4"
			Me.groupBox4.Size = New Size(324, 91)
			Me.groupBox4.TabIndex = 3
			Me.groupBox4.TabStop = False
			Me.groupBox4.Text = "Font Size"
			' 
			' radioSizeOriginal
			' 
			Me.radioSizeOriginal.AutoSize = True
			Me.radioSizeOriginal.Location = New Point(11, 20)
			Me.radioSizeOriginal.Name = "radioSizeOriginal"
			Me.radioSizeOriginal.Size = New Size(68, 17)
			Me.radioSizeOriginal.TabIndex = 0
			Me.radioSizeOriginal.Text = "Original"
'			Me.radioSizeOriginal.CheckedChanged += New System.EventHandler(Me.radioSizeOriginal_CheckedChanged)
			' 
			' radioStyleItalic
			' 
			Me.radioStyleItalic.AutoSize = True
			Me.radioStyleItalic.Location = New Point(11, 43)
			Me.radioStyleItalic.Name = "radioStyleItalic"
			Me.radioStyleItalic.Size = New Size(53, 17)
			Me.radioStyleItalic.TabIndex = 1
			Me.radioStyleItalic.Text = "Italic"
'			Me.radioStyleItalic.CheckedChanged += New System.EventHandler(Me.radioStyleItalic_CheckedChanged)
			' 
			' groupBox5
			' 
			Me.groupBox5.Controls.Add(Me.radioStyleItalic)
			Me.groupBox5.Controls.Add(Me.radioStyleOriginal)
			Me.groupBox5.Dock = DockStyle.Fill
			Me.groupBox5.Location = New Point(3, 391)
			Me.groupBox5.Name = "groupBox5"
			Me.groupBox5.Size = New Size(324, 66)
			Me.groupBox5.TabIndex = 4
			Me.groupBox5.TabStop = False
			Me.groupBox5.Text = "Font Style"
			' 
			' radioStyleOriginal
			' 
			Me.radioStyleOriginal.AutoSize = True
			Me.radioStyleOriginal.Location = New Point(11, 20)
			Me.radioStyleOriginal.Name = "radioStyleOriginal"
			Me.radioStyleOriginal.Size = New Size(64, 17)
			Me.radioStyleOriginal.TabIndex = 0
			Me.radioStyleOriginal.Text = "Normal"
'			Me.radioStyleOriginal.CheckedChanged += New System.EventHandler(Me.radioStyleOriginal_CheckedChanged)
			' 
			' radioWeightBold
			' 
			Me.radioWeightBold.AutoSize = True
			Me.radioWeightBold.Location = New Point(11, 43)
			Me.radioWeightBold.Name = "radioWeightBold"
			Me.radioWeightBold.Size = New Size(50, 17)
			Me.radioWeightBold.TabIndex = 1
			Me.radioWeightBold.Text = "Bold"
'			Me.radioWeightBold.CheckedChanged += New System.EventHandler(Me.radioWeightBold_CheckedChanged)
			' 
			' groupBox6
			' 
			Me.groupBox6.Controls.Add(Me.radioWeightBold)
			Me.groupBox6.Controls.Add(Me.radioWeightOriginal)
			Me.groupBox6.Dock = DockStyle.Fill
			Me.groupBox6.Location = New Point(3, 463)
			Me.groupBox6.Name = "groupBox6"
			Me.groupBox6.Size = New Size(324, 84)
			Me.groupBox6.TabIndex = 5
			Me.groupBox6.TabStop = False
			Me.groupBox6.Text = "Font Weight"
			' 
			' radioWeightOriginal
			' 
			Me.radioWeightOriginal.AutoSize = True
			Me.radioWeightOriginal.Location = New Point(11, 20)
			Me.radioWeightOriginal.Name = "radioWeightOriginal"
			Me.radioWeightOriginal.Size = New Size(68, 17)
			Me.radioWeightOriginal.TabIndex = 0
			Me.radioWeightOriginal.Text = "Original"
'			Me.radioWeightOriginal.CheckedChanged += New System.EventHandler(Me.radioWeightOriginal_CheckedChanged)
			' 
			' lblName
			' 
			Me.lblName.AutoSize = True
			Me.lblName.Location = New Point(6, 24)
			Me.lblName.Name = "lblName"
			Me.lblName.Size = New Size(43, 13)
			Me.lblName.TabIndex = 7
			Me.lblName.Text = "Name:"
			' 
			' lblAddress
			' 
			Me.lblAddress.AutoSize = True
			Me.lblAddress.Location = New Point(6, 47)
			Me.lblAddress.Margin = New Padding(3, 3, 3, 1)
			Me.lblAddress.Name = "lblAddress"
			Me.lblAddress.Size = New Size(94, 13)
			Me.lblAddress.TabIndex = 8
			Me.lblAddress.Text = "Street Address:"
			' 
			' lblState
			' 
			Me.lblState.AutoSize = True
			Me.lblState.Location = New Point(8, 97)
			Me.lblState.Margin = New Padding(3, 1, 3, 3)
			Me.lblState.Name = "lblState"
			Me.lblState.Size = New Size(41, 13)
			Me.lblState.TabIndex = 10
			Me.lblState.Text = "State:"
			' 
			' lblZip
			' 
			Me.lblZip.AutoSize = True
			Me.lblZip.Location = New Point(9, 121)
			Me.lblZip.Margin = New Padding(3, 1, 3, 3)
			Me.lblZip.Name = "lblZip"
			Me.lblZip.Size = New Size(29, 13)
			Me.lblZip.TabIndex = 11
			Me.lblZip.Text = "Zip:"
			' 
			' lblCity
			' 
			Me.lblCity.AutoSize = True
			Me.lblCity.Location = New Point(6, 70)
			Me.lblCity.Margin = New Padding(3, 1, 2, 1)
			Me.lblCity.Name = "lblCity"
			Me.lblCity.Size = New Size(32, 13)
			Me.lblCity.TabIndex = 9
			Me.lblCity.Text = "City:"
			' 
			' groupBox7
			' 
			Me.groupBox7.Controls.Add(Me.lblZip)
			Me.groupBox7.Controls.Add(Me.lblState)
			Me.groupBox7.Controls.Add(Me.lblCity)
			Me.groupBox7.Controls.Add(Me.lblAddress)
			Me.groupBox7.Controls.Add(Me.lblName)
			Me.groupBox7.Location = New Point(333, 294)
			Me.groupBox7.Name = "groupBox7"
			Me.tableLayoutPanel1.SetRowSpan(Me.groupBox7, 3)
			Me.groupBox7.Size = New Size(186, 155)
			Me.groupBox7.TabIndex = 12
			Me.groupBox7.TabStop = False
			Me.groupBox7.Text = "Data from control"
			' 
			' tableLayoutPanel1
			' 
			Me.tableLayoutPanel1.AutoSize = True
			Me.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink
			Me.tableLayoutPanel1.ColumnCount = 2
			Me.tableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox6, 0, 5)
			Me.tableLayoutPanel1.Controls.Add(Me.panel1, 1, 0)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox1, 0, 0)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox2, 0, 1)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox4, 0, 2)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox3, 0, 3)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox5, 0, 4)
			Me.tableLayoutPanel1.Controls.Add(Me.groupBox7, 1, 3)
			Me.tableLayoutPanel1.Dock = DockStyle.Fill
			Me.tableLayoutPanel1.Location = New Point(0, 0)
			Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
			Me.tableLayoutPanel1.RowCount = 6
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.RowStyles.Add(New RowStyle())
			Me.tableLayoutPanel1.Size = New Size(661, 550)
			Me.tableLayoutPanel1.TabIndex = 13
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(7F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.AutoSize = True
			Me.ClientSize = New Size(661, 550)
			Me.Controls.Add(Me.tableLayoutPanel1)
			Me.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.Name = "Form1"
			Me.Text = "Windows Form Hosting Wpf Control"
'			Me.Load += New System.EventHandler(Me.Form1_Load)
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			Me.groupBox2.ResumeLayout(False)
			Me.groupBox2.PerformLayout()
			Me.groupBox3.ResumeLayout(False)
			Me.groupBox3.PerformLayout()
			Me.groupBox4.ResumeLayout(False)
			Me.groupBox4.PerformLayout()
			Me.groupBox5.ResumeLayout(False)
			Me.groupBox5.PerformLayout()
			Me.groupBox6.ResumeLayout(False)
			Me.groupBox6.PerformLayout()
			Me.groupBox7.ResumeLayout(False)
			Me.groupBox7.PerformLayout()
			Me.tableLayoutPanel1.ResumeLayout(False)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private panel1 As Panel
		Private groupBox1 As GroupBox
		Private WithEvents radioBackgroundOriginal As RadioButton
		Private WithEvents radioBackgroundLightGreen As RadioButton
		Private WithEvents radioBackgroundLightSalmon As RadioButton
		Private WithEvents radioForegroundYellow As RadioButton
		Private WithEvents radioForegroundRed As RadioButton
		Private groupBox2 As GroupBox
		Private WithEvents radioForegroundOriginal As RadioButton
		Private WithEvents radioFamilyWingDings As RadioButton
		Private WithEvents radioFamilyTimes As RadioButton
		Private groupBox3 As GroupBox
		Private WithEvents radioFamilyOriginal As RadioButton
		Private groupBox4 As GroupBox
		Private WithEvents radioSizeTwelve As RadioButton
		Private WithEvents radioSizeTen As RadioButton
		Private WithEvents radioSizeOriginal As RadioButton
		Private WithEvents radioStyleItalic As RadioButton
		Private groupBox5 As GroupBox
		Private WithEvents radioStyleOriginal As RadioButton
		Private WithEvents radioWeightBold As RadioButton
		Private groupBox6 As GroupBox
		Private WithEvents radioWeightOriginal As RadioButton
		Private lblName As Label
		Private lblAddress As Label
		Private lblState As Label
		Private lblZip As Label
		Private lblCity As Label
		Private groupBox7 As GroupBox
		Private tableLayoutPanel1 As TableLayoutPanel

	End Class
End Namespace


