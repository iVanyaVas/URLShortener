using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using UrlShorteneer.Domain.Database;
using UrlShortener.Domain.Commands;
using UrlShorteneer.Contracts;

using Xunit;
using System.IO;

public class CreateUrlCommandHandlerTest : IDisposable
{
    private readonly string tempFile;
    private readonly IRequestHandler<CreateUrlCommand, CreateUrlCommandResult> _handler;
    private readonly UrlDbContext _dbContext;
    public CreateUrlCommandHandlerTest()
    {
        tempFile = Path.GetTempFileName();
        var options = new DbContextOptionsBuilder<UrlDbContext>().UseSqlite($"Data Source={tempFile};").Options;

        _dbContext = new UrlDbContext(options);
        _dbContext.Database.Migrate();
        _handler = new CreateUrlCommandHandler(_dbContext);
    }

    [Fact]
    public void TempFileShouldExist()
    {
        //Arrange
        //Act
        var result = File.Exists(tempFile);
        //Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HandleShouldCreateUrlAndCheckTheProperties()
    {

        // Arrange
        var originUrl = Guid.NewGuid().ToString();
        var shortenedUrl = Guid.NewGuid().ToString();

        var command = new CreateUrlCommand
        {
            OriginUrl = originUrl,
            ShortenedUrl = shortenedUrl
        };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Url.ShouldNotBeNull();
        result.Url.OriginUrl.ShouldNotBeNullOrWhiteSpace();
        result.Url.ShortenedUrl.ShouldNotBeNullOrWhiteSpace();
        result.Url.OriginUrl.ShouldBe(originUrl);
        result.Url.ShortenedUrl.ShouldBe(shortenedUrl);
        result.Url.Id.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}