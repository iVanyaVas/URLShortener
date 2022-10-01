using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Queries;

using UrlShortener.Domain.Commands;

namespace UrlShorteneer.API.Controllers;

[Route("api/url")]
public class UrlController : BaseController
{
    private readonly IMediator _mediator;

    public UrlController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns the redirect to the origin site by id
    /// </summary>
    /// <param name="id"> Url id</param>
    /// <param name="cancellationToken"> Cancellation Token</param>
    /// <returns>Redirect to Origin Url</returns>
    /// <response code = "302"> Redirect to origin url</response>
    /// <response code = "404">Url Not Found</response>
    /// <response code = "500">Internal Server Error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(string), 302)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public Task<IActionResult> GetUrl([FromRoute] int id, CancellationToken cancellationToken)
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
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<IActionResult> CreateUrl([FromBody] CreateUrlRequest request, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
        {
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

            return Created($"http://localhost:5246/api/url{result.Id}", response);
        }, cancellationToken);
    }
}