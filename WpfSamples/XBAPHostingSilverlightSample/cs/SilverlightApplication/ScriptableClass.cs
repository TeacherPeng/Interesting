using System.Windows.Browser; // ScriptableMemberAttribute

namespace SilverlightApplication
{
    [ScriptableType]
    public class ScriptableClass
    {
        // Method that can be called from HTML script
        [ScriptableMember]
        public void SendMessageToSilverlight(string msg)
        {
            msg = "Your Silverlight application has recieved a message: " + msg;
            ((MainPage)App.Current.RootVisual).msgTextBlock.Text = msg;
            
        }

       
    }
}
