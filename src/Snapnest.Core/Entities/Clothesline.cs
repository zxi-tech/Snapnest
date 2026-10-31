using System;
using System.Collections.Generic;

namespace Snapnest.Core.Entities;

public class Clothesline
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ClotheslineImage> ClotheslineImages { get; set; } = new List<ClotheslineImage>();
}