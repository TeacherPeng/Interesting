using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Animation_TimingBehaviors
{
    /// <summary>
    /// Interaction logic for SampleViewer.xaml
    /// </summary>
    public partial class SampleViewer : Page
    {
        public SampleViewer()
        {
            InitializeComponent();
        }
    }
    
    public class ElapsedTimeControl : Control
    {
    
        private Clock theClock;
        private Nullable<TimeSpan> previousTime;
    
        public ElapsedTimeControl()
        {
            
        }
        
        public Clock Clock
        {
            get { return theClock; }
            set
            {
                if (theClock != null)
                {
                    theClock.CurrentTimeInvalidated -= new EventHandler(onClockTimeInvalidated);    
                
                }
                
                theClock = value;
                
                if (theClock != null)
                {
                    theClock.CurrentTimeInvalidated += new EventHandler(onClockTimeInvalidated);
                    
                }
            
            }
        
        }
        
        private void onClockTimeInvalidated(object sender, EventArgs args)
        {
          

            SetValue(CurrentTimeProperty, theClock.CurrentTime);

        }
        
        
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                "CurrentTime", 
                typeof(Nullable<TimeSpan>), 
                typeof(ElapsedTimeControl),
                new FrameworkPropertyMetadata(
                    (Nullable<TimeSpan>)null, 
                    new PropertyChangedCallback(currentTime_Changed)));
                
        
        private static void currentTime_Changed(DependencyObject d, 
            DependencyPropertyChangedEventArgs args)
        {
        
            ((ElapsedTimeControl)d).SetValue(CurrentTimeAsStringProperty, args.NewValue.ToString());
        }
        

        public static readonly DependencyProperty CurrentTimeAsStringProperty =
            DependencyProperty.Register("CurrentTimeAsString", typeof(string), 
                typeof(ElapsedTimeControl));

        public string CurrentTimeAsString
        {
            get
            {
                return this.GetValue(CurrentTimeAsStringProperty) as string;
            }
            set
            {
                this.SetValue(CurrentTimeAsStringProperty, value);
            }
        }
              
        
    }

}
