Imports System.ComponentModel
Imports System.Collections.ObjectModel

Namespace DataTemplatingSample
    Public Class Task
        Implements INotifyPropertyChanged

        Private name As String
        Private descriptionValue As String
        Private priorityValue As Integer
        Private type As TaskType


        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Sub New()
        End Sub

        Public Sub New(ByVal name As String, ByVal description As String, ByVal priority As Integer, ByVal type As TaskType)
            Me.name = name
            Me.descriptionValue = description
            Me.priorityValue = priority
            Me.type = type
        End Sub

        Public Overrides Function ToString() As String
            Return name.ToString()
        End Function

        Public Property TaskName() As String
            Get
                Return name
            End Get
            Set(ByVal value As String)
                name = value
                OnPropertyChanged("TaskName")
            End Set
        End Property

        Public Property Description() As String
            Get
                Return descriptionValue
            End Get
            Set(ByVal value As String)
                descriptionValue = value
                OnPropertyChanged("Description")
            End Set
        End Property

        Public Property Priority() As Integer
            Get
                Return priorityValue
            End Get
            Set(ByVal value As Integer)
                priorityValue = value
                OnPropertyChanged("Priority")
            End Set
        End Property

        Public Property TaskType() As TaskType
            Get
                Return type
            End Get
            Set(ByVal value As TaskType)
                type = value
                OnPropertyChanged("TaskType")
            End Set
        End Property

        Protected Sub OnPropertyChanged(ByVal info As String)
            Dim handler As PropertyChangedEventHandler = PropertyChangedEvent
            If handler IsNot Nothing Then
                handler(Me, New PropertyChangedEventArgs(info))
            End If
        End Sub
    End Class
    Public Class Tasks
        Inherits ObservableCollection(Of Task)
        Public Sub New()
            MyBase.New()
            Add(New Task("Groceries", "Pick up Groceries and Detergent", 2, TaskType.Home))
            Add(New Task("Laundry", "Do my Laundry", 2, TaskType.Home))
            Add(New Task("Email", "Email clients", 1, TaskType.Work))
            Add(New Task("Clean", "Clean my office", 3, TaskType.Work))
            Add(New Task("Dinner", "Get ready for family reunion", 1, TaskType.Home))
            Add(New Task("Proposals", "Review new budget proposals", 2, TaskType.Work))
        End Sub
    End Class

    Public Enum TaskType
        Home
        Work
    End Enum
End Namespace
