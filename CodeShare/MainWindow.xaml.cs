using System.Windows;

namespace CodeShare
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DebugPos.Content = CodeShare.Properties.Settings.Default.ToolbarHorizontalOffset;
        }

        private void ToolbarSummonBtn_Click(object sender, RoutedEventArgs e)
        {
            App.OpenToolbarWindow();
        }
    }
}
