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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FilterTuneWPF.Views
{
    /// <summary>
    /// Interaction logic for MainScreenView.xaml
    /// </summary>
    public partial class MainScreenView : UserControl
    {
        public MainScreenView()
        {
            InitializeComponent();
        }



        ////private void TemplateKeyDown(object sender, KeyEventArgs e)
        ////{
        ////    if (e.Key == Key.Delete && ((TextBox)sender).IsReadOnly)
        ////    {
        ////        MessageBox.Show($"Are you sure you want to delete {((TextBox)sender).Text}?");
        ////        e.Handled = true;
        ////    }
        ////}

        //private void EnableEditing(object sender, MouseButtonEventArgs e)
        //{
        //    if (((TextBox)sender).IsKeyboardFocused)
        //    {
        //        ((TextBox)sender).IsReadOnly = false;
        //    }    
        //}
        //private void DisableEditing(object sender, KeyboardEventArgs e)
        //{
        //    ((TextBox)sender).IsReadOnly = true;
        //}
    }
}
