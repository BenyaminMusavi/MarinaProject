using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface IRegionRepository
{
    Task<List<RegionDto>> GetAll();
}
public class RegionRepository : IRegionRepository
{
    private MarinaDbContext _db;
    public RegionRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<RegionDto>> GetAll()
    {
        return await _db.Regions.Select(x => new RegionDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}
