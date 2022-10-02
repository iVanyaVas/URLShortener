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
    public async Task HandlerShouldReturnThrowDbUpdateException()
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
        catch (BadRequestException br) when (br.ErrorCode == Contracts.Http.ErrorCode.BadRequestError)
        {
            //Assert
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
        catch (BadRequestException br) when (br.ErrorCode == Contracts.Http.ErrorCode.BadRequestError)
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