﻿// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

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
using System.IO;
using System.Collections;

namespace ImageView
{
    /// <summary>
    /// Interaction logic for default.xaml
    /// </summary>
    public partial class ImageViewExample : Window
    {
	private ArrayList imageFiles;

        public ImageViewExample()
        {
            InitializeComponent();
        }

		private void WindowLoaded(object sender, EventArgs e)
		{
			imageFiles = GetImageFileInfo();
			imageListBox.DataContext = imageFiles;

		}

		private void showImage(object sender, SelectionChangedEventArgs args)
		{
			ListBox list = ((ListBox)sender);
			if (list != null)
			{
				int index = list.SelectedIndex;	//Save the selected index 
				if (index >= 0)
				{
					string selection = list.SelectedItem.ToString();

					if ((selection != null) && (selection.Length != 0))
					{
						//Set currentImage to selected Image
                  Uri selLoc = new Uri(selection);
                  BitmapImage id = new BitmapImage(selLoc);
						FileInfo currFileInfo = new FileInfo(selection);
						currentImage.Source = id;

						//Setup Info Text
						imageSize.Text = id.PixelWidth.ToString() + " x " + id.PixelHeight.ToString();
						imageFormat.Text = id.Format.ToString();
						fileSize.Text = ((currFileInfo.Length + 512) / 1024).ToString() + "k";

					}
				}
			}
		}

		private ArrayList GetImageFileInfo()
		{
			ArrayList imageFiles = new ArrayList();
			string[] files;

         //Get directory path of myData (down two directory levels)
         string currDir = Directory.GetCurrentDirectory();
         string temp = currDir + "\\..\\..\\myData";
			files = Directory.GetFiles(temp, "*.jpg");

			foreach(string image in files)
			{
				FileInfo info = new FileInfo(image);
				imageFiles.Add(info);
			}

			//imageFiles.Sort();

			return imageFiles;
		}
	}
}