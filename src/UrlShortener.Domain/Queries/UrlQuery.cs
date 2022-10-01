using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

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

    public UrlQueryHandler(UrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UrlQueryResult> Handle(UrlQuery request, CancellationToken cancellationToken)
    {
        var url = await _dbContext.Urls.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (url == null)
        {
            throw new NotFoundException($"Url with this {request.Id} ID Not Found");
        }

        return new UrlQueryResult
        {
            UrlResult = url
        };
    }
}