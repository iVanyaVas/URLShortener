using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;

namespace UrlShorteneer.Domain.Queries;

public class UrlQuery : IRequest<UrlQueryResult>
{
    public int Id { get; init; }
}

public class UrlQueryResult
{
    public Url UrlResult { get; init; }
}

public class UrlQueryHandler : IRequestHandler<UrlQuery, UrlQueryResult>
{
    private readonly UrlDbContext _dbContext;
    private readonly ILogger<UrlQueryHandler> _logger;

    internal UrlQueryHandler(UrlDbContext dbContext, ILogger<UrlQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<UrlQueryResult> Handle(UrlQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Excecution start : UrlQueryHandler, with Id = {Id}",request.Id);
        var url = await _dbContext.Urls.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (url == null)
        {
            _logger.LogInformation("Url Id {Id} Not Found", request.Id);
            throw new NotFoundException($"Url with this {request.Id} ID Not Found");
        }

        _logger.LogInformation("Url with Id {Id} Found",request.Id);
        return new UrlQueryResult
        {
            UrlResult = url
        };
    }
}