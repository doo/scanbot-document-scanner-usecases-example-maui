using System.Collections.ObjectModel;
using ScanbotSDK.MAUI.Services;
using UseCases.Document.MAUI.UseCases;
using UseCases.Document.MAUI.Utils;

namespace UseCases.Document.MAUI.ViewModels
{
    public class MultiPagePreviewViewModel : BasePagePreviewViewModel
    {
        private IEnumerable<IScannedPage> _scannedPages;

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

        public MultiPagePreviewViewModel(IEnumerable<IScannedPage> scannedPages)
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
            if (!await ActionHelpers.IsLicenseValid())
                return;

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
            if (!await ActionHelpers.IsLicenseValid())
                return;

            var saveFormat = await ActionHelpers.ChooseDocumentSaveFormatOption();
            if (saveFormat == null) 
            {
                return;
            }

            var exportedFileUri = await FileGeneratorFactory.GenerateUseCaseByFileFormat(saveFormat.Value).GenerateFilesForDocument(_scannedPages);

            await ActionHelpers.ShareFile(exportedFileUri?.LocalPath);
        }
    }
}

