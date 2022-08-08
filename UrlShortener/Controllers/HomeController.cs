using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Utility;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUrlRepository _urlRepository;

        public HomeController(ILogger<HomeController> logger, IUrlRepository urlRepository)
        {
            _logger = logger;
            _urlRepository = urlRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Start Index (GET) success");
            return View(_urlRepository.GetUrls().ToList());
        }


        [HttpPost]
        public IActionResult Index(string longUrl)
        {
            if (!string.IsNullOrEmpty(longUrl) || !string.IsNullOrWhiteSpace(longUrl))
            {
                if (ValidateUrl.IsValidUrl(longUrl))
                {

                    if (_urlRepository.GetByLongUrl(longUrl) == null)
                    {
                        var generator = new GenerateLink();
                        var urlModel = new UrlModel()
                        {
                            LongUrl = longUrl,
                            ShortUrl = $"https://loc.url/{generator.GenerateShortLink()}",
                            CreateUrl = DateTime.Now,
                            NumberOfClick = 0,
                        };

                        _urlRepository.InsertUrl(urlModel);
                        _urlRepository.SaveChanges();
                        ViewBag.URL = urlModel.ShortUrl;
                        ViewBag.MessageSuccess = "Success!";
                        _logger.LogInformation("Start Index (POST) success");
                        return View(_urlRepository.GetUrls());
                    }
                    else
                    {
                        ViewBag.MessageError = "This URL have in Database";
                        _logger.LogInformation("Index (POST): wrong url");
                        return View(_urlRepository.GetUrls());
                    }
                }
                else
                {
                    ViewBag.MessageError = "Not valid URL. Example : https://example.com";
                    _logger.LogInformation("Index (POST): wrong url");
                    return View(_urlRepository.GetUrls());
                }
            }
            ViewBag.MessageError = "Empty string";
            _logger.LogInformation("Index (POST): empty string");
            return View(_urlRepository.GetUrls());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"id-{id}, not found");
                return NotFound();
            }

            var url = _urlRepository.GetById(id);

            if (url == null)
            {
                _logger.LogInformation($"url-{url}, not found");
                return NotFound();
            }

            _logger.LogInformation("Edit (GET) start success");
            return View(url);
        }

        [HttpPost]
        public IActionResult Edit(UrlModel url)
        {
            var countLongUrl = _urlRepository.GetUrls().Where(x => x.LongUrl == url.LongUrl).Count();
            var countShortUrl = _urlRepository.GetUrls().Where(x => x.ShortUrl == url.ShortUrl).Count();
            var c = _urlRepository.GetUrls().First(x => x.Id == url.Id);

            if (countLongUrl == 0 || countShortUrl == 0 && ModelState.IsValid)
            {
                if (ValidateUrl.IsValidUrl(url.LongUrl) && url.ShortUrl.StartsWith("https://loc.url/"))
                {
                    _urlRepository.UpdateUrl(url);
                    _urlRepository.SaveChanges();
                    ViewBag.MessageSuccess = "Success!";
                    _logger.LogInformation("Edit (POST) success");
                    return RedirectToAction("Edit", "Home", new { url.Id });
                }
                else
                {
                    ViewBag.MessageError = @"Not valid URL. Example: https://example.com or https://loc.url/token";
                    _logger.LogInformation("Edit (POST): wrong url");
                    return View(url);
                }

            }
            else
            {
                ViewBag.MessageError = "Already in use!";
                _logger.LogInformation("Edit (POST): record was use");
                return View(url);
            }
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"id-{id}, not found");
                return NotFound();
            }
            var url = _urlRepository.GetById(id);

            if(url == null)
            {
                _logger.LogInformation($"url-{url}, not found");
                return NotFound();
            }
            try
            {
                
                _urlRepository.DeleteUrl(url);
                _urlRepository.SaveChanges();
                _logger.LogInformation("Delete success");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return RedirectToAction("Index", "Home");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult RedirectLongUrl(string longUrl)
        {
            if (longUrl.StartsWith("http"))
            {
                return Redirect(longUrl);
            }
            else if (longUrl.StartsWith("www."))
            {
                return Redirect("http://" + longUrl);
            }
            else
            {
                return Redirect("http://www." + longUrl);
            }
        }

        [HttpGet]
        public IActionResult RedirectShortUrl(string shortUrl)
        {
            var url = _urlRepository.GetByShortUrl(shortUrl);
            url.NumberOfClick += 1;
            _urlRepository.UpdateUrl(url);
            _urlRepository.SaveChanges();
            return RedirectLongUrl(url.LongUrl);
        }

    }
}