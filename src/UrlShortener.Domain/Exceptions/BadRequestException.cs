using UrlShorteneer.Contracts.Http;
using System;

namespace UrlShorteneer.Domain.Exceptions;

public class BadRequestException : Exception
{
    public ErrorCode ErrorCode{get;}

    public BadRequestException(string message) : base(message)
    {
        ErrorCode = ErrorCode.BadRequestError;
    }
}