using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface IRSMRepository
{
    Task<List<RSMDto>> GetAll();
}

public class RSMRepository : IRSMRepository
{
    private MarinaDbContext _db;
    public RSMRepository(MarinaDbContext db)
    {
        _db = db;
    }

    public async Task<List<RSMDto>> GetAll()
    {
        return await _db.RSMs.Select(x => new RSMDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}
