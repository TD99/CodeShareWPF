using System.Windows;

namespace CodeShare
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

        private void ToolbarSummonBtn_Click(object sender, RoutedEventArgs e)
        {
            new ToolbarWindow().Show();
        }
    }
}
