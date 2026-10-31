using System.Windows;
using Snapnest.App.ViewModels;

namespace Snapnest.App;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}