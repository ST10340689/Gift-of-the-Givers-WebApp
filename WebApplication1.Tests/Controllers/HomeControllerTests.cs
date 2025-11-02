using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTests()
        {
            
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var controller = new HomeController(_logger);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            var controller = new HomeController(_logger);

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewWithErrorViewModel()
        {
            var controller = new HomeController(_logger);

            var result = controller.Error() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<WebApplication1.Models.ErrorViewModel>(result!.Model);
        }
    }
}
