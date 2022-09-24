using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShorteneer.Contracts.Database;

[Table("tbl_urls", Schema = "public")]
public class Url
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [MaxLength(2048)]
    [Column("origin_url")]
    public string OriginUrl { get; set; }

    [Required]
    [MaxLength(1024)]
    [Column("shortened_url")]
    public string ShortenedUrl { get; set; }
}