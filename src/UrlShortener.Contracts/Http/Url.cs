using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShorteneer.Contracts.Http;

public class UrlResponse
{

    public int Id { get; init; }
    [Required]
    [MaxLength(2048)]
    public string OriginUrl { get; init; }

    [Required]
    [MaxLength(1024)]
    public string ShortenedUrl { get; init; }
}