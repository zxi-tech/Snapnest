using System;

namespace Snapnest.Core.Entities;

public class ClotheslineImage
{
    public Guid ClotheslineId { get; set; }
    public Clothesline Clothesline { get; set; } = null!;
    public Guid ImageId { get; set; }
    public ImageItem ImageItem { get; set; } = null!;
    public int SortOrder { get; set; }
}