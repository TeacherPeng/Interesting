Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.Reflection

Namespace DialogBoxSample
	Public Class FontPropertyLists
        Private Shared mFontFaces As Collection(Of FontFamily)
        Private Shared mFontStyles As Collection(Of FontStyle)
        Private Shared mFontWeights As Collection(Of FontWeight)
        Private Shared mFontSizes As Collection(Of Double)

        ''' <summary>
        ''' Return a collection of avaliable font styles 
        ''' </summary>
        Public Shared ReadOnly Property FontFaces() As ICollection(Of FontFamily)
            Get
                If mFontFaces Is Nothing Then
                    mFontFaces = New Collection(Of FontFamily)()
                End If
                For Each fontFamily As FontFamily In Fonts.SystemFontFamilies
                    mFontFaces.Add(fontFamily)
                Next fontFamily
                Return mFontFaces
            End Get
        End Property

        ''' <summary>
        ''' Return a collection of avaliable font styles 
        ''' </summary>
        Public Shared ReadOnly Property FontStyles() As ICollection
            Get
                If mFontStyles Is Nothing Then
                    mFontStyles = New Collection(Of FontStyle)()
                    mFontStyles.Add(System.Windows.FontStyles.Normal)
                    mFontStyles.Add(System.Windows.FontStyles.Oblique)
                    mFontStyles.Add(System.Windows.FontStyles.Italic)
                End If
                Return mFontStyles
            End Get
        End Property

        ''' <summary>
        ''' Return a collection of avaliable FontWeight
        ''' </summary>
        Public Shared ReadOnly Property FontWeights() As ICollection
            Get
                If mFontWeights Is Nothing Then
                    mFontWeights = New Collection(Of FontWeight)()
                    mFontWeights.Add(System.Windows.FontWeights.Thin)
                    mFontWeights.Add(System.Windows.FontWeights.Light)
                    mFontWeights.Add(System.Windows.FontWeights.Normal)
                    mFontWeights.Add(System.Windows.FontWeights.Medium)
                    mFontWeights.Add(System.Windows.FontWeights.Heavy)
                    mFontWeights.Add(System.Windows.FontWeights.SemiBold)
                    mFontWeights.Add(System.Windows.FontWeights.Bold)
                    mFontWeights.Add(System.Windows.FontWeights.ExtraLight)
                    mFontWeights.Add(System.Windows.FontWeights.ExtraBold)
                    mFontWeights.Add(System.Windows.FontWeights.ExtraBlack)
                End If
                Return mFontWeights
            End Get
        End Property

        ''' <summary>
        ''' Return a collection of font sizes.
        ''' </summary>
        Public Shared ReadOnly Property FontSizes() As Collection(Of Double)
            Get
                If mFontSizes Is Nothing Then
                    mFontSizes = New Collection(Of Double)()
                    For i As Double = 8 To 40
                        mFontSizes.Add(i)
                    Next i
                End If
                Return mFontSizes
            End Get
        End Property

		Public Shared Function CanParseFontStyle(ByVal fontStyleName As String) As Boolean
			Try
				Dim converter As New FontStyleConverter()
				converter.ConvertFromString(fontStyleName)
				Return True
			Catch
				Return False
			End Try
		End Function
		Public Shared Function ParseFontStyle(ByVal fontStyleName As String) As FontStyle
			Dim converter As New FontStyleConverter()
			Return CType(converter.ConvertFromString(fontStyleName), FontStyle)
		End Function
		Public Shared Function CanParseFontWeight(ByVal fontWeightName As String) As Boolean
			Try
				Dim converter As New FontWeightConverter()
				converter.ConvertFromString(fontWeightName)
				Return True
			Catch
				Return False
			End Try
		End Function
		Public Shared Function ParseFontWeight(ByVal fontWeightName As String) As FontWeight
			Dim converter As New FontWeightConverter()
			Return CType(converter.ConvertFromString(fontWeightName), FontWeight)
		End Function
	End Class
End Namespace
