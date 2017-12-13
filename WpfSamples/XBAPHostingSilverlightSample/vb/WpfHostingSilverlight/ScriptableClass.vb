Imports System.ComponentModel ' INotifyPropertyChanged
Imports System.Runtime.InteropServices ' ComVisibleAttribute
Imports System.Security 'Security Permissions
Imports System.Security.Permissions 'Security Permissions

Namespace WPFBrowserApplication
	<PermissionSet(SecurityAction.Assert, Name:="FullTrust"), ComVisible(True)> _
	Public Class ScriptableClass
		' Method that can be called from HTML script 
		Public Sub SendMessageToWPF(ByVal msg As String)
			MessageBox.Show("Your WPF application has received a message: " & msg)
		End Sub


	End Class
End Namespace
