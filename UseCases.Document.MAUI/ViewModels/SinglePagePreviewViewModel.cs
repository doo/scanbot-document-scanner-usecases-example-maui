using System.Diagnostics;
using System.Windows.Input;
using BarcodeSDK.MAUI.Constants;
using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Services;
using UseCases.Document.MAUI.UseCases;
using UseCases.Document.MAUI.Utils;

namespace UseCases.Document.MAUI.ViewModels
{
    public class SinglePagePreviewViewModel : BasePagePreviewViewModel
    {
        private IScannedPageService _scannedPage;

        private ImageSource _scannedImageSource;
        public ImageSource ScannedImageSource
        {
            get => _scannedImageSource;
            set
            {
                if (_scannedImageSource != value)
                {
                    _scannedImageSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ManualCropCommand { get; set; }
        public ICommand DetectBlurCommand { get; set; }

        public SinglePagePreviewViewModel(IScannedPageService scannedPage)
        {
            _scannedPage = scannedPage;

            FilterCommand = new Command(Filter);
            ExportCommand = new Command(Export);
            ManualCropCommand = new Command(ManualCrop);
            DetectBlurCommand = new Command(DetectBlur);

        }

        public override async void BindingContextChanged()
        {
            base.BindingContextChanged();
            ScannedImageSource = await _scannedPage.DecryptedDocumentPreview();
        }

        private async void Filter()
        {
            if (!await ActionHelpers.IsLicenseValid())
                return;

            var filterOption = await ActionHelpers.ChooseDocumentFilterOption();

            if (filterOption == null)
            {
                return;
            }

            await _scannedPage.SetFilterAsync(filterOption.Value);
            ScannedImageSource = await _scannedPage.DecryptedDocument();
        }

        private async void ManualCrop()
        {
            if (!await ActionHelpers.IsLicenseValid())
                return;

            var config = new CroppingScreenConfiguration();
            var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchCroppingScreenAsync(_scannedPage, config);

            if (result.Status == OperationResult.Ok)
            {
                ScannedImageSource = await _scannedPage.DecryptedDocument();
            }
        }

        private async void DetectBlur()
        {
            if (!await ActionHelpers.IsLicenseValid())
                return;

            var blur = await DocumentSDK.MAUI.ScanbotSDK.SDKService.EstimateBlurriness(await _scannedPage.DecryptedDocument());

            await App.Current.MainPage.DisplayAlert("Detect Blur", $"Estimated blurriness for detected document: {blur}", "Dismiss");
        }

        private async void Export()
        {
            if (!await ActionHelpers.IsLicenseValid())
                return;

            var saveFormat = await ActionHelpers.ChooseDocumentSaveFormatOption();

            if (saveFormat == null)
                return;

            var exportedFileUri = await UseCaseCreator.GenerateUseCaseByFileFormat(saveFormat.Value).GenerateFilesForDocument(new[] { _scannedPage });

            await ActionHelpers.ShareFile(exportedFileUri?.LocalPath);
        }

    }
}