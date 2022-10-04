using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using UrlShorteneer.Contracts.Database;
namespace UrlShorteneer.Domain.Database;


public class UrlDbContext : DbContext
{

    public DbSet<Url> Urls { get; init; }

    public UrlDbContext() : base()
    {
    }

    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>().HasIndex(x => x.OriginUrl).IsUnique();
    }

    internal Task AddAsync()
    {
        throw new NotImplementedException();
    }
}