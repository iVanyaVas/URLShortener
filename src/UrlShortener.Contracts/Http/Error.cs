namespace UrlShorteneer.Contracts.Http;

public enum ErrorCode
{
    BadRequestError = 40001,
    NotFoundError = 40401,
    InternalServerError = 50000,
    DbFailureError = 50001
}
public class Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
}