using System.Collections.ObjectModel;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;
using UseCases.Document.MAUI.Utils;

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
            var filterOption = await ActionHelpers.ChooseDocumentFilterOption();

            if (filterOption == null)
            {
                return;
            }

            ScannedImageSources.Clear();

            foreach (var page in _scannedPages)
            {
                await page.SetFilterAsync(filterOption.Value);
                ScannedImageSources.Add(await page.DecryptedDocumentPreview());
            }
        }

        private async void Export()
        {
            var saveFormat = await ActionHelpers.ChooseDocumentSaveFormatOption();

            if (saveFormat == null)
                return;

            var documentSources = _scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document)
                .ToList();

            if (saveFormat == Models.SaveFormatOption.PDF)
            {
                var exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                    documentSources,
                    PDFPageSize.FixedA4);

                await ActionHelpers.ShareFile(exportedFileUri.AbsolutePath);
            }
            else if (saveFormat == Models.SaveFormatOption.TIFF)
            {
                var exportedFileUri = await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                    documentSources,
                    new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
                );

                await ActionHelpers.ShareFile(exportedFileUri.AbsolutePath);
            }
        }
    }
}

