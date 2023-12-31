using Marina.UI.Models.Entities;
using Marina.UI.Models;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface INSMRepository
{
    Task<List<NsmDto>> GetAll();
}
public class NSMRepository : INSMRepository
{
    private MarinaDbContext _db;
    public NSMRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<NsmDto>> GetAll()
    {
        return await _db.NSMs.Select(x => new NsmDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}
