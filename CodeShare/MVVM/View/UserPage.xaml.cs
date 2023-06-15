using System.Windows.Controls;
using System.Windows.Navigation;

namespace CodeShare.MVVM.View
{
<<<<<<< HEAD
    /// <summary>
    /// Interaktionslogik für UserPage.xaml
    /// </summary>
=======
>>>>>>> ba86ed36611fa8e57bd6b493373a041dfc305473
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

        private void UserFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UserFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
    }
}
