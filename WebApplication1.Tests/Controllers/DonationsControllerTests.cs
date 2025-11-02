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
    public class DonationsControllerTests
    {
        private ApplicationDbContext GetContext()
        {
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
        public async Task Index_ReturnsViewWithDonations()
        {
            var context = GetContext();
            context.Donations.Add(new Donation { Item = "Water", Quantity = 10, DateDonated = DateTime.Now });
            await context.SaveChangesAsync();

            var controller = new DonationsController(context);

            var result = controller.Index() as ViewResult;
            var model = Assert.IsAssignableFrom<IEnumerable<Donation>>(result!.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Create_ValidDonation_RedirectsToIndex()
        {
            var context = GetContext();
            var controller = new DonationsController(context);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Session = new TestSession() }
            };
            controller.ControllerContext.HttpContext.Session.SetInt32("UserId", 1);

            var result = controller.Create("Food", 250);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}
