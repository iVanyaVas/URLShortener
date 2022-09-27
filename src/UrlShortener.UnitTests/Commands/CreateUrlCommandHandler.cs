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
using UrlShorteneer.UnitTests.Helpers;

public class CreateUrlCommandHandlerTest : IDisposable
{
    private readonly string _tempFile;
    private readonly IRequestHandler<CreateUrlCommand, CreateUrlCommandResult> _handler;
    private readonly UrlDbContext _dbContext;
    public CreateUrlCommandHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb(ref _tempFile);
        _handler = new CreateUrlCommandHandler(_dbContext);
    }

    [Fact]
    public void TempFileShouldExist()
    {
        //Arrange
        //Act
        var result = File.Exists(_tempFile);
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
        result.UrlResult.ShouldNotBeNull();
        result.UrlResult.OriginUrl.ShouldNotBeNullOrWhiteSpace();
        result.UrlResult.ShortenedUrl.ShouldNotBeNullOrWhiteSpace();
        result.UrlResult.OriginUrl.ShouldBe(originUrl);
        result.UrlResult.ShortenedUrl.ShouldBe(shortenedUrl);
        result.UrlResult.Id.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}