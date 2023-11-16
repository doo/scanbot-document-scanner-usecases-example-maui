using DocumentSDK.MAUI.Constants;
using UseCases.Document.MAUI.Models;

namespace UseCases.Document.MAUI.Utils
{
    public static class ActionHelpers
    {
        public static async Task<SaveFormatOption?> ChooseDocumentSaveFormatOption()
        {
            var parameters = Enum.GetNames(typeof(SaveFormatOption));

            string saveFormat = await App.Current.MainPage.DisplayActionSheet("Save Image as", "Cancel", null, parameters);

            if (saveFormat == null || saveFormat.Equals("Cancel"))
            {
                return null;
            }

            return Enum.Parse<SaveFormatOption>(saveFormat);
        }

        public static async Task<ImageFilter?> ChooseDocumentFilterOption()
        {
            var parameters = Enum.GetNames(typeof(ImageFilter));
            var filterOption = await App.Current.MainPage.DisplayActionSheet("Filter", "Cancel", null, parameters);

            if (filterOption == null || filterOption.Equals("Cancel"))
            {
                return null;
            }

            return Enum.Parse<ImageFilter>(filterOption);
        }

        public static async Task ShareFile(string filePath)
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share file",
                File = new ShareFile(filePath),
            });
        }

        public static async Task<bool> IsLicenseValid()
        {
            if (!DocumentSDK.MAUI.ScanbotSDK.SDKService.IsLicenseValid)
            {
                await App.Current.MainPage.DisplayAlert("Oops!", "License expired or invalid", "Dismiss");
                return false;
            }

            return true;
        }
    }
}

