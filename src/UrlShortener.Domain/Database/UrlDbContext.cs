using System;

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

}