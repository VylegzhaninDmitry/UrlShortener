using Microsoft.Extensions.Logging;
using UrlShortener.Controllers;
using UrlShortener.Repositories;

namespace UnitTests
{
    public class Tests
    {
        private HomeController _homeController;
        private IUrlRepository _urlRepository;
        private ILogger<HomeController> _logger;
        [SetUp]
        public void Setup()
        {
            _homeController = new HomeController(_logger,_urlRepository);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}