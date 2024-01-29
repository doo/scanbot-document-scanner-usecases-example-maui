using ScanbotSDK.MAUI.Constants;
using ScanbotSDK.MAUI.Models;
using ScanbotSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class TiffFileGenerator : FileGenerator
    {
        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPage> scannedPages)
        {
            return await ScanbotSDK.MAUI.ScanbotSDK.SDKService.WriteTiffAsync(
                   ExtractImageSourcesFromScannedPages(scannedPages),
                   new TiffOptions { OneBitEncoded = true, Dpi = 300, Compression = TiffCompressionOptions.CompressionCcittT6 }
               );
        }
    }
}
