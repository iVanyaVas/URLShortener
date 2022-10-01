using System.IO;

using Microsoft.EntityFrameworkCore;

using UrlShorteneer.Domain.Database;

namespace UrlShorteneer.UnitTests.Helpers;

internal static class DbContextHelper
{
    public static UrlDbContext CreateTestDb()
    {
        var tempFile = Path.GetTempFileName();
        return CreateTestDb($"Data Source={tempFile}");
    }

    public static UrlDbContext CreateTestDb(string connectionString)
    {
        var options = new DbContextOptionsBuilder<UrlDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var dbContext = new UrlDbContext(options);
        dbContext.Database.Migrate();

        return dbContext;
    }
}