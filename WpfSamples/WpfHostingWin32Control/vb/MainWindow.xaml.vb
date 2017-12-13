Imports System.Text
Imports System.Windows.Interop
Imports System.Runtime.InteropServices

Namespace WpfHostingWin32Control
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class HostWindow
		Inherits Window
		Private selectedItem As Integer
		Private hwndListBox As IntPtr
		Private listControl As WpfHostingWin32Control.ControlHost
		Private app As Application
		Private myWindow As Window
		Private itemCount As Integer

		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub On_UIReady(ByVal sender As Object, ByVal e As EventArgs)
			app = Application.Current
			myWindow = app.MainWindow
			myWindow.SizeToContent = SizeToContent.WidthAndHeight
			listControl = New ControlHost(ControlHostElement.ActualHeight, ControlHostElement.ActualWidth)
			ControlHostElement.Child = listControl
			AddHandler listControl.MessageHook, AddressOf ControlMsgFilter
			hwndListBox = listControl.hwndListBox
			For i As Integer = 0 To 14 'populate listbox
				Dim itemText As String = "Item" & i.ToString()
				SendMessage(hwndListBox, LB_ADDSTRING, IntPtr.Zero, itemText)
			Next i
			itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero)
			numItems.Text = "" & itemCount.ToString()
		End Sub

		Private Sub AppendText(ByVal sender As Object, ByVal args As EventArgs)
			If txtAppend.Text <> String.Empty Then
				SendMessage(hwndListBox, LB_ADDSTRING, IntPtr.Zero, txtAppend.Text)
			End If
			itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero)
			numItems.Text = "" & itemCount.ToString()
		End Sub
		Private Sub DeleteText(ByVal sender As Object, ByVal args As EventArgs)
			selectedItem = SendMessage(listControl.hwndListBox, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero)
			If selectedItem <> -1 Then 'check for selected item
				SendMessage(hwndListBox, LB_DELETESTRING, New IntPtr(selectedItem), IntPtr.Zero)
			End If
			itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero)
			numItems.Text = "" & itemCount.ToString()
		End Sub
		Private Function ControlMsgFilter(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByRef handled As Boolean) As IntPtr
			Dim textLength As Integer

			handled = False
			If msg = WM_COMMAND Then
				Select Case CUInt(wParam.ToInt32()) >> 16 And &HFFFF 'extract the HIWORD
					Case LBN_SELCHANGE 'Get the item text and display it
						selectedItem = SendMessage(listControl.hwndListBox, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero)
						textLength = SendMessage(listControl.hwndListBox, LB_GETTEXTLEN, IntPtr.Zero, IntPtr.Zero)
						Dim itemText As New StringBuilder()
						SendMessage(hwndListBox, LB_GETTEXT, selectedItem, itemText)
						selectedText.Text = itemText.ToString()
						handled = True
				End Select
			End If
			Return IntPtr.Zero
		End Function
		Friend Const LBN_SELCHANGE As Integer = &H00000001, WM_COMMAND As Integer = &H00000111, LB_GETCURSEL As Integer = &H00000188, LB_GETTEXTLEN As Integer = &H0000018A, LB_ADDSTRING As Integer = &H00000180, LB_GETTEXT As Integer = &H00000189, LB_DELETESTRING As Integer = &H00000182, LB_GETCOUNT As Integer = &H0000018B

		<DllImport("user32.dll", EntryPoint := "SendMessage", CharSet := CharSet.Unicode)>
		Friend Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
		End Function

		<DllImport("user32.dll", EntryPoint := "SendMessage", CharSet := CharSet.Unicode)>
		Friend Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As StringBuilder) As Integer
		End Function

		<DllImport("user32.dll", EntryPoint := "SendMessage", CharSet := CharSet.Unicode)>
		Friend Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As String) As IntPtr
		End Function

	End Class
End Namespace
