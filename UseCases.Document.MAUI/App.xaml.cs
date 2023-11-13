namespace UseCases.Document.MAUI;

public partial class App : Application
{
	public const string LICENSE_KEY = null;

    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new Pages.HomePage());

		BarcodeSDK.MAUI.ScanbotBarcodeSDK.Initialize(new BarcodeSDK.MAUI.Models.InitializationOptions
        {
            LicenseKey = LICENSE_KEY,
            LoggingEnabled = true,
            ErrorHandler = (status, feature) =>
            {
                Console.WriteLine($"License error: {status}, {feature}");
            }
        });
	}
}

