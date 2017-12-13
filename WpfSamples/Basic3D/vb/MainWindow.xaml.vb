' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Media3D

Namespace BlankSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub

		'declare scene objects
		Private modelGroup As New Model3DGroup()
		Private myPCamera As New PerspectiveCamera()
		Private myDirLight As New DirectionalLight()
		Private teapotModel As New GeometryModel3D()
		Private myTransforms As New Transform3DCollection()
		Private myViewport As New Viewport3D()

		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As EventArgs)

			'Set camera viewpoint and properties.
			myPCamera.FarPlaneDistance = 20
			myPCamera.NearPlaneDistance = 1
			myPCamera.FieldOfView = 45
			myPCamera.Position = New Point3D(-5, 2, 3)
			myPCamera.LookDirection = New Vector3D(5, -2, -3)
			myPCamera.UpDirection = New Vector3D(0, 1, 0)

			'Add light sources to the scene.
			myDirLight.Color = Colors.White
			myDirLight.Direction = New Vector3D(-3, -4, -5)
			teapotModel.Geometry = CType(Application.Current.Resources("myTeapot"), MeshGeometry3D)

			'Define material and apply to the mesh geometries.
			Dim teapotMaterial As New DiffuseMaterial(New SolidColorBrush(Colors.Blue))

			teapotModel.Material = teapotMaterial

			'Add 3D model and lights to the collection; add the collection to the visual.

			modelGroup.Children.Add(teapotModel)
			modelGroup.Children.Add(myDirLight)

			Dim modelsVisual As New ModelVisual3D()
			modelsVisual.Content = modelGroup

			'Add the visual and camera to the Viewport3D.
			myViewport.Camera = myPCamera
			myViewport.Children.Add(modelsVisual)

			mainWindow.Content = myViewport

		End Sub
	End Class
End Namespace