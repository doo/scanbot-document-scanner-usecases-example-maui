using System;
using UseCases.Document.MAUI.Models;

namespace UseCases.Document.MAUI.Services
{
    public interface IFileFormatService
    {
        Task<Uri> ConvertFromFileImageSourceAsync(SaveFormatOption formatOption, FileImageSource fileImage);
    }
}

