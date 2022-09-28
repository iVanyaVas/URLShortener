using System.Threading;
using System.Threading.Tasks;

using MediatR;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Domain.Database;

namespace UrlShortener.Domain.Commands;

public class CreateUrlCommandTest : IRequest<CreateUrlCommandTestResult>
{
    public string OriginUrl { get; init; }
    public string ShortenedUrl { get; init; }
}

public class CreateUrlCommandTestResult
{
    public Url UrlResult { get; init; }
}

public class CreateUrlCommandTestHandler : IRequestHandler<CreateUrlCommandTest, CreateUrlCommandTestResult>
{

    private readonly UrlDbContext _dbContext;

    public CreateUrlCommandTestHandler(UrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateUrlCommandTestResult> Handle(CreateUrlCommandTest request, CancellationToken cancellationToken = default)
    {
        var url = new Url
        {
            OriginUrl = request.OriginUrl,
            ShortenedUrl = request.ShortenedUrl
        };

        await _dbContext.AddAsync(url, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUrlCommandTestResult
        {
            UrlResult = url
        };
    }
}