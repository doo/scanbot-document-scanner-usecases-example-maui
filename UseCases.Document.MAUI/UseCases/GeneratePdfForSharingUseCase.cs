using System;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class GeneratePdfForSharingUseCase : GenerateFilesForSharingUseCase
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            var documentSources = scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document)
                .ToList();

            return await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreatePdfAsync(
                           documentSources,
                           PDFPageSize.FixedA4);
        }
    }
}

