using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface IProvinceRepository
{
    Task<List<ProvinceDto>> GetAll();

}

public class ProvinceRepository : IProvinceRepository
{
    private MarinaDbContext _db;
    public ProvinceRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<ProvinceDto>> GetAll()
    {
        return await _db.Provinces.Select(x => new ProvinceDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}

