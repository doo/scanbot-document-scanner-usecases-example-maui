﻿using System.Text;
using BarcodeSDK.MAUI.Constants;
using DocumentSDK.MAUI;
using DocumentSDK.MAUI.Models;
using UseCases.Document.MAUI.Models;

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

        if (result.Status == OperationResult.Ok &&
            result?.Pages != null)
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

        if (result.Status == OperationResult.Ok &&
            result?.Pages != null)
        {
            await WaitThenNavigate(new MultiplePagesPreview(result.Pages));
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

        if (result?.Pages != null)
        {
            await WaitThenNavigate(new SinglePagePreview(result.Pages.FirstOrDefault()));
        }
    }

    private async Task ImportImagesFromLibrary()
    {
        ImageSource pickedImageSource = await DocumentSDK.MAUI.ScanbotSDK.PickerService.PickImageAsync();

        var scannedPage = await DocumentSDK.MAUI.ScanbotSDK.SDKService.CreateScannedPageAsync(pickedImageSource);

        if (scannedPage != null)
        {
            await WaitThenNavigate(new SinglePagePreview(scannedPage));
        }
    }

    private async void UseCaseSelected(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e?.CurrentSelection.FirstOrDefault() is UseCaseOption service && service.NavigationAction != null)
        {
            if (!DocumentSDK.MAUI.ScanbotSDK.SDKService.IsLicenseValid)
            {
                await DisplayAlert("Oops!", "License expired or invalid", "Dismiss");
                return;
            }

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
