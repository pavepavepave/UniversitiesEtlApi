using Microsoft.EntityFrameworkCore;
using Universities.DB.DbContexts;
using Universities.DB.Mapping;
using Universities.DB.Models;

namespace Universities.DB.Repository;

public sealed class UniversityRepository : IUniversityRepository
{
    private readonly UniversitiesDbContext _db;

    public UniversityRepository(UniversitiesDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UniversityDto>> GetByCountryAndNameAsync(string country, string universityName)
    {
        var universities = await _db.Universities.
            Where(u => u.Country == country && u.Name == universityName)
            .ToListAsync();
        return UniversityMapper.ToUniversitiesDto(universities);
    }

    public async Task AddRangeAsync(IEnumerable<UniversityDto> universities)
    {
        var universitiesEntities = UniversityMapper.ToUniversities(universities);
        await _db.Universities.AddRangeAsync(universitiesEntities);
        await _db.SaveChangesAsync();
    }
}