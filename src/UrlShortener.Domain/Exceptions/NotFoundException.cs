using UrlShorteneer.Contracts.Http;
using System;

namespace UrlShorteneer.Domain.Exceptions;

public class NotFoundException : Exception
{
    public ErrorCode ErrorCode{get;}

    public NotFoundException(string message) : base(message)
    {
        ErrorCode = ErrorCode.NotFoundError;
    }
}