using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Snapnest.Infrastructure.Data;
using Snapnest.App.ViewModels;
using Snapnest.Application.Interfaces;
using Snapnest.Infrastructure.Services;

namespace Snapnest.App;

public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging => { logging.ClearProviders(); logging.AddDebug(); })
            .ConfigureServices((context, services) =>
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appDataFolder = Path.Combine(localAppData, "Snapnest", "Database");
                Directory.CreateDirectory(appDataFolder);
                string dbPath = Path.Combine(appDataFolder, "snapnest.db");

                services.AddDbContext<SnapnestDbContext>(options => { options.UseSqlite($"Data Source={dbPath}"); });

                services.AddScoped<IClotheslineService, ClotheslineService>();

                services.AddScoped<IImageService, ImageService>();

                services.AddSingleton<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<ClotheslinesViewModel>();
                services.AddTransient<MainWindow>();
            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        using (var scope = _host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SnapnestDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host) { await _host.StopAsync(TimeSpan.FromSeconds(5)); }
        base.OnExit(e);
    }
}