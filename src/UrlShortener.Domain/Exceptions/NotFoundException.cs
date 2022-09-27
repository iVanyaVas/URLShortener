using UrlShorteneer.Contracts.Http;
using System;

namespace UrlShorteneer.Domain.Exceptions;

public class NotFoundException : Exception
{
    public ErrorCode ErrorCode{get;}

    public NotFoundException(ErrorCode errorCode) : this(errorCode, null)
    {
        ErrorCode = errorCode;
    }

    public NotFoundException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}