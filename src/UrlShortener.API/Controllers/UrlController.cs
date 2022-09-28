using MediatR;

using Microsoft.AspNetCore.Components;

namespace UrlShorteneer.API.Controllers;


[Route("api/url")]
public class UrlController : BaseController
{
    private readonly IMediator _meddiator;

    public UrlController(IMediator mediator)
    {
        _meddiator = mediator;
    }


}