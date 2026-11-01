using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Snapnest.Application.Interfaces;
using Snapnest.App.Views.Capture;

namespace Snapnest.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IScreenshotService _screenshotService;
    private readonly IImageService _imageService;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    public MainViewModel(
        IServiceProvider serviceProvider, 
        IScreenshotService screenshotService, 
        IImageService imageService)
    {
        _serviceProvider = serviceProvider;
        _screenshotService = screenshotService;
        _imageService = imageService;
        
        // Load the initial dashboard view
        NavigateToHome();
    }

    [RelayCommand]
    private void NavigateToHome()
    {
        CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
    }

    [RelayCommand]
    private void NavigateToClotheslines()
    {
        CurrentViewModel = _serviceProvider.GetRequiredService<ClotheslinesViewModel>();
    }

    [RelayCommand]
    private async Task TakeScreenshotAsync()
    {
        // Explicitly use System.Windows.Application to avoid namespace collision with Snapnest.Application
        System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;

        // Allow a brief moment for the minimize animation to complete
        await Task.Delay(300);

        string tempPath = await _screenshotService.CaptureFullScreenAsync();

        // Restore the main window after the capture is complete
        System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;

        // Present the decision popup to the user
        var decisionWindow = new CaptureDecisionWindow(_screenshotService, _imageService, tempPath)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };
        
        decisionWindow.ShowDialog();
    }
}