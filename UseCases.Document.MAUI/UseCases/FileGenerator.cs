using ScanbotSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public abstract class FileGenerator
    {
        public abstract Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPage> scannedPages);

        protected IEnumerable<FileImageSource> ExtractImageSourcesFromScannedPages(IEnumerable<IScannedPage> scannedPages)
        {
            var list = new List<FileImageSource>();
            list.AddRange(scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document.ToFileImageSource()));
            return list;
        }
    }

    public static class ImageExtension
    {
        public static FileImageSource ToFileImageSource(this ImageSource source)
        {
            var fileImageSource = source as FileImageSource;
            return fileImageSource;
        }
    }
}

