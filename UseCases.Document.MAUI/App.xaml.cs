namespace UseCases.Document.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new Pages.HomePage());
    }
}

