' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Media3D

Namespace QuadraticSurface
	Public MustInherit Class Primitive3D
		Inherits ModelVisual3D
		Public Sub New()
			Content = _content
			_content.Geometry = Tessellate()
		End Sub

		Public Shared MaterialProperty As DependencyProperty = DependencyProperty.Register("Material", GetType(Material), GetType(Primitive3D), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf OnMaterialChanged)))

		Public Property Material() As Material
			Get
				Return CType(GetValue(MaterialProperty), Material)
			End Get
			Set(ByVal value As Material)
				SetValue(MaterialProperty, value)
			End Set
		End Property

		Friend Shared Sub OnMaterialChanged(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)
			Dim p As Primitive3D = (CType(sender, Primitive3D))

			p._content.Material = p.Material
		End Sub

		Friend Shared Sub OnGeometryChanged(ByVal d As DependencyObject)
			Dim p As Primitive3D = (CType(d, Primitive3D))

			p._content.Geometry = p.Tessellate()
		End Sub

		Friend Function DegToRad(ByVal degrees As Double) As Double
			Return (degrees / 180.0) * Math.PI
		End Function

		Friend MustOverride Function Tessellate() As Geometry3D

		Friend ReadOnly _content As New GeometryModel3D()
	End Class
End Namespace
