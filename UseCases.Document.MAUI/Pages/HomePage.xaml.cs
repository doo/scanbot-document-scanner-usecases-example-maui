using BarcodeSDK.MAUI.Constants;
using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Models;
using UseCases.Document.MAUI.Models;
using UseCases.Document.MAUI.Utils;

namespace UseCases.Document.MAUI.Pages;

public partial class HomePage : BasePage
{
    private List<UseCaseOption> documentUseCases;

    private const int rowHeight = 45;

    public HomePage()
    {
        InitializeComponent();

        documentUseCases = new List<UseCaseOption>
        {
            new UseCaseOption("Single-Page Scanning", RunSinglePageScanner),
            new UseCaseOption("Multiple-Page Scanning", RunMultiPageScanner),
            new UseCaseOption("Single-Page Scanning with Finder", RunFinderPageScanner),
            new UseCaseOption("Pick from Gallery", ImportImagesFromLibrary),
        };

        DocumentUseCaseList.ItemsSource = documentUseCases;

        // size fix for iOS
        DocumentUseCaseList.HeightRequest = rowHeight * (documentUseCases.Count + 1); // + 1 for the header,
    }

    private async Task RunSinglePageScanner()
    {
        var config = new DocumentScannerConfiguration()
        {
            MultiPageEnabled = false,
            MultiPageButtonHidden = true,
            IgnoreBadAspectRatio = true,
        };

        var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchDocumentScannerAsync(config);

        if (result?.Status == OperationResult.Ok)
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.FirstOrDefault()));
        }
    }

    private async Task RunSinglePageAutoScanner()
    {
        var config = new DocumentScannerConfiguration()
        {
            AutoSnappingEnabled = true,
            AutoSnappingButtonHidden = false,
            AutoSnappingButtonTitle = "Auto-Snap"
        };

        var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchDocumentScannerAsync(config);

        if (result?.Status == OperationResult.Ok)
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.FirstOrDefault()));
        }
    }

    private async Task RunSinglePageScannerWithGuidance()
    {
        var config = new DocumentScannerConfiguration()
        {
           MultiPageEnabled = false,
           MultiPageButtonHidden = true,
           TextHintBadAngles = "Hold your phone parallel to the document",
           TextHintOK = "Hold your phone, steady, trying to scan",
           TextHintBadAspectRatio = "The document is not in the correct format",
           TextHintTooDark = "Its too dark, please add more light",
           TextHintTooSmall = "Document too small, please move closer",
           TextHintTooNoisy = "Image too noisy, please move to a better lit area",
           TextHintNothingDetected = "No document detected, please try again",
        };

        var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchDocumentScannerAsync(config);

        if (result?.Status == OperationResult.Ok)
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.FirstOrDefault()));
        }
    }

    private async Task RunMultiPageScanner()
    {
        var config = new DocumentScannerConfiguration()
        {
            MultiPageEnabled = true,
            MultiPageButtonHidden = false,
            ShutterButtonAutoInnerColor = SBColors.ScanbotRed,
            ShutterButtonManualInnerColor = SBColors.ScanbotRed,
        };

        var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchDocumentScannerAsync(config);

        if (result?.Status != OperationResult.Ok)
        {
            return;
        }


        if (result.Pages.Count() > 1)
        {
            await WaitThenNavigate(new MultiplePagesPreview(result.Pages));
        }
        else
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.First()));
        }
    }

    private async Task RunFinderPageScanner()
    {
        var config = new FinderDocumentScannerConfiguration()
        {
            FinderAspectRatio = new AspectRatio(21.0, 29.7),
            LockDocumentAspectRatioToFinder = true,
            AcceptedSizeScore = 0.75,
        };

        var result = await DocumentSDK.MAUI.ScanbotSDK.ReadyToUseUIService.LaunchFinderDocumentScannerAsync(config);

        if (result?.Status == OperationResult.Ok)
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.FirstOrDefault()));
        }
    }

    private async Task ImportImagesFromLibrary()
    {
        ImageSource pickedImageSource = await DocumentSDK.MAUI.ScanbotSDK.PickerService.PickImageAsync();

        if (pickedImageSource == null)
            return;

        var scannedPage = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreateScannedPageAsync(pickedImageSource);

        if (scannedPage != null)
        {
            await scannedPage.DetectDocumentAsync();

            await WaitThenNavigate(new SinglePagePreview(scannedPage));
        }
    }

    private async void UseCaseSelected(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e?.CurrentSelection.FirstOrDefault() is UseCaseOption service && service.NavigationAction != null)
        {
            if (!await ActionHelpers.IsLicenseValid())
                return;

            await service.NavigationAction();
        }

        if (sender is CollectionView listView)
        {
            listView.SelectedItem = null;
        }
    }

    async Task WaitThenNavigate(Page page)
    {
#if ANDROID
        await Task.Delay(150);
#endif
        await Navigation.PushAsync(page);
    }
}
