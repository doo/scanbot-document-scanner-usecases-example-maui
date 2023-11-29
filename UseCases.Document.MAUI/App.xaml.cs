namespace UseCases.Document.MAUI;

public partial class App : Application
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

    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new Pages.HomePage());
    }
}

