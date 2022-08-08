using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
    public class UrlContext:DbContext
    {
        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {

        }

        public DbSet<UrlModel> Url { get; set; }
    }
}
