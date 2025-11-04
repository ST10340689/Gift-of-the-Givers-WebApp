using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WebApplication1.Controllers;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTests()
        {
            // Use a no-op logger for tests to avoid depending on console providers
            _logger = NullLogger<HomeController>.Instance;
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

            // Provide a HttpContext so HttpContext.TraceIdentifier access inside Error() won't NRE
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Error() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<WebApplication1.Models.ErrorViewModel>(result!.Model);
        }
    }
}
