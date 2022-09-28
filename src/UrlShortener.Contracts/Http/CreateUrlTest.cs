using System.ComponentModel.DataAnnotations;

namespace UrlShorteneer.Contracts.Http;

public class CreateUrlTestRequest
{
    [Required]
    [MaxLength(2048)]
    public string OriginUrl { get; init; }
    [Required]
    [MaxLength(1024)]
    public string ShortenedUrl { get; init; }
}

public class CreateUrlTestResponse
{
    public int Id { get; init; }

}