using System.Windows.Controls;

namespace CodeShare.MVVM.View
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            Navigate(new Register());
        }

        public void Navigate(Page page)
        {
            UserFrame.Navigate(page);
        }
    }
}
