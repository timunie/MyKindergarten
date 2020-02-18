using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace MyKindergarten.Model
{
    public static class DialogHelper
    {
        public static async void ShowErrorMessage(Exception e)
        {
            MetroWindow window = App.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);
            if (window is null)
            {
                MessageBox.Show(e.Message, Lang.General.ID);
            }
            else
            {
                await window.ShowMessageAsync(Lang.General.ID, e.Message, MessageDialogStyle.Affirmative);
            }
        }
    }
}
