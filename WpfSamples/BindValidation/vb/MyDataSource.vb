Namespace BindValidation
	Public Class MyDataSource
		Private _age As Integer
		Private _age2 As Integer
		Private _age3 As Integer

		Public Sub New()
			Age = 0
			Age2 = 0
		End Sub

		Public Property Age() As Integer
			Get
				Return _age
			End Get
			Set(ByVal value As Integer)
				_age = value
			End Set
		End Property
		Public Property Age2() As Integer
			Get
				Return _age2
			End Get
			Set(ByVal value As Integer)
				_age2 = value
			End Set
		End Property

		Public Property Age3() As Integer
			Get
				Return _age3
			End Get
			Set(ByVal value As Integer)
				_age3 = value
			End Set
		End Property
	End Class
End Namespace
