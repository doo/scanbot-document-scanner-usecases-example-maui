using System;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public abstract class GenerateFilesForSharingUseCase
    {
        public abstract Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages);

        protected List<ImageSource> ExtractImageSourcesFromScannedPages(IEnumerable<IScannedPageService> scannedPages)
        {
            return scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document)
                .ToList();

        }
    }
}

