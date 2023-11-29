namespace UseCases.Document.MAUI.UseCases
{
    public static class FileGeneratorFactory
    {
        public static FileGenerator GenerateUseCaseByFileFormat(Models.SaveFormatOption saveFormatOption)
        {
            switch (saveFormatOption)
            {
                case Models.SaveFormatOption.JPG:
                case Models.SaveFormatOption.PNG:
                    return new ImageFileGenerator(saveFormatOption);
                case Models.SaveFormatOption.PDF:
                    return new PdfFileGenerator();
                case Models.SaveFormatOption.TIFF:
                    return new TiffFileGenerator();
                default:
                    return null;
            }
        }
    }
}

