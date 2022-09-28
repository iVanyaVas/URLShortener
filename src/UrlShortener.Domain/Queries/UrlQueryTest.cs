using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Database;
using UrlShorteneer.Domain.Exceptions;

namespace UrlShorteneer.Domain.Queries;

public class UrlQueryTest : IRequest<UrlQueryTestResult>
{
    public int Id { get; init; }
}

public class UrlQueryTestResult
{
    public Url UrlResult { get; init; }
}

public class UrlQueryTestHandler : IRequestHandler<UrlQueryTest, UrlQueryTestResult>
{
    private readonly UrlDbContext _dbContext;

    public UrlQueryTestHandler(UrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UrlQueryTestResult> Handle(UrlQueryTest request, CancellationToken cancellationToken)
    {
        var url = await _dbContext.Urls.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (url == null)
        {
            throw new NotFoundException(ErrorCode.NotFoundError, $"Url with this {request.Id} ID Not Found");
        }

        return new UrlQueryTestResult
        {
            UrlResult = url
        };
    }
}