using System.Collections.ObjectModel;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.ViewModels
{
	public class MultiPagePreviewViewModel : BasePagePreviewViewModel
    {
        private IEnumerable<IScannedPageService> _scannedPages;

        private ObservableCollection<ImageSource> _scannedImageSources;
        public ObservableCollection<ImageSource> ScannedImageSources
        {
            get => _scannedImageSources;
            set
            {
                if (_scannedImageSources != value)
                {
                    _scannedImageSources = value;
                    OnPropertyChanged();
                }
            }
        }

        public MultiPagePreviewViewModel(IEnumerable<IScannedPageService> scannedPages)
        {
            _scannedPages = scannedPages;

            FilterCommand = new Command(Filter);
            ExportCommand = new Command(Export);

            ScannedImageSources = new ObservableCollection<ImageSource>();
        }

        public override async void BindingContextChanged()
        {
            base.BindingContextChanged();
            await InitScannedImageSources();
        }

        private async Task InitScannedImageSources()
        {
            foreach (var page in _scannedPages)
            {
                var imageSource = await page.DecryptedDocumentPreview();
                ScannedImageSources.Add(imageSource);
            }
        }

        private async void Filter()
        {
            var buttons = Enum.GetNames(typeof(ImageFilter));
            var action = await App.Current.MainPage.DisplayActionSheet("Filter", "Cancel", null, buttons);

            if (Enum.TryParse<ImageFilter>(action, out var filter))
            {
                ScannedImageSources.Clear();

                foreach (var page in _scannedPages)
                {
                    await page.SetFilterAsync(filter);
                    ScannedImageSources.Add(await page.DecryptedDocumentPreview());
                }
            }
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
            var documentSources = _scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document)
                .ToList();

            if (action.Equals(parameters[0]))
            {
                exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                    documentSources,
                    PDFPageSize.FixedA4);

                await ShareFile(exportedFileUri.AbsolutePath);
            }
            else if (action.Equals(parameters[1]))
            {
                exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                    documentSources,
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

