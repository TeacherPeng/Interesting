﻿// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

namespace BlankSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //declare scene objects
        Model3DGroup modelGroup = new Model3DGroup();
        PerspectiveCamera myPCamera = new PerspectiveCamera();
        DirectionalLight myDirLight = new DirectionalLight();
        GeometryModel3D teapotModel = new GeometryModel3D();
        Transform3DCollection myTransforms = new Transform3DCollection();
        Viewport3D myViewport = new Viewport3D();

        private void WindowLoaded(object sender, EventArgs e)
        {

            //Set camera viewpoint and properties.
            myPCamera.FarPlaneDistance = 20;
            myPCamera.NearPlaneDistance = 1;
            myPCamera.FieldOfView = 45;
            myPCamera.Position = new Point3D(-5, 2, 3);
            myPCamera.LookDirection = new Vector3D(5, -2, -3);
            myPCamera.UpDirection = new Vector3D(0, 1, 0);

            //Add light sources to the scene.
            myDirLight.Color = Colors.White;
            myDirLight.Direction = new Vector3D(-3, -4, -5);
            teapotModel.Geometry = (MeshGeometry3D)Application.Current.Resources["myTeapot"];

            //Define material and apply to the mesh geometries.
            DiffuseMaterial teapotMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));

            teapotModel.Material = teapotMaterial;

            //Add 3D model and lights to the collection; add the collection to the visual.

            modelGroup.Children.Add(teapotModel);
            modelGroup.Children.Add(myDirLight);

            ModelVisual3D modelsVisual = new ModelVisual3D();
            modelsVisual.Content = modelGroup;

            //Add the visual and camera to the Viewport3D.
            myViewport.Camera = myPCamera;
            myViewport.Children.Add(modelsVisual);

            mainWindow.Content = myViewport;

        }
    }
}