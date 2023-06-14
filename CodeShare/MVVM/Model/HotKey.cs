using System.Windows.Input;

namespace CodeShare.MVVM.Model
{
    public class HotKey
    {
        public string Name { get; }
        public ModifierKeys Modifiers { get; }
        public Key Key { get; }

        public HotKey(string name, ModifierKeys modifiers, Key key)
        {
            Name = name;
            Modifiers = modifiers;
            Key = key;
        }
    }
}
