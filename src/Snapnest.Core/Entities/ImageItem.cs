using System;

namespace Snapnest.Core.Entities;

public class ImageItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string OriginalPath { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty; 
    public long FileSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}