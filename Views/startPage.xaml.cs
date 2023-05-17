using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TSI_6.Views
{
    /// <summary>
    /// Interaction logic for startPage.xaml
    /// </summary>
    /// 
    public class MyEventArgs : EventArgs
    {
        public Dictionary<string, int> MyData { get; set; }

        public MyEventArgs(Dictionary<string, int> data)
        {
            MyData = data;
        }
    }

    public partial class startPage : UserControl
    {
        public Dictionary<string, int> Map { get; private set; }

        public event EventHandler<MyEventArgs> proceed;
        public int[] options { get; set; }
        public startPage()
        {
            options = new int[] { 1, 2, 3, 4, 5 };
            InitializeComponent();
            DataContext = this;
        }

        public void goBack(object sender, RoutedEventArgs e)
        {
            Map = new Dictionary<string, int>();
            Map.Add("Аппаратные средства", Convert.ToInt32(hardwareChoice.SelectedItem.ToString()));
            Map.Add("Программное обеспечение", Convert.ToInt32(softwareChoice.SelectedItem.ToString()));
            Map.Add("Сеть и коммуникация", Convert.ToInt32(networkChoice.SelectedItem.ToString()));
            Map.Add("Персонал", Convert.ToInt32(personalChoice.SelectedItem.ToString()));
            Map.Add("Место функционирования организации", Convert.ToInt32(locationChoice.SelectedItem.ToString()));

            OnFinish();
        }

        protected virtual void OnFinish()
        {
            proceed?.Invoke(this, new MyEventArgs(Map));
        }
    }
}
