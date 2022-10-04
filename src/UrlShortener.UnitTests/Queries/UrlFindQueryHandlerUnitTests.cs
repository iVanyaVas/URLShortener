using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;
using UrlShorteneer.Domain.Queries;
using UrlShorteneer.UnitTests.Helpers;

using Xunit;

namespace UrlShorteneer.UnitTests.Queries;

public class UrlFindQueryHandlerUnitTests : IDisposable
{
    private readonly UrlDbContext _dbContext;
    private readonly IRequestHandler<UrlFindQuery, UrlFindQueryResult> _handler;

    public UrlFindQueryHandlerUnitTests()
    {
        _dbContext = Helpers.DbContextHelper.CreateTestDb();
        _handler = new UrlFindQueryHandler(_dbContext, new Mock<ILogger<UrlFindQueryHandler>>().Object);
    }

    [Fact]
    public async Task UrlFindQueryShouldReturnOriginUrl()
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

        var query = new UrlFindQuery
        {
            ShortenedUrl = "https://localhost/ABC"
        };
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert

        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
        result.OriginUrl.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task UrlFindQueryShouldThrowException()
    {
        var dbContext = DbContextHelper.CreateTestDb(_dbContext.Database.GetDbConnection().ConnectionString);
        //Arrange
        var query = new UrlFindQuery
        {
            ShortenedUrl = "https://localhost/ABC"
        };
        //Act
        try
        {
            var result = await _handler.Handle(query, CancellationToken.None);
        }

        catch (NotFoundException nf)
        {
            //Assert
            nf.ErrorCode.ShouldBe(Contracts.Http.ErrorCode.NotFoundError);
        }
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}