using ScanbotSDK.MAUI.Services;
using UseCases.Document.MAUI.ViewModels;

namespace UseCases.Document.MAUI.Pages;

public partial class SinglePagePreview : BasePage
{
    public SinglePagePreview(IScannedPage scannedPage)
    {
        InitializeComponent();

        ViewModel = new SinglePagePreviewViewModel(scannedPage);
        BindingContext = ViewModel;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel.BindingContextChanged();
    }
}
