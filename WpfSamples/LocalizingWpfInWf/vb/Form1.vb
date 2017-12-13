' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms

Namespace LocalizingWpfInWf
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("es-ES")

			InitializeComponent()
		End Sub
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim sc As New SimpleControl()

			Me.elementHost1.Child = sc
		End Sub
	End Class
End Namespace