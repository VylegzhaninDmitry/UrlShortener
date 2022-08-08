using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlModel
    {
        [Key]
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreateUrl { get; set; }
        public int NumberOfClick { get; set; }
    }
}
