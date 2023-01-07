using System.Windows;

namespace CrossStruc
{
    public partial class ProgressBarWindow : Window
    {
        public ProgressBarWindow()
        {
            InitializeComponent();
        }

        public void UpdateProgress(double current)
        {
            progressBar.Value = current;
        }
    }
}
