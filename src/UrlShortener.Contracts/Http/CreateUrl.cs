using System.ComponentModel.DataAnnotations;

namespace UrlShorteneer.Contracts.Http;

public class CreateUrlRequest
{
    [Required]
    [MaxLength(2048)]
    public string OriginUrl { get; init; }

}

public class CreateUrlResponse
{
    public int Id { get; init; }

    public string ShortenedUrl { get; init; }

}