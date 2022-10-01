using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using MediatR;

using Shouldly;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;
using UrlShorteneer.Domain.Queries;
using UrlShorteneer.UnitTests.Helpers;

using Xunit;

namespace UrlShorteneer.UnitTests.Queries;

public class UrlQueryHandlerUnitTests : IDisposable
{
    private readonly UrlDbContext _dbContext;
    private readonly IRequestHandler<UrlQuery, UrlQueryResult> _handler;
    public UrlQueryHandlerUnitTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new UrlQueryHandler(_dbContext);
    }

    [Fact]
    public async Task QueryHandleShouldReturnUrl()
    {
        var dbContext = DbContextHelper.CreateTestDb(_dbContext.Database.GetDbConnection().ConnectionString);
        //Arrange
        var url = new Url
        {
            OriginUrl = "https://github.com/",
            ShortenedUrl = "https://localhost/ABC"
        };

        await dbContext.AddAsync(url);
        await dbContext.SaveChangesAsync();

        var query = new UrlQuery
        {
            Id = url.Id
        };
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert

        result.ShouldNotBeNull();
        result.UrlResult.ShouldNotBeNull();
        result.UrlResult.Id.ShouldBe(url.Id);
        result.UrlResult.OriginUrl.ShouldBe(url.OriginUrl);
        result.UrlResult.ShortenedUrl.ShouldBe(url.ShortenedUrl);

    }

    [Fact]
    public async Task QueryShouldThrowException()
    {
        //Arrange
        var urlId = -1;
        var query = new UrlQuery
        {
            Id = urlId
        };
        //Act
        try
        {
            await _handler.Handle(query, CancellationToken.None);
        }
        catch (NotFoundException e) when (e.ErrorCode == Contracts.Http.ErrorCode.NotFoundError)
        {
            //Assert
        }
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}