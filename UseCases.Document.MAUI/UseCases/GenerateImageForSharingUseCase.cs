using DocumentSDK.MAUI.Services;
using UseCases.Document.MAUI.Models;
using UseCases.Document.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public class GenerateImageForSharingUseCase : GenerateFilesForSharingUseCase
    {
        private readonly IFileFormatService _fileFormatService;
        private readonly SaveFormatOption _saveFormat;

        public GenerateImageForSharingUseCase(SaveFormatOption saveFormat)
        {
            _fileFormatService = new NativeFileFormatService();
            _saveFormat = saveFormat;
        }

        public override async Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages)
        {
            foreach (var page in scannedPages)
            {
                if (page.Document is FileImageSource fileImageSource)
                {
                    return await _fileFormatService.ConvertFromFileImageSourceAsync(_saveFormat, fileImageSource);
                }
            }

            return null;
        }
    }
}

