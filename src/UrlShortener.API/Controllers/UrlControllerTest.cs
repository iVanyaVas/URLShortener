using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Queries;

using UrlShortener.Domain.Commands;

namespace UrlShorteneer.API.Controllers;

[Route("test/api/url")]
public class UrlControllerTest : BaseController
{
    private readonly IMediator _mediator;

    public UrlControllerTest(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("{Id}")]
    public Task<IActionResult> GetUrl([FromRoute] int Id, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
         {
             var query = new UrlQueryTest
             {
                 Id = Id
             };
             var result = await _mediator.Send(query, cancellationToken);
             var response = new GetUrlTestResponse
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
    public Task<IActionResult> CreateUrl([FromBody] CreateUrlTestRequest request, CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
        {
            var command = new CreateUrlCommandTest
            {
                OriginUrl = request.OriginUrl,
                ShortenedUrl = request.ShortenedUrl
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateUrlTestResponse
            {
                Id = result.UrlResult.Id
            };

            return Created("http://todo.com", response);
        }, cancellationToken);
    }
}