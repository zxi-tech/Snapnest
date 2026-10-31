using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snapnest.Core.Entities;

namespace Snapnest.Application.Interfaces;

public interface IClotheslineService
{
    Task<IEnumerable<Clothesline>> GetAllAsync();
    Task<Clothesline> CreateAsync(string name);
    Task DeleteAsync(Guid id);
}