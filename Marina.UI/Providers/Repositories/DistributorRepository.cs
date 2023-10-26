using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface IDistributorRepository
{
    Task<List<DistributorDto>> GetAll();

}

public class DistributorRepository: IDistributorRepository
{
    private MarinaDbContext _db;
    public DistributorRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<DistributorDto>> GetAll()
    {
        return await _db.Distributors.Select(x => new DistributorDto
        {
            Id = x.Id,
            Name = x.Code,
        }).ToListAsync();
    }
}
