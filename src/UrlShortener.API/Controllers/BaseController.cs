using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Npgsql;

using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Exceptions;

namespace UrlShorteneer.API.Controllers;

public class BaseController : ControllerBase
{
    private readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }
    protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        
        catch(NotFoundException nf)
        {
             _logger.LogError(nf, "NotFoundException raised");
               var response = new Error
            {
                Code = ErrorCode.NotFoundError,
                Message = nf.Message
            };

            return ToActionResult(response);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
             _logger.LogError(ioe, "Db exception raised");
            var response = new Error
            {
                Code = ErrorCode.DbFailureError,
                Message = "Db Failure"
            };

            return ToActionResult(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandeled exception raised");
            var response = new Error
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandeled exception"
            };
            return ToActionResult(response);
        }
    }

    public IActionResult ToActionResult (Error errorResponse)
    {
        return StatusCode((int)errorResponse.Code/100,errorResponse);
    }
}