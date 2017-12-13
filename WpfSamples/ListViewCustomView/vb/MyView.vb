Imports System.Windows.Controls.Primitives
Imports System.Globalization
Imports System.ComponentModel
Imports System.Xml

Namespace ListViewCustomView
	Public Class CoolView
		Inherits ViewBase
		Protected Overrides ReadOnly Property DefaultStyleKey() As Object
		  Get
			Return New ComponentResourceKey(Me.GetType(), "MyViewDSK")
		  End Get
		End Property

		Protected Overrides ReadOnly Property ItemContainerDefaultStyleKey() As Object
		  Get
			Return New ComponentResourceKey(Me.GetType(), "MyViewItemDSK")
		  End Get
		End Property
	End Class
End Namespace