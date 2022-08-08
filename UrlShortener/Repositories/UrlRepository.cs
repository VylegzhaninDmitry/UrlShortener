using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlContext _context;

        public UrlRepository(UrlContext context)
        {
            _context = context;
        }
        public void DeleteUrl(UrlModel url)
        {
            _context.Url.Remove(url);
        }

        public UrlModel GetById(int id)
        {
            return _context.Url.FirstOrDefault(x => x.Id == id);
        }

        public UrlModel GetByLongUrl(string longUrl)
        {
            return _context.Url.FirstOrDefault(x => x.LongUrl == longUrl);
        }

        public UrlModel GetByShortUrl(string shortUrl)
        {
            return _context.Url.FirstOrDefault(x => x.ShortUrl == shortUrl);
        }

        public IEnumerable<UrlModel> GetUrls()
        {
            return _context.Url.ToList();
        }

        public void InsertUrl(UrlModel url)
        {
            _context.Add(url);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateUrl(UrlModel url)
        {
            var oldUrl = _context.Url.First(e => e.Id == url.Id);
            _context.Entry(oldUrl).CurrentValues.SetValues(url);
            //_context.Entry(url).State = EntityState.Modified;
        }
    }
}
