using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EWT.RTB.FirstOverview
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private string text;

        public ViewModel()
        {
            Text = "";
            DebugCommand = new RelayCommand(DebugCommand_CanExecute, DebugCommand_Executed);
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public ICommand DebugCommand { get; private set; }

        private bool DebugCommand_CanExecute(object obj)
        {
            return true;
        }

        private void DebugCommand_Executed(object obj)
        {
        }

        #region inpc

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion inpc
    }
}