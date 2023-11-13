using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UseCases.Document.MAUI.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {
        }

        public virtual void BindingContextChanged() { }

        public virtual void OnAppearing() { }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

