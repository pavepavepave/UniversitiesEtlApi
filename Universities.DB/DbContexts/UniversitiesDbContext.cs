using Microsoft.EntityFrameworkCore;
using Universities.DB.Models;

namespace Universities.DB.DbContexts;

public sealed class UniversitiesDbContext : DbContext
{
    public UniversitiesDbContext(DbContextOptions<UniversitiesDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<University> Universities { get; set; }
}