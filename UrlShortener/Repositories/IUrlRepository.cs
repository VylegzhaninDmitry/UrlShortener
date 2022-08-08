using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUrlRepository
    {
        IEnumerable<UrlModel> GetUrls();
        UrlModel GetByLongUrl(string longUrl);
        UrlModel GetById(int id);
        UrlModel GetByShortUrl(string shortUrl);
        void UpdateUrl(UrlModel url);
        void DeleteUrl(UrlModel url);
        void InsertUrl(UrlModel url);
        void SaveChanges();
    }
}
