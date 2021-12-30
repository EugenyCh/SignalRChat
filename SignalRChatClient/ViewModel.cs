using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SignalRChatClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string header;

        public string Header
        {
            get => header;
            set
            {
                header = value;
                OnPropertyChanged();
            }
        }

        private string userText;

        public string UserText
        {
            get => userText;
            set
            {
                userText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeHeader
        {
            get => new Command((_) =>
            {
                Header = $"User {UserText}";
            });
        }

        public ViewModel()
        {
            Header = "New SignalR Chat Client";
        }
    }

    public class Command : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}