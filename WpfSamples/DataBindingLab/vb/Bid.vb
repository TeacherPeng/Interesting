Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace DataBindingLab
	Public Class Bid

        Private amountValue As Integer

        Private bidderValue As User

#Region "Property Getters and Setters"
        Public ReadOnly Property Amount() As Integer
            Get
                Return Me.amountValue
            End Get
        End Property

        Public ReadOnly Property Bidder() As User
            Get
                Return Me.bidderValue
            End Get
        End Property
#End Region

        Public Sub New(ByVal amount As Integer, ByVal bidder As User)
            Me.amountValue = amount
            Me.bidderValue = bidder
        End Sub
	End Class
End Namespace
