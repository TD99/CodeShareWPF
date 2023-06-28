using System.Windows.Controls;

namespace CodeShare.MVVM.View
{
    public partial class UserRouterPage : Page
    {
        public UserRouterPage(string type = nameof(View.Login))
        {
            InitializeComponent();

            switch (type)
            {
                case nameof(Register):
                    Navigate(new Register());
                    break;
                case nameof(Login):
                    Navigate(new Login());
                    break;
            }
        }

        public void Navigate(Page page)
        {
            UserFrame.Navigate(page);
        }
    }
}
