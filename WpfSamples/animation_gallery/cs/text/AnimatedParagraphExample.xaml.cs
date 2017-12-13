// Copyright ?Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Data;


namespace Microsoft.Samples.Animation.AnimationGallery
{


    public partial class AnimatedParagraphExample : Page
    {
    
        
        public AnimatedParagraphExample()
        {
                  
        
        }
        
        
        // Updates the center of the RotateTransform used to rotate
        // the TextBlock's characters.
        private void textBlockSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (args != null && !args.NewSize.IsEmpty){
                
                TextEffectRotateTransform.CenterX = args.NewSize.Width / 2;
                TextEffectRotateTransform.CenterY = args.NewSize.Height / 2;
                   
            } 
        
        }
        

    }


}
