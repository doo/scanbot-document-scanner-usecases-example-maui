using System.Windows.Input;
using BarcodeSDK.MAUI.Constants;
using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;
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
            var config = new CroppingScreenConfiguration();
            var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchCroppingScreenAsync(_scannedPage, config);

            if (result.Status == OperationResult.Ok)
            {
                ScannedImageSource = await _scannedPage.DecryptedDocument();
            }
        }

        private async void DetectBlur()
        {
            var blur = await DocumentSDK.MAUI.ScanbotSDK.SDKService.EstimateBlurriness(await _scannedPage.DecryptedDocument());

            await App.Current.MainPage.DisplayAlert("Detect Blur", $"Estimated blurriness for detected document: {blur}", "Dismiss");
        }

        private async void Export()
        {
            var saveFormat = await ActionHelpers.ChooseDocumentSaveFormatOption();

            if (saveFormat == null)
                return;

            if (saveFormat == Models.SaveFormatOption.PDF)
            {
                var exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                    new[] { _scannedPage.Document },
                    PDFPageSize.FixedA4);

                await ActionHelpers.ShareFile(exportedFileUri.AbsolutePath);
            }
            else if (saveFormat == Models.SaveFormatOption.TIFF)
            {
                var exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                    new[] { _scannedPage.Document },
                    new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
                );

                await ActionHelpers.ShareFile(exportedFileUri.AbsolutePath);
            }
        }

    }
}

