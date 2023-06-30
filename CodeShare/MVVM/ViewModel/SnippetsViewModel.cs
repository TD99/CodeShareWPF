using CodeShare.Core;
using CodeShare.Properties;

namespace CodeShare.MVVM.ViewModel
{
    public class SnippetsViewModel : ObservableObject
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                if (_isLoggedIn == value) return;
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public SnippetsViewModel()
        {
            IsLoggedIn = !string.IsNullOrWhiteSpace(Settings.Default.CurrentUser);
        }
    }
}
