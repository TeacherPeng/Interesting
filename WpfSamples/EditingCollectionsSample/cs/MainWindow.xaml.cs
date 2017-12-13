﻿using System;
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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace EditingCollectionsSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
       private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (itemsControl.SelectedItem == null)
            {
                MessageBox.Show("No item is selected");
                return;
            }

            IEditableCollectionView editableCollectionView =
                        itemsControl.Items as IEditableCollectionView;

            // Create a window that prompts the user to edit an item.
            ChangeItemWindow win = new ChangeItemWindow();
            editableCollectionView.EditItem(itemsControl.SelectedItem);
            win.DataContext = itemsControl.SelectedItem;

            // If the user submits the new item, commit the changes.
            // If the user cancels the edits, discard the changes. 
            if ((bool)win.ShowDialog())
            {
                editableCollectionView.CommitEdit();
            }
            else
            {
                editableCollectionView.CancelEdit();
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            IEditableCollectionView editableCollectionView = itemsControl.Items as IEditableCollectionView; 
            
            if (!editableCollectionView.CanAddNew)
            {
                MessageBox.Show("You cannot add items to the list.");
                return;
            }

            // Create a window that prompts the user to enter a new
            // item to sell.
            ChangeItemWindow win = new ChangeItemWindow();

            //Create a new item to be added to the collection.
            win.DataContext = editableCollectionView.AddNew();

            // If the user submits the new item, commit the new
            // object to the collection.  If the user cancels 
            // adding the new item, discard the new item.
            if ((bool)win.ShowDialog())
            {
                editableCollectionView.CommitNew();
            }
            else
            {
                editableCollectionView.CancelNew();
            }

        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            PurchaseItem item = itemsControl.SelectedItem as PurchaseItem;

            if (item == null)
            {
                MessageBox.Show("No Item is selected");
                return;
            }

            IEditableCollectionView editableCollectionView = 
                    itemsControl.Items as IEditableCollectionView; 
            
            if (!editableCollectionView.CanRemove)
            {
                MessageBox.Show("You cannot remove items from the list.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove " + item.Description,
                                "Remove Item", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                editableCollectionView.Remove(itemsControl.SelectedItem);
            }
        }
    }

    // PurchaseItem implements INotifyPropertyChanged so that the 
    // application is notified when a property changes.  It 
    // implements IEditableObject so that pending changes can be discarded.
    public class PurchaseItem : INotifyPropertyChanged, IEditableObject
    {
        struct ItemData
        {
            internal string Description;
            internal double Price;
            internal DateTime OfferExpires;
        }
        ItemData copyData;
        ItemData currentData;

        public PurchaseItem()
            : this("New item", 0, DateTime.Now)
        {
        }

        public PurchaseItem(string desc, double price, DateTime endDate)
        {
            Description = desc;
            Price = price;
            OfferExpires = endDate;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1:c}, {2:D}", Description, Price, OfferExpires);
        }

        public string Description
        {
            get { return currentData.Description; }
            set
            {
                if (currentData.Description != value)
                {
                    currentData.Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public double Price
        {
            get { return currentData.Price; }
            set
            {
                if (currentData.Price != value)
                {
                    currentData.Price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public DateTime OfferExpires
        {
            get { return currentData.OfferExpires; }
            set
            {
                if (value != currentData.OfferExpires)
                {
                    currentData.OfferExpires = value;
                    NotifyPropertyChanged("OfferExpires");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            copyData = currentData;
        }

        public void CancelEdit()
        {
            currentData = copyData;
            NotifyPropertyChanged("");

        }

        public void EndEdit()
        {
            copyData = new ItemData();

        }

        #endregion

    }

    public class ItemsForSale : ObservableCollection<PurchaseItem>
    {
        public ItemsForSale()
        {
            Add((new PurchaseItem("Snowboard and bindings", 120, new DateTime(2009, 1, 1))));
            Add((new PurchaseItem("Inside C#, second edition", 10, new DateTime(2009, 2, 2))));
            Add((new PurchaseItem("Laptop - only 1 year old", 499.99, new DateTime(2009, 2, 28))));
            Add((new PurchaseItem("Set of 6 chairs", 120, new DateTime(2009, 2, 28))));
            Add((new PurchaseItem("My DVD Collection", 15, new DateTime(2009, 1, 1))));
            Add((new PurchaseItem("TV Drama Series", 39.985, new DateTime(2009, 1, 1))));
            Add((new PurchaseItem("Squash racket", 60, new DateTime(2009, 2, 28))));
        }

    }
}
