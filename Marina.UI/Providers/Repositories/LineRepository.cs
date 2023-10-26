using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Providers.Repositories;

public interface ILineRepository
{
    Task<List<LineDto>> GetAll();

}

public class LineRepository: ILineRepository
{
    private MarinaDbContext _db;
    public LineRepository(MarinaDbContext db)
    {
        _db = db;
    }
    public async Task<List<LineDto>> GetAll()
    {
        return await _db.Lines.Select(x => new LineDto
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();
    }
}
