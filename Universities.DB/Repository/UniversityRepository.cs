using Microsoft.EntityFrameworkCore;
using Universities.DB.DbContexts;
using Universities.DB.Models;

namespace Universities.DB.Repository;

public sealed class UniversityRepository : IUniversityRepository
{
    private readonly UniversitiesDbContext _db;

    public UniversityRepository(UniversitiesDbContext db)
    {
        _db = db;
    }
}