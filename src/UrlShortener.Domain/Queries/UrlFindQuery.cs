using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;

namespace UrlShorteneer.Domain.Queries;


public class UrlFindQuery : IRequest<UrlFindQueryResult>
{
    public string ShortenedUrl { get; init; }
}

public class UrlFindQueryResult
{
    public int Id { get; init; }
    public string OriginUrl { get; init; }
}

internal class UrlFindQueryHandler : IRequestHandler<UrlFindQuery, UrlFindQueryResult>
{
    private readonly UrlDbContext _dbContext;
    private readonly ILogger<UrlFindQueryHandler> _logger;

    public UrlFindQueryHandler(UrlDbContext dbContext, ILogger<UrlFindQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<UrlFindQueryResult> Handle(UrlFindQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Excecution start : UrlFindQueryHandler, with Short Url = {ShortenedUrl}",request.ShortenedUrl);
        var url = await _dbContext.Urls.SingleOrDefaultAsync(u => u.ShortenedUrl == request.ShortenedUrl, cancellationToken);

        if(url == null)
        {
            _logger.LogInformation("Shortened Url: {ShortenedUrl} Not Found", request.ShortenedUrl );
            throw new NotFoundException($"Shortened Url: {request.ShortenedUrl} Not Found");
        }

        _logger.LogInformation("Shortened Url: {ShortenedUrl} Found", request.ShortenedUrl );
        return new UrlFindQueryResult
        {
            Id = url.Id,
            OriginUrl = url.OriginUrl
        };
    }
}