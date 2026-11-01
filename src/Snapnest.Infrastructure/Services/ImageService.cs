using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snapnest.Application.Interfaces;
using Snapnest.Core.Entities;
using Snapnest.Infrastructure.Data;

namespace Snapnest.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly SnapnestDbContext _dbContext;
    private readonly string _imagesDirectory;

    public ImageService(SnapnestDbContext dbContext)
    {
        _dbContext = dbContext;
        
        // Define internal storage path: %localappdata%\Snapnest\Images
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _imagesDirectory = Path.Combine(localAppData, "Snapnest", "Images");
        
        // Ensure the directory exists
        Directory.CreateDirectory(_imagesDirectory);
    }

    public async Task<IEnumerable<ImageItem>> GetAllImagesAsync()
    {
        return await _dbContext.ImageItems
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<ImageItem?> ImportImageAsync(string sourceFilePath)
    {
        if (!File.Exists(sourceFilePath)) return null;

        var fileInfo = new FileInfo(sourceFilePath);
        var extension = fileInfo.Extension.ToLowerInvariant();
        
        // Generate a unique filename for internal storage to prevent collisions
        var newFileName = $"{Guid.NewGuid()}{extension}";
        var destinationPath = Path.Combine(_imagesDirectory, newFileName);

        // Copy the file to Snapnest's internal storage
        File.Copy(sourceFilePath, destinationPath, overwrite: true);

        var imageItem = new ImageItem
        {
            FileName = fileInfo.Name,
            StoragePath = destinationPath,
            OriginalPath = sourceFilePath,
            FileSize = fileInfo.Length,
            SourceType = "Import",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.ImageItems.Add(imageItem);
        await _dbContext.SaveChangesAsync();

        return imageItem;
    }

    public async Task DeleteImageAsync(Guid id)
    {
        var image = await _dbContext.ImageItems.FindAsync(id);
        if (image != null)
        {
            // Delete the physical file from internal storage
            if (File.Exists(image.StoragePath))
            {
                File.Delete(image.StoragePath);
            }

            // Remove from database
            _dbContext.ImageItems.Remove(image);
            await _dbContext.SaveChangesAsync();
        }
    }
}