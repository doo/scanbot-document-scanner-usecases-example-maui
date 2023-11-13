using System.Diagnostics;
using System.Windows.Input;
using BarcodeSDK.MAUI.Constants;
using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;

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
            var buttons = Enum.GetNames(typeof(ImageFilter));
            var action = await App.Current.MainPage.DisplayActionSheet("Filter", "Cancel", null, buttons);

            if (Enum.TryParse<ImageFilter>(action, out var filter))
            {
                await _scannedPage.SetFilterAsync(filter);
                ScannedImageSource = await _scannedPage.DecryptedDocument();
            }
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
            var parameters = new string[] { "Save PDF", "Save TIFF" };

            string action = await App.Current.MainPage.DisplayActionSheet("Save Image as", "Cancel", null, parameters);

            if (action == null || action.Equals("Cancel"))
            {
                return;
            }
                               
            Uri exportedFileUri;

            if (action.Equals(parameters[0]))
            {
                exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                    new[] { _scannedPage.Document },
                    PDFPageSize.FixedA4);

                await ShareFile(exportedFileUri.AbsolutePath);
            }
            else if (action.Equals(parameters[1]))
            {
                exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                    new[] { _scannedPage.Document },
                    new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
                );

                await ShareFile(exportedFileUri.AbsolutePath);
            }

            async Task ShareFile(string filePath)
            {
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share file",
                    File = new ShareFile(filePath)
                });
            }
        }

    }
}

