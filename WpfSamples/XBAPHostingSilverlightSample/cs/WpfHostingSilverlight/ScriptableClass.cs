using System.ComponentModel; // INotifyPropertyChanged
using System.Windows; // MessageBox
using System.Runtime.InteropServices; // ComVisibleAttribute
using System.Security; //Security Permissions
using System.Security.Permissions; //Security Permissions

namespace WPFBrowserApplication
{
    [PermissionSet(SecurityAction.Assert, Name="FullTrust")]
    [ComVisible(true)]
    public class ScriptableClass
    {
        // Method that can be called from HTML script 
        public void SendMessageToWPF(string msg)
        {
            MessageBox.Show("Your WPF application has received a message: " + msg);
        }

       
    }
}
