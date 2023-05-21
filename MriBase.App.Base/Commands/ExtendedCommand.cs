using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MriBase.App.Base.Commands
{
    public class ExtendedCommand : Command, INotifyPropertyChanged
    {
        public ExtendedCommand(Action<object> execute) : base(execute)
        {
            this.CanExecuteChanged += ExtendedCommandCanExecuteChanged;
        }

        public ExtendedCommand(Action<object> execute, Func<object, bool> canExecute) : base(execute, canExecute)
        {
            this.CanExecuteChanged += ExtendedCommandCanExecuteChanged;
        }

        public ExtendedCommand(Action execute) : base(execute)
        {
            this.CanExecuteChanged += ExtendedCommandCanExecuteChanged;
        }

        public ExtendedCommand(Action execute, Func<bool> canExecute) : base(execute, canExecute)
        {
            this.CanExecuteChanged += ExtendedCommandCanExecuteChanged;
        }

        private void ExtendedCommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged(nameof(CanExecuteProperty));
        }

        public bool CanExecuteProperty { get => this.CanExecute(null); }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
