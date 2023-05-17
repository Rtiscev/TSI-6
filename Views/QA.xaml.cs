using System.Windows.Controls;

namespace TSI_6.Views
{
    public partial class QA : UserControl
    {
        public string topicName { get; set; }
        public int iterationFolder { get; set; }
        public int iterationQuestion { get; set; }
        public string[] Files { get; set; }
        public int folderNumber { get; set; }

        public QA()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
