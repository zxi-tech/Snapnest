using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Snapnest.Application.Interfaces;
using Snapnest.Core.Entities;

namespace Snapnest.App.ViewModels;

public partial class ClotheslinesViewModel : ViewModelBase
{
    private readonly IClotheslineService _clotheslineService;

    [ObservableProperty]
    private string _newClotheslineName = string.Empty;

    public ObservableCollection<Clothesline> Clotheslines { get; } = new();

    public ClotheslinesViewModel(IClotheslineService clotheslineService)
    {
        _clotheslineService = clotheslineService;
        _ = LoadClotheslinesAsync();
    }

    [RelayCommand]
    private async Task LoadClotheslinesAsync()
    {
        Clotheslines.Clear();
        var items = await _clotheslineService.GetAllAsync();
        foreach (var item in items)
        {
            Clotheslines.Add(item);
        }
    }

    [RelayCommand]
    private async Task CreateClotheslineAsync()
    {
        if (string.IsNullOrWhiteSpace(NewClotheslineName)) return;

        var created = await _clotheslineService.CreateAsync(NewClotheslineName.Trim());
        Clotheslines.Add(created);
        NewClotheslineName = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteClotheslineAsync(Clothesline clothesline)
    {
        if (clothesline == null) return;

        await _clotheslineService.DeleteAsync(clothesline.Id);
        Clotheslines.Remove(clothesline);
    }
}