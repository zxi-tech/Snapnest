using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Snapnest.Application.Interfaces;

namespace Snapnest.Infrastructure.Services;

public class ScreenshotService : IScreenshotService
{
    private readonly string _tempDirectory;
    private readonly string _normalSaveDirectory;

    public ScreenshotService()
    {
        // Define the temporary storage path for unconfirmed screenshots
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _tempDirectory = Path.Combine(localAppData, "Snapnest", "Temporary");
        Directory.CreateDirectory(_tempDirectory);

        // Define the default save destination in the user's Pictures folder
        string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        _normalSaveDirectory = Path.Combine(picturesFolder, "Snapnest");
        Directory.CreateDirectory(_normalSaveDirectory);
    }

    public async Task<string> CaptureFullScreenAsync()
    {
        // Offload the capture process to a background thread to keep the UI responsive
        return await Task.Run(() =>
        {
            // Capture the primary screen dimensions
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            using var bmp = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bmp);
            
            // Extract pixel data directly from the screen
            graphics.CopyFromScreen(0, 0, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            // Generate a unique filename using the current timestamp
            string tempFileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string tempFilePath = Path.Combine(_tempDirectory, tempFileName);

            bmp.Save(tempFilePath, ImageFormat.Png);
            
            return tempFilePath;
        });
    }

    public void DiscardTempImage(string tempFilePath)
    {
        if (File.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
        }
    }

    public string GetNormalSaveDirectory()
    {
        return _normalSaveDirectory;
    }
}