using System.Windows.Controls;
using CodeShare.Properties;

namespace CodeShare.MVVM.View
{
    public partial class UserRouterPage : Page
    {
        public UserRouterPage(string type = nameof(Login))
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(Settings.Default.CurrentUser))
            {
                Navigate(new Account());
                return;
            }

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
