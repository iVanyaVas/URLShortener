using Microsoft.EntityFrameworkCore;
using UrlShorteneer.Contracts.Database;
namespace UrlShorteneer.Domain.Database;


public class UrlDbContext : DbContext
{

    public DbSet<Url> Urls { get; init; }

    public UrlDbContext(): base()
    {
    }

    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
    {        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=qwerty;Host=localhost;Port=5432;Database=urldb;");
    }

}