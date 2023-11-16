using System;
using DocumentSDK.MAUI.Services;

namespace UseCases.Document.MAUI.UseCases
{
    public abstract class GenerateFilesForSharingUseCase
    {
        public abstract Task<Uri> GenerateFilesForDocument(IEnumerable<IScannedPageService> scannedPages);
    }
}

