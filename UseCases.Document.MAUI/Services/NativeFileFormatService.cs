using System;
using Foundation;
using UseCases.Document.MAUI.Models;

#if __IOS__

using Foundation;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.PlatformConfiguration;
using UIKit;

#endif

namespace UseCases.Document.MAUI.Services
{
    public class NativeFileFormatService : IFileFormatService
    {
        public async Task<Uri> ConvertFromFileImageSourceAsync(SaveFormatOption formatOption, FileImageSource fileImage)
        {
            var saveFormatOptionString = formatOption.ToString().ToLower();
            var generatedFilePath = GetGeneratedFilePathAsync(fileImage.File, saveFormatOptionString);

            if (!IsFileSameFormat(fileImage.File, saveFormatOptionString))
            {
                await CopyFileAsync(fileImage.File, generatedFilePath);
            }

            return new Uri(generatedFilePath);
        }

        private string GetGeneratedFilePathAsync(string sourceFilePath, string formatOption)
        {
            if (IsFileSameFormat(sourceFilePath, formatOption))
            {
                return GetLocalPath(sourceFilePath);
            }
            else
            {
                var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{fileName}.{formatOption}");
            }
        }

        private bool IsFileSameFormat(string sourceFilePath, string formatOption)
        {
            var initialFileExtension = Path.GetExtension(sourceFilePath);
            return initialFileExtension.Contains(formatOption);
        }

        private string GetLocalPath(string sourceFilePath)
        {
#if __IOS__
            return NSUrl.FromFilename(sourceFilePath).Path;
#else
            return sourceFilePath;
#endif
        }

        private async Task CopyFileAsync(string sourceFilePath, string destinationFilePath)
        {
#if IOS
            using (var fileStream = new FileStream(destinationFilePath, FileMode.Create))
            {
                using (var sourceStream = File.OpenRead(sourceFilePath))
                {
                    await sourceStream.CopyToAsync(fileStream);
                }
            }
#elif ANDROID
            using (var fileStream = new FileStream(destinationFilePath, FileMode.Create))
            {
                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeFile(sourceFilePath);
                if (bitmap != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
                        var bytes = stream.ToArray();

                        using (var bufferedStream = new BufferedStream(fileStream, 4096))
                        {
                            await bufferedStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
#endif
        }
    }
}

