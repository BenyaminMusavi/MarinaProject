using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface ISupervisorRepository
{
    Task<List<SupervisorDto>> GetAll();

}

public class SupervisorRepository : ISupervisorRepository
{
    private MarinaDbContext _db;
    public SupervisorRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<SupervisorDto>> GetAll()
    {
        return await _db.Supervisors.Select(x => new SupervisorDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}
