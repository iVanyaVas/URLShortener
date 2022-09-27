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


    [HttpGet("{Id}")]
    public Task<IActionResult> GetUrl([FromRoute] int Id, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
         {
             var query = new UrlQuery
             {
                 Id = Id
             };
             var result = await _mediator.Send(query, cancellationToken);
             var response = new GetUrlResponse
             {
                 UrlResponse = new Contracts.Database.Url
                 {
                     Id = result.UrlResult.Id,
                     OriginUrl = result.UrlResult.OriginUrl,
                     ShortenedUrl = result.UrlResult.ShortenedUrl
                 }
             };

             return Ok(response);
         }, cancellationToken);


    }

    [HttpPut]
    public Task<IActionResult> CreateUrl([FromBody] CreateUrlRequest request, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
        {
            var command = new CreateUrlCommand
            {
                OriginUrl = request.OriginUrl,
                ShortenedUrl = request.ShortenedUr
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateUrlResponse
            {
                Id = result.UrlResult.Id
            };

            return Created("http://todo.com", response);
        }, cancellationToken);
    }
}