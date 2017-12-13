Imports System.Text
Imports System.Windows.Controls

Namespace MyControls
    Partial Public Class CustomControl1
        Inherits Grid
        Public Delegate Sub MyControlEventHandler(ByVal sender As Object, ByVal args As MyControlEventArgs)
        Public Event OnButtonClick As MyControlEventHandler
        Private _fontWeight As FontWeight
        Private _fontSize As Double
        Private _fontFamily As FontFamily
        Private _fontStyle As FontStyle
        Private _foreground As SolidColorBrush
        Private _background As SolidColorBrush

        Private Sub Init(ByVal sender As Object, ByVal e As EventArgs)
            'They all have the same style, so use nameLabel to set initial values.
            _fontWeight = nameLabel.FontWeight
            _fontSize = nameLabel.FontSize
            _fontFamily = nameLabel.FontFamily
            _fontStyle = nameLabel.FontStyle
            _foreground = CType(nameLabel.Foreground, SolidColorBrush)
            _background = CType(rootElement.Background, SolidColorBrush)
        End Sub
        Private Sub ButtonClicked(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim retvals As New MyControlEventArgs(True, txtName.Text, txtAddress.Text, txtCity.Text, txtState.Text, txtZip.Text)
            If sender Is btnCancel Then
                retvals.IsOK = False
            End If
            RaiseEvent OnButtonClick(Me, retvals)
        End Sub
        Public Property MyControl_FontWeight() As FontWeight
            Get
                Return _fontWeight
            End Get
            Set(ByVal value As FontWeight)
                _fontWeight = value
                nameLabel.FontWeight = value
                addressLabel.FontWeight = value
                cityLabel.FontWeight = value
                stateLabel.FontWeight = value
                zipLabel.FontWeight = value
            End Set
        End Property
        Public Property MyControl_FontSize() As Double
            Get
                Return _fontSize
            End Get
            Set(ByVal value As Double)
                _fontSize = value
                nameLabel.FontSize = value
                addressLabel.FontSize = value
                cityLabel.FontSize = value
                stateLabel.FontSize = value
                zipLabel.FontSize = value
            End Set
        End Property
        Public Property MyControl_FontStyle() As FontStyle
            Get
                Return _fontStyle
            End Get
            Set(ByVal value As FontStyle)
                _fontStyle = value
                nameLabel.FontStyle = value
                addressLabel.FontStyle = value
                cityLabel.FontStyle = value
                stateLabel.FontStyle = value
                zipLabel.FontStyle = value
            End Set
        End Property
        Public Property MyControl_FontFamily() As FontFamily
            Get
                Return _fontFamily
            End Get
            Set(ByVal value As FontFamily)
                _fontFamily = value
                nameLabel.FontFamily = value
                addressLabel.FontFamily = value
                cityLabel.FontFamily = value
                stateLabel.FontFamily = value
                zipLabel.FontFamily = value
            End Set
        End Property

        Public Property MyControl_Background() As SolidColorBrush
            Get
                Return _background
            End Get
            Set(ByVal value As SolidColorBrush)
                _background = value
                rootElement.Background = value
            End Set
        End Property
        Public Property MyControl_Foreground() As SolidColorBrush
            Get
                Return _foreground
            End Get
            Set(ByVal value As SolidColorBrush)
                _foreground = value
                nameLabel.Foreground = value
                addressLabel.Foreground = value
                cityLabel.Foreground = value
                stateLabel.Foreground = value
                zipLabel.Foreground = value
            End Set
        End Property
    End Class

    Public Class MyControlEventArgs
        Inherits EventArgs
        Private _Name As String
        Private _StreetAddress As String
        Private _City As String
        Private _State As String
        Private _Zip As String
        Private _IsOK As Boolean

        Public Sub New(ByVal result As Boolean, ByVal name As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal zip As String)
            _IsOK = result
            _Name = name
            _StreetAddress = address
            _City = city
            _State = state
            _Zip = zip
        End Sub

        Public Property MyName() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Property MyStreetAddress() As String
            Get
                Return _StreetAddress
            End Get
            Set(ByVal value As String)
                _StreetAddress = value
            End Set
        End Property
        Public Property MyCity() As String
            Get
                Return _City
            End Get
            Set(ByVal value As String)
                _City = value
            End Set
        End Property
        Public Property MyState() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property
        Public Property MyZip() As String
            Get
                Return _Zip
            End Get
            Set(ByVal value As String)
                _Zip = value
            End Set
        End Property
        Public Property IsOK() As Boolean
            Get
                Return _IsOK
            End Get
            Set(ByVal value As Boolean)
                _IsOK = value
            End Set
        End Property
    End Class
End Namespace