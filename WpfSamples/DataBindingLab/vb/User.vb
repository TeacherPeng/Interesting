Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace DataBindingLab
	Public Class User

        Private nameValue As String

        Private ratingValue As Integer

        Private memberSinceValue As Date

#Region "Property Getters and Setters"
        Public ReadOnly Property Name() As String
            Get
                Return Me.nameValue
            End Get
        End Property

        Public Property Rating() As Integer
            Get
                Return Me.ratingValue
            End Get
            Set(ByVal value As Integer)
                Me.ratingValue = value
            End Set
        End Property

        Public ReadOnly Property MemberSince() As Date
            Get
                Return Me.memberSinceValue
            End Get
        End Property
#End Region

        Public Sub New(ByVal name As String, ByVal rating As Integer, ByVal memberSince As Date)
            Me.nameValue = name
            Me.ratingValue = rating
            Me.memberSinceValue = memberSince
        End Sub
	End Class

End Namespace
