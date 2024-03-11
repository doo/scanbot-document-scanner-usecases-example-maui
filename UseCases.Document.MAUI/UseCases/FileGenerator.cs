using ScanbotSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public abstract class FileGenerator
    {
        public abstract Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPage> scannedPages);

        protected IEnumerable<FileImageSource> ExtractImageSourcesFromScannedPages(IEnumerable<IScannedPage> scannedPages)
        {
            return scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document as FileImageSource);
        }
    }
}

