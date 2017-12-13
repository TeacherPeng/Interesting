Imports System.Windows.Threading

Class MainWindow
    Delegate Sub NextPrimeDelegate()

    'Current number to check 
    Private num As Long = 3

    Private continueCalculating As Boolean = False

    Private Sub StartOrStop(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If continueCalculating Then
            continueCalculating = False
            startStopButton.Content = "Resume"
        Else
            continueCalculating = True
            startStopButton.Content = "Stop"
            startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, New NextPrimeDelegate(AddressOf Me.CheckNextNumber))
        End If
    End Sub

    Public Sub CheckNextNumber()
        ' Reset flag.
        NotAPrime = False

        Dim i As Long = 3
        While i <= Math.Sqrt(num)
            If num Mod i = 0 Then
                ' Set not a prime flag to true.
                NotAPrime = True
                Exit While
            End If
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While

        ' If a prime number.
        If Not NotAPrime Then
            bigPrime.Text = num.ToString()
        End If

        num += 2
        If continueCalculating Then
            startStopButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, New NextPrimeDelegate(AddressOf Me.CheckNextNumber))
        End If
    End Sub

    Private NotAPrime As Boolean = False
End Class
