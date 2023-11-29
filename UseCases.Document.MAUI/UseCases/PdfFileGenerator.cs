using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class PdfFileGenerator : FileGenerator
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            return await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                           ExtractImageSourcesFromScannedPages(scannedPages),
                           PDFPageSize.FixedA4);
        }
    }
}

