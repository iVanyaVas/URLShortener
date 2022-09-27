using System.ComponentModel.DataAnnotations;

namespace UrlShorteneer.Contracts.Http;

public class CreateUrlRequest
{
    [Required]
    [MaxLength(2048)]
    public string OriginUrl { get; init; }
    [Required]
    [MaxLength(1024)]
    public string ShortenedUr { get; init; }
}

public class CreateUrlResponse
{
    public int Id { get; init; }

}