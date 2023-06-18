using System;
using System.Windows.Controls;

namespace CodeShare.MVVM.Model
{
    public class ControlException : Exception
    {
        public Control Control { get; set; }

        public ControlException(Control control, string message) : base(message)
        {
            Control = control;
        }
    }
}