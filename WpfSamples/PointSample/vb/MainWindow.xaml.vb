Imports System.Text

Namespace MilPointSample
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

			Dim li As RadioButton = TryCast(sender, RadioButton)

			' Strings used to display the results
			Dim syntaxString, resultType, operationString As String

			' The local variables point1, point2, vector2, etc are defined in each
			' case block for readability reasons. Each variable is contained within
			' the scope of each case statement.  
			Select Case li.Name

				Case "rb1"
						' Translates a Point by a Vector using the overloaded + operator. 
						' Returns a Point.
						Dim point1 As New Point(10, 5)
						Dim vector1 As New Vector(20, 30)
						Dim pointResult As New Point()

						pointResult = point1 + vector1
						' pointResult is equal to (30, 35)

						' Note: Adding a Point to a Point is not a legal operation 

						' Displaying Results
						syntaxString = "pointResult = point1 + vector1;"
						resultType = "Point"
						operationString = "Adding a Point and Vector"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb2"
						' Translates a Point by a Vector using the static Add method.
						' Returns a Point.  
						Dim point1 As New Point(10, 5)
						Dim vector1 As New Vector(20, 30)
						Dim pointResult As New Point()

						pointResult = Point.Add(point1, vector1)
						' pointResult is equal to (30, 35)

						' Displaying Results
						syntaxString = "pointResult = Point.Add(point1, vector1);"
						resultType = "Point"
						operationString = "Adding a Point and Vector"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb3"
						' Subtracts a Vector from a Point using the overloaded - operator.
						' Returns a Point.
						Dim point1 As New Point(10, 5)
						Dim vector1 As New Vector(20, 30)
						Dim pointResult As New Point()

						pointResult = point1 - vector1
						' pointResult is equal to (-10, -25) 

						' Displaying Results
						syntaxString = "pointResult = point1 - vector1;"
						resultType = "Point"
						operationString = "Subtracting a Vector from a Point"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb4"
						' Subtracts a Vector from a Point using the static Subtract method. 
						' Returns a Point.
						Dim point1 As New Point(10, 5)
						Dim vector1 As New Vector(20, 30)
						Dim pointResult As New Point()

						pointResult = Point.Subtract(point1, vector1)
						' pointResult is equal to (-10, -25)

						' Displaying Results
						syntaxString = "pointResult = Point.Subtract(point1, vector1);"
						resultType = "Point"
						operationString = "Subtracting a Vector from a Point"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb5"
						' Subtracts a Point from a Point using the overloaded - operator.
						' Returns a Vector.
						Dim point1 As New Point(10, 5)
						Dim point2 As New Point(15, 40)
						Dim vectorResult As New Vector()

						vectorResult = point1 - point2
						' vectorResult is equal to (-5, -35)

						' Displaying Results
						syntaxString = "vectorResult = point1 - point2;"
						resultType = "Vector"
						operationString = "Subtracting a Point from a Point"
						ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb6"
						' Subtracts a Point from a Point using the static Subtract method.  
						' Returns a Vector.
						Dim point1 As New Point(10, 5)
						Dim point2 As New Point(15, 40)
						Dim vectorResult As New Vector()

						vectorResult = Point.Subtract(point1, point2)
						' vectorResult is equal to (-5, -35)

						' Displaying Results
						syntaxString = "vectorResult = Point.Subtract(point1, point2);"
						resultType = "Vector"
						operationString = "Subtracting a Point from a Point"
						ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb7"
						' Offsets the X and Y values of a Point.
						Dim point1 As New Point(10, 5)

						point1.Offset(20, 30)
						' point1 is equal to (30, 35)

						' Note: This operation is equivalent to adding a point 
						' to vector with the corresponding X,Y values.

						' Displaying Results
						syntaxString = "point1.Offset(20,30);"
						resultType = "Point"
						operationString = "Offsetting a Point"
						ShowResults(point1.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb8"
						' Multiplies a Point by a Matrix.  
						' Returns a Point.
						Dim point1 As New Point(10, 5)
						Dim pointResult As New Point()
						Dim matrix1 As New Matrix(40, 50, 60, 70, 80, 90)

						pointResult = point1 * matrix1
						' pointResult is equal to (780, 940)

						' Displaying Results
						resultType = "Point"
						syntaxString = "pointResult = point1 * matrix1;"
						operationString = "Multiplying a Point by a Matrix"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb9"
						' Multiplies a Point by a Matrix.  
						' Returns a Point.
						Dim point1 As New Point(10, 5)
						Dim pointResult As New Point()
						Dim matrix1 As New Matrix(40, 50, 60, 70, 80, 90)

						pointResult = Point.Multiply(point1, matrix1)
						' pointResult is equal to (780, 940)

						' Displaying Results
						resultType = "Point"
						syntaxString = "pointResult = Point.Multiply(point1, matrix1);"
						operationString = "Multiplying a Point by a Matrix"
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb10"
						' Checks if two Points are equal using the overloaded equality operator.
						Dim point1 As New Point(10, 5)
						Dim point2 As New Point(15, 40)
						Dim areEqual As Boolean

						areEqual = (point1 = point2)
						' areEqual is False

						' Displaying Results
						syntaxString = "areEqual = (point1 == point2);"
						resultType = "Boolean"
						operationString = "Checking if two points are equal"
						ShowResults(areEqual.ToString(), syntaxString, resultType, operationString)
						Exit Select


				Case "rb11"
						' Checks if two Points are equal using the static Equals method.
						Dim point1 As New Point(10, 5)
						Dim point2 As New Point(15, 40)
						Dim areEqual As Boolean

						areEqual = Point.Equals(point1, point2)
						' areEqual is False	

						' Displaying Results
						syntaxString = "areEqual = Point.Equals(point1, point2);"
						resultType = "Boolean"
						operationString = "Checking if two points are equal"
						ShowResults(areEqual.ToString(), syntaxString, resultType, operationString)
						Exit Select
				Case "rb12"
						' Compares an Object and a Point for equality using the non-static Equals method.
						Dim point1 As New Point(10, 5)
						Dim point2 As New Point(15, 40)
						Dim areEqual As Boolean

						areEqual = point1.Equals(point2)
						' areEqual is False	


						' Displaying Results
						syntaxString = "areEqual = point1.Equals(point2);"
						resultType = "Boolean"
						operationString = "Checking if two points are equal"
						ShowResults(areEqual.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb13"
						' Compares an Object and a Vector for equality using the non-static Equals method.
						Dim vector1 As New Vector(20, 30)
						Dim vector2 As New Vector(45, 70)
						Dim areEqual As Boolean

						areEqual = vector1.Equals(vector2)
						' areEqual is False	


						' Displaying Results
						syntaxString = "areEqual = vector1.Equals(vector2);"
						resultType = "Boolean"
						operationString = "Checking if two vectors are equal"
						ShowResults(areEqual.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb14"
						' Converts a string representation of a point into a Point structure
						Dim pointResult As New Point()

						pointResult = Point.Parse("1,3")
						' pointResult is equal to (1, 3)

						' Displaying Results
						syntaxString = "pointResult = Point.Parse(""1,3"");"
						resultType = "Matrix"
						operationString = "Converts a string into a Point structure."
						ShowResults(pointResult.ToString(), syntaxString, resultType, operationString)
						Exit Select
				Case "rb15"
						' Gets a string representation of a Point structure
						Dim point1 As New Point(10, 5)
						Dim pointString As String

						pointString = point1.ToString()
						' pointString is equal to 10,5

						' Displaying Results
						syntaxString = "pointString = point1.ToString();"
						resultType = "String"
						operationString = "Getting the string representation of a Point"
						ShowResults(pointString.ToString(), syntaxString, resultType, operationString)
						Exit Select
				Case "rb16"
						' Gets the hashcode of a Point structure

						Dim point1 As New Point(10, 5)
						Dim pointHashCode As Integer

						pointHashCode = point1.GetHashCode()

						' Displaying Results
						syntaxString = "pointHashCode = point1.GetHashCode();"
						resultType = "int"
						operationString = "Getting the hashcode of Point"
						ShowResults(pointHashCode.ToString(), syntaxString, resultType, operationString)
						Exit Select
				Case "rb17"
						' Explicitly converts a Point structure into a Size structure
						' Returns a Size.

						Dim point1 As New Point(10, 5)
						Dim size1 As New Size()

						size1 = CType(point1, Size)
						' size1 has a width of 10 and a height of 5

						' Displaying Results
						syntaxString = "size1 = (Size)point1;"
						resultType = "Size"
						operationString = "Expliciting casting a Point into a Size"
						ShowResults(size1.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case "rb18"
						' Explicitly converts a Point structure into a Vector structure
						' Returns a Vector.

						Dim point1 As New Point(10, 5)
						Dim vector1 As New Vector()

						vector1 = CType(point1, Vector)
						' vector1 is equal to (10,5)

						' Displaying Results
						syntaxString = "vector1 = (Vector)point1;"
						resultType = "Vector"
						operationString = "Expliciting casting a Point into a Vector"
						ShowResults(vector1.ToString(), syntaxString, resultType, operationString)
						Exit Select

				' task example.  Not accessed through radio buttons
				Case "rb20"
						' Checks if two Points are not equal using the overloaded inequality operator.

						' Declaring point1 and initializing x,y values
						Dim point1 As New Point(10, 5)

						' Declaring point2 without initializing x,y values
						Dim point2 As New Point()

						' Boolean to hold the result of the comparison
						Dim areNotEqual As Boolean

						' assigning values to point2
						point2.X = 15
						point2.Y = 40

						' checking for inequality
						areNotEqual = (point1 <> point2)

						' areNotEqual is True

						' Displaying Results
						syntaxString = "areNotEqual = (point1 != point2);"
						resultType = "Boolean"
						operationString = "Checking if two points are not equal"
						ShowResults(areNotEqual.ToString(), syntaxString, resultType, operationString)
						Exit Select

				Case Else
					Exit Select

			End Select 'end switch
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

			Dim p1 As New Point(10, 5)
			Dim p2 As New Point(15, 40)

			Dim v1 As New Vector(20, 30)
			Dim v2 As New Vector(45, 70)

			Dim m1 As New Matrix(40, 50, 60, 70, 80, 90)

			' Displaying values in Text objects
			txtPoint1.Text = p1.ToString()
			txtPoint2.Text = p2.ToString()
			txtVector1.Text = v1.ToString()
			txtVector2.Text = v2.ToString()
			txtMatrix1.Text = m1.ToString()
		End Sub
	End Class
End Namespace
