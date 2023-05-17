using System;
using System.Windows;
using System.Windows.Controls;

namespace TSI_6.Views
{
    public class RadioEventArgs : EventArgs
    {
        public string selectedItem { get; set; }
        public RadioEventArgs(string data)
        {
            selectedItem = data;
        }
    }
    public partial class Category : UserControl
    {
        public int number { get; set; }
        public string question { get; set; }
        public string imgPath { get; set; }
        public string category { get; set; }
        private string selected { get; set; }

        public event EventHandler<RadioEventArgs> next;

        public Category()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void nextQ(object sender, RoutedEventArgs e)
        {
            onClick();
        }

        protected virtual void onClick()
        {
            //if (selected != null)
            //{
                next?.Invoke(this, new RadioEventArgs(selected));
            //}
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null && radioButton.IsChecked == true)
            {
                selected = radioButton.Content.ToString();
            }
        }
    }
}
