using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snapnest.Core.Entities;

namespace Snapnest.Application.Interfaces;

public interface IImageService
{
    Task<IEnumerable<ImageItem>> GetAllImagesAsync();
    Task<ImageItem?> ImportImageAsync(string sourceFilePath);
    Task DeleteImageAsync(Guid id);
}