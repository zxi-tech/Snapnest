using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snapnest.Application.Interfaces;
using Snapnest.Core.Entities;
using Snapnest.Infrastructure.Data;

namespace Snapnest.Infrastructure.Services;

public class ClotheslineService : IClotheslineService
{
    private readonly SnapnestDbContext _dbContext;

    public ClotheslineService(SnapnestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Clothesline>> GetAllAsync()
    {
        return await _dbContext.Clotheslines
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Clothesline> CreateAsync(string name)
    {
        var clothesline = new Clothesline { Name = name };
        _dbContext.Clotheslines.Add(clothesline);
        await _dbContext.SaveChangesAsync();
        return clothesline;
    }

    public async Task DeleteAsync(Guid id)
    {
        var clothesline = await _dbContext.Clotheslines.FindAsync(id);
        if (clothesline != null)
        {
            _dbContext.Clotheslines.Remove(clothesline);
            await _dbContext.SaveChangesAsync();
        }
    }
}