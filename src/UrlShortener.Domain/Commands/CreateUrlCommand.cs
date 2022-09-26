using System.Threading;
using System.Threading.Tasks;

using MediatR;

using UrlShorteneer.Contracts.Database;
using UrlShorteneer.Domain.Database;

namespace UrlShortener.Domain.Commands;

public class CreateUrlCommand : IRequest<CreateUrlCommandResult>
{
    public string OriginUrl { get; init; }
    public string ShortenedUrl { get; init; }
}

public class CreateUrlCommandResult
{
    public Url Url { get; init; }
}

public class CreateUrlCommandHandler : IRequestHandler<CreateUrlCommand, CreateUrlCommandResult>
{

    private readonly UrlDbContext _dbContext;

    public CreateUrlCommandHandler(UrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateUrlCommandResult> Handle(CreateUrlCommand request, CancellationToken cancellationToken = default)
    {
        var url = new Url
        {
            OriginUrl = request.OriginUrl,
            ShortenedUrl = request.ShortenedUrl
        };

        await _dbContext.AddAsync(url, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUrlCommandResult
        {
            Url= url
        };
    }
}