using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

using UrlShorteneer.Contracts.Http;
using UrlShorteneer.Domain.Exceptions;

namespace UrlShorteneer.API.Controllers;

public class BaseController : ControllerBase
{
    protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        
        catch(NotFoundException nf)
        {
               var response = new Error
            {
                Code = ErrorCode.NotFoundError,
                Message = nf.Message
            };

            return ToActionResult(response);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            var response = new Error
            {
                Code = ErrorCode.DbFailureError,
                Message = "Db Failure"
            };

            return ToActionResult(response);
        }
        catch (Exception)
        {
            var response = new Error
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandeled exception"
            };
            return ToActionResult(response);
        }
    }

    private IActionResult ToActionResult (Error errorResponse)
    {
        return StatusCode((int)errorResponse.Code/100,errorResponse);
    }
}