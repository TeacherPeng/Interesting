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

namespace MilMatrixSample
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

        public void PerformOperation(object sender, RoutedEventArgs e)
        {

            RadioButton li = sender as RadioButton;
            String syntaxString, resultType, operationString;
            String[,] varArray = new String[5,2];


            ///The local variable point1, vector1, matrix1, etc are defined in each
            ///case block for readability reasons. Each variable is contained within
            ///the scope of each case statement.  
           
            switch (li.Name)
            { // begin switch


                case "rb1":
                    {
                        // Multiplies a Matrix by a Matrix using the overloaded * operator
                        // Returns a Matrix
                        Matrix matrix1 = new Matrix(5,10,15,20,25,30);
                        Boolean isInvertible;

                        isInvertible = matrix1.HasInverse;
                        // isInvertible is equal to True    

                        // Displaying Results
                        syntaxString = "isInvertible = matrix1.HasInverse;";
                        resultType = "Boolean";
                        operationString = "Checking if matrix1 is invertible";
                        ShowResults(isInvertible.ToString(), syntaxString, resultType, operationString);

                        break;
                    }
                case "rb2":
                    {   
                        // Translates a Matrix
                        // Returns a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrixResult = new Matrix();
                        Double offsetX = 15;
                        Double offsetY = 25;

                        matrix1.Translate(offsetX, offsetY);
                        // matrix1 is not equal to 
                        
                        //Displaying Results
                        syntaxString = "matrix1.Translate(offsetX, offsetY);";
                        resultType = "Void";
                        operationString = "Translating a Matrix by a Point";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);

                        break;
                    }
                case "rb3":
                    {
                        // Prepend a Tranlsation to a Matrix
                        // Returns a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrixResult = new Matrix();
                        Double offsetX = 15;
                        Double offsetY = 25;

                        matrix1.TranslatePrepend(offsetX, offsetY);
                        // matrix1 is not equal to 

                        //Displaying Results
                        syntaxString = " matrix1.TranslatePrepend(offsetX, offsetY);";
                        resultType = "Void";
                        operationString = "Prepending Translating a matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);

                        break;
                    }
                case "rb4":
                    {
                        // Sets a Matrix to an identity matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);

                        matrix1.SetIdentity();
                        // matrix1 is now equal to (1,0,0,1,0,0)

                        //Displaying Results
                        syntaxString = "matrix1.SetIdentity();";
                        resultType = "Void";
                        operationString = "Setting a matrix to an identity matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);


                        break;
                    }
                case "rb5":
                    {


                        // Checks if a Matrix is an identity matrix

                        // Creates a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Boolean isIdentityMatrix;
                        
                        // Sets matrix1 into an identity matrix
                        matrix1.SetIdentity();

                        isIdentityMatrix = matrix1.IsIdentity;
                        // isIdentityMatrix is equal to True

                        //Displaying Results
                        syntaxString = "isIdentityMatrix = matrix1.IsIdentity;";
                        resultType = "Boolean";
                        operationString = "Determining if a Matrix is an identity matrix";
                        ShowResults(isIdentityMatrix.ToString(), syntaxString, resultType, operationString);


                        break;
                    }



                case "rb6":
                    {
                        // Changes a Matrix into an identity matrix
                        
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);

                        matrix1 = Matrix.Identity;
                        // matrix1 is now equal to (1,0,0,1,0,0)

                        //Displaying Results
                        syntaxString = "matrix1 = Matrix.Identity;";
                        resultType = "Matrix";
                        operationString = "Gets an identity Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);



                        break;
                    }
                case "rb7":
                    {
                        // Converts a string representation of a matrix into a Matrix structure
                        Matrix matrixResult = new Matrix();

                        matrixResult = Matrix.Parse("1,2,3,4,5,6"); 
                        // matrixResult is equal to (1,2,3,4,5,6)

                        //Displaying Results
                        syntaxString = "matrixResult = Matrix.Parse(\"1,2,3,4,5,6\");";
                        resultType = "Matrix";
                        operationString = "Convert a string into a Matrix structure";
                        ShowResults(matrixResult.ToString(), syntaxString, resultType, operationString);
                        break;
                    }
                case "rb8":
                    {
                        // Checks if two Matrixes are equal using the static Equals method
                        // Returns a Boolean.
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Boolean areEqual;

                        areEqual = Matrix.Equals(matrix1, matrix2);
                        // areEqual is equal to False

                        //Displaying Results
                        syntaxString = "areEqual = Matrix.Equals(matrix1, matrix2);";
                        resultType = "Boolean";
                        operationString = "Checking if the matrices are equal";
                        ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);

                        break;
                    }
                case "rb8b":
                    {
                        // Checks if an Object is equal to a Matrix using the static Equals method
                        // Returns a Boolean.
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Boolean areEqual;

                        areEqual = matrix1.Equals(matrix2);
                        // areEqual is equal to False

                        //Displaying Results
                        syntaxString = "areEqual = Matrix.Equals(matrix1, matrix2);";
                        resultType = "Boolean";
                        operationString = "Checking if the matrices are equal";
                        ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);

                        break;
                    }

                case "rb9":
                    {
                        // Checks if two Matrixes are equal using the overloaded == operator
                        // Returns a Boolean.    
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Boolean areEqual;

                        areEqual = matrix1 == matrix2;
                        // areEqual is equal to False

                        //Displaying Results
                        syntaxString = "areEqual = matrix1 == matrix2;";
                        resultType = "Boolean";
                        operationString = "Checking if the matrices are equal";
                        ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);
                        break;
                    }
                case "rb10":
                    {
                        // Checks if two Matrixes are not equal using the overloaded != operator
                        // Returns a Boolean.    
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Boolean areEqual;

                        areEqual = matrix1 != matrix2;
                        // areEqual is equal to False

                        //Displaying Results
                        syntaxString = "areEqual = matrix1 != matrix2;";
                        resultType = "Boolean";
                        operationString = "Checking if the matrices are not equal";
                        ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);
                        break;
                    }
                case "rb11":
                    {
                        // Inverts a Matrix

                        // Creating a Matrix structure
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        
                        // Checking if matrix1 is invertible
                        if (matrix1.HasInverse)
                        {
                            
                            // Inverting matrix1                         
                            matrix1.Invert();
                            
                            // matrix1 is equal to (-0.04, 0.2 , 0.3, -0.1, 1, -2) 
                        }
                        
                          

                        //Displaying Results
                        syntaxString = "matrix1.Invert();";
                        resultType = "Void";
                        operationString = "Inverting a matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                    }
                case "rb12":
                    {
                        // Prepends a Matrix to another Matrix.
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);

                        matrix1.Prepend(matrix2);
                        // matrix1 is equal to (70,100,150,220,255,370)     

                        //Displaying Results
                        syntaxString = "matrix1.Prepend(matrix2);";
                        resultType = "Void";
                        operationString = "Prepending a Matrix to another Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
           
                    }
                case "rb13":
                    {
                        // Appends a Matrix to another Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);

                        matrix1.Append(matrix2);
                        // matrix1 is equal to (70,100,150,220,240,352)    

                        //Displaying Results
                        syntaxString = "matrix1.Append(matrix2);";
                        resultType = "Void";
                        operationString = "Appending a Matrix to another Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;

                    }
                case "rb14":
                    {
                        // Rotates a Matrix by a specified angle
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double rotateAngle = 90;

                        matrix1.Rotate(rotateAngle);
                        // matrix1 is equal to (-10,5,-20,15,-30,25)   

                        //Displaying Results
                        syntaxString = "matrix1.Rotate(rotateAngle);";
                        resultType = "Void";
                        operationString = "Rotating a Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                    }

                case "rb15":
                    {
                        // Rotates a Matrix by a specified angle at a specific point
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);

                        matrix1.RotateAt(90, 2, 4);
                        // matrix1 is equal to (-10,5,-20,15,-24,27)    

                        //Displaying Results
                        syntaxString = "matrix1.RotateAt(rotateAngle, rotateCenterX, rotateCenterY);";
                        resultType = "Void";
                        operationString = "Rotating a Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                       
                    }

                case "rb16":
                    {
                        // Prepends a Rotation to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double rotateAngle = 90;

                        matrix1.RotatePrepend(rotateAngle);
                        // matrix1 is equal to (15,20,-5,-10,25,30)    

                        //Displaying Results
                        syntaxString = "matrix1.RotatePrepend(rotateAngle);";
                        resultType = "Void";
                        operationString = "Rotating a Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                       
                    }

                case "rb17":
                    {
                        // Prepends a Rotation at a specific point to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double rotateAngle = 90;

                        matrix1.RotateAtPrepend(90, 2, 4);
                        // matrix1 is equal to  (15,20,-5,-10,85,130)

                        //Displaying Results
                        syntaxString = "matrix1.RotateAtPrepend(rotateAngle, rotateCenterX, rotateCenterY);";
                        resultType = "Void";
                        operationString = "Rotating a Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                       
                    }

                case "rb18":
                    {
                        // Scales a Matrix 
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double scaleX = (1);
                        Double scaleY = (2);

                        matrix1.Scale(scaleX, scaleY);
                        // matrix1 is equal to     

                        //Displaying Results
                        syntaxString = "matrix1.Scale(scaleX, scaleY);";
                        resultType = "Void";
                        operationString = "Scaling a Matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                      
                    }
                case "rb19":
                    {
                        // Multiplies a Matrix by another Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Matrix matrixResult = new Matrix();

                        matrixResult = Matrix.Multiply(matrix2, matrix1);
                        // matrixResult is equal to (70, 100, 150, 220, 255, 370)    

                        //Displaying Results
                        syntaxString = "matrixResult = Matrix.Multiply(matrix2, matrix1);";
                        resultType = "Matrix";
                        operationString = "Multiplying matrix1 and matrix2";
                        ShowResults(matrixResult.ToString(), syntaxString, resultType, operationString);
                        break;
                    }

                case "rb20":
                    {
                        // Multiplies a Matrix by another Matrix using the overloaded * operator
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Matrix matrix2 = new Matrix(2, 4, 6, 8, 10, 12);
                        Matrix matrixResult = new Matrix();

                        matrixResult = matrix1 * matrix2;
                        // matrixResult is equal to (70, 100, 150, 220, 240, 352)   

                        //Displaying Results
                        syntaxString = " matrixResult = matrix1 * matrix2;";
                        resultType = "Matrix";
                        operationString = "Multiplying matrix1 and matrix2";
                        ShowResults(matrixResult.ToString(), syntaxString, resultType, operationString);
                        break;
                    }


                case "rb21":
                    {
                        // Appends a skew to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double skewAngleX = 45;
                        Double skewAngleY = 180;

                        matrix1.Skew(skewAngleX, skewAngleY);
                        // matrix1 is equal to (15, 10, 35, 20, 55, 30)


                        //Displaying Results
                        syntaxString = "matrix1.Skew(skewAngleX, skewAngleY);";
                        resultType = "Void";
                        operationString = "Multiplying matrix2 and matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;


                    }

                case "rb22":
                    {
                        // Prepends a skew to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double skewAngleX = 45;
                        Double skewAngleY = 180;

                        matrix1.SkewPrepend(skewAngleX, skewAngleY);
                        // matrix1 is equal to (5, 10, 20, 30, 25, 30)

                        //Displaying Results
                        syntaxString = "matrix1.SkewPrepend(skewAngleX, skewAngleY);";
                        resultType = "Void";
                        operationString = "Multiplying matrix2 and matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                    

                    }


                case "rb23":
                    {
                        // Appends a scale to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double scaleFactorX = 2;
                        Double scaleFactorY = 4;

                        matrix1.Scale(scaleFactorX, scaleFactorY);
                        // matrix1 is equal to (10, 40, 30, 80, 50, 120)

                        //Displaying Results
                        syntaxString = "matrix1.Scale(scaleFactorX, scaleFactorY);";
                        resultType = "Void";
                        operationString = "Appending a scale to a matrix";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;


                    }
                case "rb24":
                    {
                        // Appends a scale at a specific point to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);

                        matrix1.ScaleAt(2, 4, 5, 10);
                        // matrix1 is equal to (10, 40, 30, 80, 45, 90)

                        //Displaying Results
                        syntaxString = " matrix1.ScaleAt(scaleFactorX, scaleFactorY, scaleCenterX, scaleCenterY);";
                        resultType = "Void";
                        operationString = "Appends a scale at a specific point to matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;


                    }
                case "rb25":
                    {
                        // Prepends a scale to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Double scaleFactorX = 2;
                        Double scaleFactorY = 4;

                        matrix1.ScalePrepend(scaleFactorX, scaleFactorY);
                        // matrix1 is equal to (10, 20, 60, 80, 25, 30)

                        //Displaying Results
                        syntaxString = "matrix1.ScalePrepend(scaleFactorX, scaleFactorY);";
                        resultType = "Void";
                        operationString = "Prepending a scale to matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;


                    }
                case "rb26":
                    {
                        // Prepends a scale at a specific point to a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);

                        matrix1.ScaleAtPrepend(2, 4, 5, 10);
                        // matrix1 is equal to (10, 20, 60, 80, -450, -620)

                        //Displaying Results
                        syntaxString = "matrix1.ScalePrependAt(scaleFactorX, scaleFactorY, centerPointX, centerPointY);";
                        resultType = "Void";
                        operationString = "Prepending a scale at a specific point to matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;


                    }

                case "rb29":
                    {
                        // Transform a point by a matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Point point1 = new Point(15, 25);
                        Point pointResult = new Point();

                        pointResult = matrix1.Transform(point1);
                        // pointResult is equal to (475, 680)

                        //Displaying Results
                        syntaxString = "pointResult = matrix1.Transform(point1)";
                        resultType = "Point";
                        operationString = "Transforming a point1 by matrix1";
                        ShowResults(pointResult.ToString(), syntaxString, resultType, operationString);
                        break;


                    }
                case "rb30":
                    {
                        // Transform a Vector by a Matrix
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Vector vector1 = new Vector(15, 25);
                        Vector vectorResult = new Vector();

                        vectorResult = matrix1.Transform(vector1);
                        // vectorResult is equal to (450, 650)

                        //Displaying Results
                        syntaxString = "vectorResult = matrix1.Transform(vector1);";
                        resultType = "Vector";
                        operationString = "Multiplying matrix2 and matrix1";
                        ShowResults(matrix1.ToString(), syntaxString, resultType, operationString);
                        break;
                    }

                case "rb31":
                    {
                        // Transform an array of Points by a Matrix

                        // Creating a Matrix and an array of Pointers
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Point[] pointArray = new Point[2];
    
                        // Setting the Point's X and Y values
                        pointArray[0].X = 15;
                        pointArray[0].Y = 25;
                        pointArray[1].X = 30;
                        pointArray[1].Y = 35;

                        // Transforming the Points in pointArry by matrix1
                        matrix1.Transform(pointArray);

                        // pointArray[0] is equal to (475, 680)
                        // pointArray[1] is equal to (700, 1030)

                        //Displaying Results
                        syntaxString = "matrix1.Transform(pointArray);";
                        resultType = "void";
                        operationString = "Transforming an array of Points by matrix1";
                        ShowResults(pointArray[1].ToString(), syntaxString, resultType, operationString);
                        break;


                    }
                case "rb32":
                    {
                        // Transform an array of Vectors by a Matrix
                        
                        // Creating  a Matrix and an array of Vectors
                        Matrix matrix1 = new Matrix(5, 10, 15, 20, 25, 30);
                        Vector[] vectorArray = new Vector[2];

                        // Setting the Vector's X and Y values
                        vectorArray[0].X = 15;
                        vectorArray[0].Y = 25;
                        vectorArray[1].X = 30;
                        vectorArray[1].Y = 35;

                        // Transforming the Vectors in vectorArray by matrix1
                        matrix1.Transform(vectorArray);

                        // VectorArray[0] is equal to (450, 650)
                        // VectorArray[1] is equal to (675, 1000)

                        //Displaying Results
                        syntaxString = " matrix1.Transform(vectorArray);";
                        resultType = "Void";
                        operationString = "Multiplying matrix2 and matrix1";
                        ShowResults(vectorArray[0].ToString(), syntaxString, resultType, operationString);
                        break;
                    }

                default:
                        break;
            }// end switch
          
        
        }


        // Displays the results 

        private void ShowResults(String resultValue, String syntax, String resultType, String opString)
        {
            // Displays the results of the operation
            txtResultValue.Text = resultValue;
            txtSyntax.Text = syntax;
            txtResultType.Text = resultType;
            txtOperation.Text = opString;
        }
        
        // Displays the variables

        public void ShowVars()
        {
            // Displays the values of the variables
            Point p1 = new Point(15, 25);
            Vector v1 = new Vector(15, 25);
            Matrix m1 = new Matrix(5, 10, 15, 20, 25, 30);
            Matrix m2 = new Matrix(2, 4, 6, 8, 10, 12);
            Double s1 = 75;

            // Sets the Text in the text objects.  These are 
            // defined in the MainWindow.xaml file

            txtPoint1.Text = p1.ToString();
            txtVector1.Text = v1.ToString();
            txtMatrix1.Text = m1.ToString();
            txtMatrix2.Text = m2.ToString();
            txtScalar1.Text = s1.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowVars();
        }

    }
}
