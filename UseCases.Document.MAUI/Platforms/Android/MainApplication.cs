using Android.App;
using Android.Runtime;
using ScanbotSDK.MAUI;
using ScanbotSDK.MAUI.Droid;

namespace UseCases.Document.MAUI;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => CreateMauiInstance();

    private MauiApp CreateMauiInstance()
    {
        var configuration = new SBSDKConfiguration
        {
            EnableLogging = true,
            AllowGpuAcceleration = false,
            AllowXnnpackAcceleration = false,
            EnableNativeLogging = true,
            StorageImageFormat = ScanbotSDK.MAUI.Constants.CameraImageFormat.Jpg,

            // You can enable encryption by uncommenting the following lines:
            // Note: all the images and files exported through the SDK will
            // not be openable from external application, since they will be
            // encrypted

            //Encryption = new SBSDKEncryption
            //{
            //    Password = "SomeSecretPa$$w0rdForFileEncryption",
            //    Mode = EncryptionMode.AES256
            //}
        };

        SBSDKInitializer.Initialize(this, App.LICENSE_KEY, configuration);

        return MauiProgram.CreateMauiApp();
    }
}

