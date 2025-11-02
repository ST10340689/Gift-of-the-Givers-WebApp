using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class AccountControllerTests
    {
        private ApplicationDbContext GetContext()
        {
            // use a unique in-memory database name per call to avoid cross-test contamination
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        // Minimal in-memory ISession implementation for tests
        private class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _store = new();
            public IEnumerable<string> Keys => _store.Keys;
            public string Id { get; } = Guid.NewGuid().ToString();
            public bool IsAvailable => true;
            public void Clear() => _store.Clear();
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public void Remove(string key) => _store.Remove(key);
            public void Set(string key, byte[] value) => _store[key] = value;
            public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
        }

        [Fact]
        public void Login_InvalidUser_ReturnsView()
        {
            var context = GetContext();
            var controller = new AccountController(context);

            // Provide a default HttpContext so controller methods that touch HttpContext won't NRE.
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Session = new TestSession() }
            };

            var result = controller.Login("wrong@example.com", "password");

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Register_ValidUser_RedirectsToHomeIndex()
        {
            var context = GetContext();
            var controller = new AccountController(context);

            // Provide a session to avoid NullReference when controller sets session values
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Session = new TestSession() }
            };

            // Use the controller's Register signature: (string fullName, string email, string role, string password)
            var fullName = "Tester";
            var email = "test@example.com";
            var role = "User";
            var password = "123";

            var result = controller.Register(fullName, email, role, password);

            // Controller returns a Redirect (to Home/Index) as a RedirectResult with a URL string.
            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Contains("/Home/Index", redirect.Url);
        }
    }
}
