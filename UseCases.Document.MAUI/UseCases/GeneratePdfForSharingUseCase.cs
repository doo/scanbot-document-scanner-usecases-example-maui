using System;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class GeneratePdfForSharingUseCase : GenerateFilesForSharingUseCase
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            return await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                           ExtractImageSourcesFromScannedPages(scannedPages),
                           PDFPageSize.FixedA4);
        }
    }
}

