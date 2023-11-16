
namespace UseCases.Document.MAUI.UseCases
{
    public static class UseCaseCreator
    {
        public static GenerateFilesForSharingUseCase GenerateUseCaseByFileFormat(Models.SaveFormatOption saveFormatOption)
        {
            switch (saveFormatOption)
            {
                case Models.SaveFormatOption.JPG:
                case Models.SaveFormatOption.PNG:
                    return new GenerateImageForSharingUseCase(saveFormatOption);
                case Models.SaveFormatOption.PDF:
                    return new GeneratePdfForSharingUseCase();
                case Models.SaveFormatOption.TIFF:
                    return new GenerateTifforSharingUseCase();
                default:
                    return null;
            }
        }
    }
}

