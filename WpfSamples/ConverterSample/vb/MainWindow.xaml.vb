' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Media3D


Namespace MilConverterSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
		' This method performs the Point operations
		Public Sub PerformOperation(ByVal sender As Object, ByVal e As RoutedEventArgs)

			Dim li As RadioButton = (TryCast(sender, RadioButton))

			' Strings used to display the results
			Dim syntaxString, resultType, operationString As String

			' The local variables point1, point2, vector2, etc are defined in each
			' case block for readability reasons. Each variable is contained within
			' the scope of each case statement.  
	  Select Case li.Name

				Case "rb1"
						' Converts a String to a Point using a PointConverter
						' Returns a Point.

						Dim pConverter As New PointConverter()
						Dim pointResult As New Point()
						Dim string1 As String = "10,20"

						pointResult = CType(pConverter.ConvertFromString(string1), Point)
						' pointResult is equal to (10, 20)

						' Displaying Results
						syntaxString = "pointResult = (Point)pConverter1.ConvertFromString(string1);"
						resultType = "Point"
						operationString = "Converting a String to a Point"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb2"
						' Converts a String to a Vector using a VectorConverter
						' Returns a Vector.

						Dim vConverter As New VectorConverter()
						Dim vectorResult As New Vector()
						Dim string1 As String = "10,20"

						vectorResult = CType(vConverter.ConvertFromString(string1), Vector)
						' vectorResult is equal to (10, 20)

						' Displaying Results
						syntaxString = "vectorResult = (Vector)vConverter.ConvertFromString(string1);"
						resultType = "Vector"
						operationString = "Converting a String into a Vector"
						ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb3"
						' Converts a String to a Matrix using a MatrixConverter
						' Returns a Matrix.

						Dim mConverter As New MatrixConverter()
						Dim matrixResult As New Matrix()
						Dim string2 As String = "10,20,30,40,50,60"

						matrixResult = CType(mConverter.ConvertFromString(string2), Matrix)
						' matrixResult is equal to (10, 20, 30, 40, 50, 60)

						' Displaying Results
						syntaxString = "matrixResult = (Vector)mConverter.ConvertFromString(string2);"
						resultType = "Matrix"
						operationString = "Converting a String into a Matrix"
						ShowResults(matrixResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb4"
						' Converts a String to a Point3D using a Point3DConverter
						' Returns a Point3D.

						Dim p3DConverter As New Point3DConverter()
						Dim point3DResult As New Point3D()
						Dim string3 As String = "10,20,30"

						point3DResult = CType(p3DConverter.ConvertFromString(string3), Point3D)
						' point3DResult is equal to (10, 20, 30)

						' Displaying Results
						syntaxString = "point3DResult = (Point3D)p3DConverter.ConvertFromString(string3);"
						resultType = "Point3D"
						operationString = "Converting a String into a Point3D"
						ShowResults(point3DResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb5"
						' Converts a String to a Vector3D using a Vector3DConverter
						' Returns a Vector3D.

						Dim v3DConverter As New Vector3DConverter()
						Dim vector3DResult As New Vector3D()
						Dim string3 As String = "10,20,30"

						vector3DResult = CType(v3DConverter.ConvertFromString(string3), Vector3D)
						' vector3DResult is equal to (10, 20, 30)

						' Displaying Results
						syntaxString = "vector3DResult = (Vector3D)v3DConverter.ConvertFromString(string3);"
						resultType = "Vector3D"
						operationString = "Converting a String into a Vector3D"
						ShowResults(vector3DResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb6"
						' Converts a String to a Size3D using a Size3DConverter
						' Returns a Size3D.

						Dim s3DConverter As New Size3DConverter()
						Dim size3DResult As New Size3D()
						Dim string3 As String = "10,20,30"

						size3DResult = CType(s3DConverter.ConvertFromString(string3), Size3D)
						' size3DResult is equal to (10, 20, 30)

						' Displaying Results
						syntaxString = "size3DResult = (Size3D)v3DConverter.ConvertFromString(string3);"
						resultType = "Size3D"
						operationString = "Converting a String into a Size3D"
						ShowResults(size3DResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb7"
						' Converts a String to a Point4D using a Point4DConverter
						' Returns a Point4D.

						Dim p4DConverter As New Point4DConverter()
						Dim point4DResult As New Point4D()
						Dim string4 As String = "10,20,30,40"

						point4DResult = CType(p4DConverter.ConvertFromString(string4), Point4D)
						' point4DResult is equal to (10, 20, 30)

						' Displaying Results
						syntaxString = "point4DResult = (Point4D)v3DConverter.ConvertFromString(string3);"
						resultType = "Point4D"
						operationString = "Converting a String into a Point4D"
						ShowResults(point4DResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

					Case Else
					Exit Select

            End Select
		End Sub


		' Displays the results of the operation
		Private Sub ShowResults(ByVal resultValue As String, ByVal syntax As String, ByVal resultType As String, ByVal opString As String)

			txtResultValue.Text = resultValue
			txtSyntax.Text = syntax
			txtResultType.Text = resultType
			txtOperation.Text = opString
		End Sub

		' Displays the values of the variables
		Public Sub ShowVars()

			Dim s1 As String = "10, 20"
			Dim s2 As String = "10, 20, 30, 40, 50, 60"
			Dim s3 As String = "10, 20, 30"
			Dim s4 As String = "10, 20, 30, 40"

			' Displaying values in Text objects

			txtString1.Text = s1.ToString()
			txtString2.Text = s2.ToString()
			txtString3.Text = s3.ToString()
			txtString4.Text = s4.ToString()
		End Sub
	End Class
End Namespace