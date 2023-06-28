using CodeShare.Core;
using CodeShare.MVVM.Model;

namespace CodeShare.MVVM.ViewModel
{
    public class ToolbarViewModel : ObservableObject
    {
        private string? _code;
        public string? Code
        {
            get => _code;
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged(nameof(Code));
                OnPropertyChanged(nameof(DisplayCode));
            }
        }

        public string? DisplayCode => TextTools.GetFirstLines(TextTools.RemoveBlankLinesBeforeContent(_code), 2);

        private string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                if (_title  == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string? _path;
        public string? Path
        {
            get => _path;
            set
            {
                if (_path == value) return;
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        private Language? _language;
        public Language? Language
        {
            get => _language;
            set
            {
                if (_language == value) return;
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
    }
}
