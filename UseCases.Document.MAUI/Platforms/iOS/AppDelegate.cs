using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Constants;
using DocumentSDK.MAUI.iOS;
using Foundation;

namespace UseCases.Document.MAUI;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => CreateApp();

    private MauiApp CreateApp()
    {
        SBSDKInitializer.Initialize(UIKit.UIApplication.SharedApplication, App.LICENSE_KEY, new SBSDKConfiguration
        {
            EnableLogging = true,
            StorageBaseDirectory = GetDemoStorageBaseDirectory(),
            StorageImageFormat = CameraImageFormat.Jpg,
            StorageImageQuality = 50,
            DetectorType = DocumentDetectorType.MLBased,

            // You can enable encryption by uncommenting the following lines:
            // Note: all the images and files exported through the SDK will
            // not be openable from an external application, since they will be
            // encrypted.

            // Encryption = new SBSDKEncryption
            // {
            //    Password = "SomeSecretPa$$w0rdForFileEncryption",
            //    Mode = EncryptionMode.AES256
            // }
        });

        return MauiProgram.CreateMauiApp();
    }

    string GetDemoStorageBaseDirectory()
    {
        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var folder = Path.Combine(documents, "forms-dev-app-storage");
        Directory.CreateDirectory(folder);

        return folder;
    }
}

