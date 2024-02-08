﻿using ScanbotSDK.MAUI;
using ScanbotSDK.MAUI.Constants;

namespace UseCases.Document.MAUI;

public static class MauiProgram
{
    /*
     * TODO Add the Scanbot SDK license key here.
     * Please note: The Scanbot SDK will run without a license key for one minute per session!
     * After the trial period is over all Scanbot SDK functions as well as the UI components will stop working.
     * You can get an unrestricted "no-strings-attached" 7 days trial license key for free.
     * Please submit the trial license form (https://scanbot.io/en/sdk/demo/trial) on our website by using
     * the app identifier "io.scanbot.example.sdk.maui" of this example app.
    */
    public const string LICENSE_KEY = null;

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        SBSDKInitializer.Initialize(LICENSE_KEY, new SBSDKConfiguration
        {
            EnableLogging = true,
            StorageImageFormat = CameraImageFormat.Jpg,
#if ANDROID
            AllowGpuAcceleration = false,
            AllowXnnpackAcceleration = false,
            EnableNativeLogging = true,
#endif
            // You can enable encryption by uncommenting the following lines:
            //Encryption = new SBSDKEncryption
            //{
            //    Password = "SomeSecretPa$$w0rdForFileEncryption",
            //    Mode = EncryptionMode.AES256
            //}
            // Note: all the images and files exported through the SDK will
            // not be openable from external application, since they will be
            // encrypted.
        });

        return builder.Build();
	}
}

