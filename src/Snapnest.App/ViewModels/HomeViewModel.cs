using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Snapnest.Application.Interfaces;
using Snapnest.Core.Entities;

namespace Snapnest.App.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IImageService _imageService;

    public ObservableCollection<ImageItem> ImportedImages { get; } = new();

    public HomeViewModel(IImageService imageService)
    {
        _imageService = imageService;
        _ = LoadImagesAsync();
    }

    [RelayCommand]
    private async Task LoadImagesAsync()
    {
        ImportedImages.Clear();
        var images = await _imageService.GetAllImagesAsync();
        foreach (var img in images)
        {
            ImportedImages.Add(img);
        }
    }

    [RelayCommand]
    private async Task ImportImagesAsync()
    {
        // Open Windows File Picker
        var openFileDialog = new OpenFileDialog
        {
            Title = "Import Images to Snapnest",
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.webp;*.bmp|All Files|*.*",
            Multiselect = true // Allow user to select multiple files
        };

        if (openFileDialog.ShowDialog() == true)
        {
            foreach (var filePath in openFileDialog.FileNames)
            {
                var imported = await _imageService.ImportImageAsync(filePath);
                if (imported != null)
                {
                    // Insert at the top of the list
                    ImportedImages.Insert(0, imported);
                }
            }
        }
    }

    [RelayCommand]
    private async Task DeleteImageAsync(ImageItem image)
    {
        if (image == null) return;
        
        await _imageService.DeleteImageAsync(image.Id);
        ImportedImages.Remove(image);
    }
}