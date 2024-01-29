using ScanbotSDK.MAUI.Constants;
using ScanbotSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class PdfFileGenerator : FileGenerator
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPage> scannedPages)
        {
            var fileImageSources = ExtractImageSourcesFromScannedPages(scannedPages);
            return await ScanbotSDK.MAUI.ScanbotSDK.SDKService?.CreatePdfAsync(fileImageSources, PDFPageSize.A4, PDFPageOrientation.Portrait, ScanbotSDK.MAUI.ScanbotSDK.SDKService.DefaultOcrConfig);
        }
    }
}

