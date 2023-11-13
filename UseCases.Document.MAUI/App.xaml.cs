namespace UseCases.Document.MAUI;

public partial class App : Application
{
	public const string LICENSE_KEY = null;

    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new Pages.HomePage());
	}
}

