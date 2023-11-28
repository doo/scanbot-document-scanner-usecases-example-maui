using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.Models;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class TiffFileGenerator : FileGenerator
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            return await DocumentSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                   ExtractImageSourcesFromScannedPages(scannedPages),
                   new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
               );
        }
    }
}

