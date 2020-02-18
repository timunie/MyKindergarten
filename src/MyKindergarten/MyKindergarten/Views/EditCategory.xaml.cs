using MahApps.Metro.Controls;
using MyKindergarten.Model;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyKindergarten.Views
{
    /// <summary>
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : MetroWindow
    {
        Category Category => DataContext as Category;

        public EditCategory()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btb_SaveAndExitClick(object sender, RoutedEventArgs e)
        {
            if (await Category.Save())
            {
                Close();
            }
        }

        private void cmbParentCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            
            // Add Category at the correct point
            if (e.AddedItems.Count == 1 && e.AddedItems[0] is Category parent)
            {
                if (parent != Category.Parent) 
                    parent.SubCategories.Add(Category);
            }
            else if(e.AddedItems.Count == 0)
            {
                ViewModel.Categories.Add(Category);
            }

            // remove Category from the correct point
            if (e.RemovedItems.Count == 1 && e.RemovedItems[0] is Category oldParent)
            {
                oldParent.SubCategories.Remove(Category);
            }
            else if (e.RemovedItems.Count == 0)
            {
                ViewModel.Categories.Remove(Category);
            }

        }
    }
}
