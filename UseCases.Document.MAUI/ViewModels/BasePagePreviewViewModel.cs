using System.Windows.Input;

namespace UseCases.Document.MAUI.ViewModels
{
	public class BasePagePreviewViewModel : BaseViewModel
	{
        public ICommand FilterCommand { get; set; }
        public ICommand ExportCommand { get; set; }
    }
}

