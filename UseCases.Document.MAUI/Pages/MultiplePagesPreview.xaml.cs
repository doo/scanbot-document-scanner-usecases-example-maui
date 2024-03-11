using ScanbotSDK.MAUI.Services;
using UseCases.Document.MAUI.ViewModels;

namespace UseCases.Document.MAUI.Pages;

public partial class MultiplePagesPreview : BasePage
{
    public MultiplePagesPreview(IScannedPage[] scannedPages)
    {
        InitializeComponent();

        ViewModel = new MultiPagePreviewViewModel(scannedPages);
        BindingContext = ViewModel;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel.BindingContextChanged();
    }
}
