using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Snapnest.Application.Interfaces;

namespace Snapnest.App.Views.Capture;

public partial class CaptureDecisionWindow : Window
{
    private readonly IScreenshotService _screenshotService;
    private readonly IImageService _imageService;
    private readonly string _tempFilePath;

    public CaptureDecisionWindow(IScreenshotService screenshotService, IImageService imageService, string tempFilePath)
    {
        InitializeComponent();
        _screenshotService = screenshotService;
        _imageService = imageService;
        _tempFilePath = tempFilePath;

        LoadPreview();
    }

    private void LoadPreview()
    {
        // Use BitmapCacheOption.OnLoad so the file is not locked by the UI!
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.UriSource = new Uri(_tempFilePath);
        bitmap.EndInit();
        PreviewImage.Source = bitmap;
    }

    private void BtnSaveNormally_Click(object sender, RoutedEventArgs e)
    {
        string fileName = Path.GetFileName(_tempFilePath);
        string destination = Path.Combine(_screenshotService.GetNormalSaveDirectory(), fileName);
        
        File.Copy(_tempFilePath, destination, overwrite: true);
        _screenshotService.DiscardTempImage(_tempFilePath); // Hapus file temporary
        
        MessageBox.Show($"Saved to Pictures/Snapnest:\n{fileName}", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private async void BtnAddToClothesline_Click(object sender, RoutedEventArgs e)
    {
        // Import file temporary ke dalam library Snapnest
        await _imageService.ImportImageAsync(_tempFilePath);
        _screenshotService.DiscardTempImage(_tempFilePath); // Hapus file temporary
        
        MessageBox.Show("Image added to Snapnest Library!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private void BtnDiscard_Click(object sender, RoutedEventArgs e)
    {
        _screenshotService.DiscardTempImage(_tempFilePath);
        Close();
    }
}