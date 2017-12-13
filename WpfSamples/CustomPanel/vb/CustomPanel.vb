Imports System.Threading

Namespace Custom_Panel
	Public Class app
		Inherits Application

        Private window1 As Window
        Private customPanel1 As CustomPanel
        Private rect1 As Rectangle
        Private rect2 As Rectangle
        Private rect3 As Rectangle
        Private txt1 As TextBlock

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            MyBase.OnStartup(e)
            CreateAndShowMainWindow()
        End Sub

        Private Sub CreateAndShowMainWindow()
            ' Create the application's main window and instantiate a CustomPanel

            customPanel1 = New CustomPanel()
            customPanel1.Width = 450
            customPanel1.Height = 450

            ' Add elements to populate the CustomPanel

            rect1 = New Rectangle()
            rect2 = New Rectangle()
            rect3 = New Rectangle()
            rect1.Width = 200
            rect1.Height = 200
            rect1.Fill = Brushes.Blue
            rect2.Width = 200
            rect2.Height = 200
            rect2.Fill = Brushes.Purple
            rect3.Width = 200
            rect3.Height = 200
            rect3.Fill = Brushes.Red

            ' Add a TextBlock to show flowing and clip behavior

            txt1 = New TextBlock()
            txt1.Text = "Text is clipped when it reaches the edge of the container"
            txt1.FontSize = 25

            ' Add the elements defined above as children of the CustomPanel

            customPanel1.Children.Add(rect1)
            customPanel1.Children.Add(rect2)
            customPanel1.Children.Add(rect3)
            customPanel1.Children.Add(txt1)

            ' Add the CustomPanel as a Child of the MainWindow and show the Window

            window1 = New Window()
            window1.Content = customPanel1
            window1.Title = "Custom Panel Sample"
            window1.Show()
        End Sub

		' Define the CustomPanel class, derived from Panel

		Public Class CustomPanel
			Inherits Panel

		' Define a default Constructor

			Public Sub New()
				MyBase.New()
			End Sub

			' Override the Measure method of Panel

			Protected Overrides Function MeasureOverride(ByVal constraint As Size) As Size
				Dim curLineSize As New Size()
				Dim panelSize As New Size()

				Dim children As UIElementCollection = InternalChildren

				For i As Integer = 0 To children.Count - 1
					Dim child As UIElement = TryCast(children(i), UIElement)

					' Flow passes its own constraint to children

					child.Measure(constraint)
					Dim sz As Size = child.DesiredSize

                    If curLineSize.Width + sz.Width > constraint.Width Then

                        'need to switch to another line
                        panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width)
                        panelSize.Height += curLineSize.Height
                        curLineSize = sz

                        If sz.Width > constraint.Width Then

                            ' if the element is wider then the constraint - give it a separate line
                            panelSize.Width = Math.Max(sz.Width, panelSize.Width)
                            panelSize.Height += sz.Height
                            curLineSize = New Size()
                        End If
                    Else

                        'continue to accumulate a line
                        curLineSize.Width += sz.Width
                        curLineSize.Height = Math.Max(sz.Height, curLineSize.Height)
                    End If
				Next i

				' the last line size, if any need to be added

				panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width)
				panelSize.Height += curLineSize.Height

				Return panelSize
			End Function


			Protected Overrides Function ArrangeOverride(ByVal arrangeBounds As Size) As Size
				Dim firstInLine As Integer = 0

				Dim curLineSize As New Size()

				Dim accumulatedHeight As Double = 0

				Dim children As UIElementCollection = InternalChildren

				For i As Integer = 0 To children.Count - 1
					Dim sz As Size = children(i).DesiredSize

                    If curLineSize.Width + sz.Width > arrangeBounds.Width Then

                        'switch to another line
                        arrangeLine(accumulatedHeight, curLineSize.Height, firstInLine, i)

                        accumulatedHeight += curLineSize.Height
                        curLineSize = sz

                        If sz.Width > arrangeBounds.Width Then

                            'the element is wider then the constraint - give it a separate line
                            i = i + 1
                            arrangeLine(accumulatedHeight, sz.Height, i, i)
                            accumulatedHeight += sz.Height
                            curLineSize = New Size()
                        End If
                        firstInLine = i
                    Else

                        'continue to accumulate a line
                        curLineSize.Width += sz.Width
                        curLineSize.Height = Math.Max(sz.Height, curLineSize.Height)
                    End If
				Next i

				If firstInLine < children.Count Then
					arrangeLine(accumulatedHeight, curLineSize.Height, firstInLine, children.Count)
				End If

				Return arrangeBounds
			End Function

			Private Sub arrangeLine(ByVal y As Double, ByVal lineHeight As Double, ByVal start As Integer, ByVal [end] As Integer)
				Dim x As Double = 0
				Dim children As UIElementCollection = InternalChildren
				For i As Integer = start To [end] - 1
					Dim child As UIElement = children(i)
					child.Arrange(New Rect(x, y, child.DesiredSize.Width, lineHeight))
					x += child.DesiredSize.Width
				Next i
			End Sub

		End Class


	End Class

	' Run the application

	Friend NotInheritable Class EntryClass
		Private Sub New()
		End Sub
        <STAThread()>
        Public Shared Sub Main()
            Dim app As New app()
            app.Run()
        End Sub
	End Class


End Namespace