using System;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class GenerateTifforSharingUseCase : GenerateFilesForSharingUseCase
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            var documentSources = scannedPages
                .Where(p => p.Document != null)
                .Select(p => p.Document)
                .ToList();

            return await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                   documentSources,
                   new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
               );
        }
    }
}

