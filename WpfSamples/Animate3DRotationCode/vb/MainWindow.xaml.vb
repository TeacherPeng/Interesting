' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.Animation

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
		Private bMaterial As New DiffuseMaterial()
		Private cubeModel As New GeometryModel3D()
		Private coneModel As New GeometryModel3D()
		Private teapotModel As New GeometryModel3D()
		Private myTransforms As New Transform3DCollection()
		Private myModelVisual3D As New ModelVisual3D()
		Private myViewport As New Viewport3D()

		Private Sub WindowLoaded(ByVal sender As Object, ByVal e As EventArgs)
			RenderSomeModels()
		End Sub

		Private Sub RenderSomeModels()
			myViewport.Name = "myViewport"
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

			'Define DiffuseMaterial and apply to the mesh geometries.
			Dim teapotMaterial As New DiffuseMaterial(New SolidColorBrush(Colors.Blue))
			'Define a transformation
			Dim myRotateTransform As New RotateTransform3D(New AxisAngleRotation3D(New Vector3D(0, 2, 0), 1))
			'Define an animation for the transformation
			Dim myAnimation As New DoubleAnimation()
			myAnimation.From = 1
			myAnimation.To = 361
			myAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(5000))
			myAnimation.RepeatBehavior = RepeatBehavior.Forever
			'Add animation to the transformation
			myRotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, myAnimation)

			'Add transformation to the model
			teapotModel.Transform = myRotateTransform

			teapotModel.Material = teapotMaterial

			'Add the model to the model group collection
			modelGroup.Children.Add(teapotModel)
			modelGroup.Children.Add(myDirLight)
			myViewport.Camera = myPCamera

			myModelVisual3D.Content = modelGroup
			myViewport.Children.Add(myModelVisual3D)

			'add the Viewport3D to the window
			mainWindow.Content = myViewport

        End Sub
	End Class
End Namespace