using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;

using UrlShortener.Domain.Commands;

using Xunit;

namespace UrlShorteneer.UnitTests.Commands;


public class CreateUrlCommandHandlerUnitTests : IDisposable
{

    private readonly IRequestHandler<CreateUrlCommand, CreateUrlCommandResult> _handler;
    private readonly UrlDbContext _dbContext;

    public CreateUrlCommandHandlerUnitTests()
    {
        _dbContext = Helpers.DbContextHelper.CreateTestDb();
        _handler = new CreateUrlCommandHandler(_dbContext, new Mock<ILogger<CreateUrlCommandHandler>>().Object);

    }


    [Fact]
    public async Task HandlerShouldReturnGeneratedShortUrl()
    {
        //Arrange
        var originUrl = "https://vsetop.org/";
        var command = new CreateUrlCommand
        {
            OriginUrl = originUrl
        };

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        //Assert

        result.ShouldNotBeNull();
        result.ShortenedUrl.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task HandlerShouldReturnExistRecord()
    {
        //Arrange
        var url = new Url
        {
            OriginUrl = "https://www.google.com/",
            ShortenedUrl = "localhost:5246/S6876"
        };

        await _dbContext.AddAsync(url);
        await _dbContext.SaveChangesAsync();

        var command = new CreateUrlCommand
        {
            OriginUrl = url.OriginUrl
        };

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        //Assert

        result.ShouldNotBeNull();
        result.ShortenedUrl.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
        result.ShortenedUrl.ShouldBe("localhost:5246/S6876");
    }

    [Fact]
    public async Task EFShouldThrowDbUpdateException()
    {
        //Arrange
        var url = new Url
        {
            OriginUrl = "https://www.google.com/",
            ShortenedUrl = "localhost:5246/S6876"
        };

        //Act
        try
        {
            await _dbContext.AddAsync(url);
            await _dbContext.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            //Assert
        }
    }

    [Fact]
    public async Task EFShouldThrowDbUpdateExceptionForAddRange()
    {
        //Arrange
        var url1 = new Url
        {
            OriginUrl = "https://www.google.com/",
            ShortenedUrl = "localhost:5246/S6876"
        };
        var url2 = new Url
        {
            OriginUrl = "https://www.google.com/",
            ShortenedUrl = "localhost:5246/ABCDE"
        };
        var url3 = new Url
        {
            OriginUrl = "https://vsetop.org/",
            ShortenedUrl = "localhost:5246/SAC43"
        };

        //Act
        try
        {
            await _dbContext.AddRangeAsync(url1, url2, url3);
            await _dbContext.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            //Assert
        }
    }


    [Fact]
    public async Task HandlerShouldReturnExistRecordForAddRange()
    {
        //Arrange
        var url1 = new Url
        {
            OriginUrl = "https://www.google.com/",
            ShortenedUrl = "localhost:5246/S6876"
        };
        var url2 = new Url
        {
            OriginUrl = "https://www.clickminded.com/",
            ShortenedUrl = "localhost:5246/ABCDE"
        };
        var url3 = new Url
        {
            OriginUrl = "https://vsetop.org/",
            ShortenedUrl = "localhost:5246/SAC43"
        };

        var command = new CreateUrlCommand
        {
            OriginUrl = url1.OriginUrl
        };

        await _dbContext.AddRangeAsync(url1, url2, url3);
        await _dbContext.SaveChangesAsync();
        //Act

        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
        result.ShortenedUrl.ShouldNotBeNullOrEmpty();
        result.ShortenedUrl.ShouldBe("localhost:5246/S6876");

    }


    [Fact]
    public async Task HandlerShouldThrowException()
    {
        //Arrange
        var originUrl = Guid.NewGuid().ToString();
        var command = new CreateUrlCommand
        {
            OriginUrl = originUrl
        };

        //Act
        try
        {
            var result = await _handler.Handle(command, CancellationToken.None);
        }
        catch (BadRequestException br)
        {
            //Assert
            br.ErrorCode.ShouldBe(Contracts.Http.ErrorCode.BadRequestError);
        }

    }

    [Fact]
    public async Task HandlerShouldThrowExceptionForNullString()
    {
        //Arrange
        string originUrl = null;
        var command = new CreateUrlCommand
        {
            OriginUrl = originUrl
        };

        //Act
        try
        {
            var result = await _handler.Handle(command, CancellationToken.None);
        }
        catch (BadRequestException br)
        {
            //Assert
            br.ErrorCode.ShouldBe(Contracts.Http.ErrorCode.BadRequestError);
        }

    }
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}