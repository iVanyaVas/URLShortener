using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Queries;

using UrlShortener.Domain.Commands;

namespace UrlShorteneer.API.Controllers;

[Route("api/url")]
public class UrlController : BaseController
{
    private readonly IMediator _mediator;

    public UrlController(IMediator mediator, ILogger<UrlController> logger) : base(logger)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns the redirect to the origin url by id acess
    /// </summary>
    /// <param name="id"> Url id</param>
    /// <param name="cancellationToken"> Cancellation Token</param>
    /// <returns>Origin URL</returns>
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetUrlById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
         {
             var query = new UrlQuery
             {
                 Id = id
             };
             var result = await _mediator.Send(query, cancellationToken);

             return Redirect(result.UrlResult.OriginUrl);
         }, cancellationToken);


    }

    /// <summary>
    /// Returns redirect to the origin url by short link access
    /// </summary>
    /// <param name="shortenedUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Origin URL</returns>
    [HttpGet("{shortenedUrl:minlength(4)}")]
    public Task<IActionResult> GetUrl([FromRoute] string shortenedUrl, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
         {
             var query = new UrlFindQuery
             {
                 ShortenedUrl = shortenedUrl
             };
             var result = await _mediator.Send(query, cancellationToken);

             return Redirect(result.OriginUrl);
         }, cancellationToken);

    }

    /// <summary>
    /// Input Origin Url and Returns Shortened url
    /// </summary>
    /// <param name="request"> Origin Url</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns> Short Url</returns>
    [HttpPut]
    public Task<IActionResult> CreateUrl([FromBody] CreateUrlRequest request, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
        {
            if(!ModelState.IsValid)
            {
                return ToActionResult( new Error
                {
                    Code = ErrorCode.BadRequestError,
                    Message = "invalid request"
                });
            }
            var command = new CreateUrlCommand
            {
                OriginUrl = request.OriginUrl,
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateUrlResponse
            {
                Id = result.Id,
                ShortenedUrl = result.ShortenedUrl
            };

            return Created($"http://localhost:5246/api/url/{result.ShortenedUrl}", response);
        }, cancellationToken);
    }
}