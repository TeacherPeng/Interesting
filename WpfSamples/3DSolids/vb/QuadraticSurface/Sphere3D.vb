' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Windows.Media.Media3D

Namespace QuadraticSurface
	Public NotInheritable Class Sphere3D
		Inherits Primitive3D
		Friend Function GetPosition(ByVal t As Double, ByVal y As Double) As Point3D
			Dim r As Double = Math.Sqrt(1 - y * y)
			Dim x As Double = r * Math.Cos(t)
			Dim z As Double = r * Math.Sin(t)

			Return New Point3D(x, y, z)
		End Function

		Private Function GetNormal(ByVal t As Double, ByVal y As Double) As Vector3D
			Return CType(GetPosition(t, y), Vector3D)
		End Function

		Private Function GetTextureCoordinate(ByVal t As Double, ByVal y As Double) As Point
			Dim TYtoUV As New Matrix()
			TYtoUV.Scale(1 / (2 * Math.PI), -0.5)

			Dim p As New Point(t, y)
			p = p * TYtoUV

			Return p
		End Function

		Friend Overrides Function Tessellate() As Geometry3D
			Dim tDiv As Integer = 32
			Dim yDiv As Integer = 32
			Dim maxTheta As Double = DegToRad(360.0)
			Dim minY As Double = -1.0
			Dim maxY As Double = 1.0

			Dim dt As Double = maxTheta / tDiv
			Dim dy As Double = (maxY - minY) / yDiv

			Dim mesh As New MeshGeometry3D()

			For yi As Integer = 0 To yDiv
				Dim y As Double = minY + yi * dy

				For ti As Integer = 0 To tDiv
					Dim t As Double = ti * dt

					mesh.Positions.Add(GetPosition(t, y))
					mesh.Normals.Add(GetNormal(t, y))
					mesh.TextureCoordinates.Add(GetTextureCoordinate(t, y))
				Next ti
			Next yi

			For yi As Integer = 0 To yDiv - 1
				For ti As Integer = 0 To tDiv - 1
					Dim x0 As Integer = ti
					Dim x1 As Integer = (ti + 1)
					Dim y0 As Integer = yi * (tDiv + 1)
					Dim y1 As Integer = (yi + 1) * (tDiv + 1)

					mesh.TriangleIndices.Add(x0 + y0)
					mesh.TriangleIndices.Add(x0 + y1)
					mesh.TriangleIndices.Add(x1 + y0)

					mesh.TriangleIndices.Add(x1 + y0)
					mesh.TriangleIndices.Add(x0 + y1)
					mesh.TriangleIndices.Add(x1 + y1)
				Next ti
			Next yi

			mesh.Freeze()
			Return mesh
		End Function
	End Class
End Namespace
