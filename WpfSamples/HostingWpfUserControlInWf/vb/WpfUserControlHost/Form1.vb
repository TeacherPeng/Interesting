' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.ComponentModel
Imports System.Text

Imports System.Windows.Forms.Integration

Namespace WpfUserControlHost
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' Create the ElementHost control for hosting the
			' WPF UserControl.
			Dim host As New ElementHost()
			host.Dock = DockStyle.Fill

			' Create the WPF UserControl.
			Dim uc As New HostingWpfUserControlInWf.UserControl1()

			' Assign the WPF UserControl to the ElementHost control's
			' Child property.
			host.Child = uc

			' Add the ElementHost control to the form's
			' collection of child controls.
			Me.Controls.Add(host)
		End Sub
	End Class
End Namespace
