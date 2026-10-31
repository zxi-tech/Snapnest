using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Snapnest.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
}