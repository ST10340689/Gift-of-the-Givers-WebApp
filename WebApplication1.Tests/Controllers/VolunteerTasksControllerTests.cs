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
    public class VolunteerTasksControllerTests
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
        public void Index_ReturnsVolunteerTasks()
        {
            var context = GetContext();
            context.VolunteerTasks.Add(new VolunteerTask { TaskName = "Food Distribution", ScheduledDate = DateTime.Now });
            context.SaveChanges();

            var controller = new VolunteerTasksController(context);
            var result = controller.Index() as ViewResult;
            var model = Assert.IsAssignableFrom<IEnumerable<VolunteerTask>>(result!.Model);

            Assert.Single(model);
        }

        [Fact]
        public void Create_ValidTask_RedirectsToIndex()
        {
            var context = GetContext();
            var controller = new VolunteerTasksController(context);

            // Provide a session so controller won't redirect to Login
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Session = new TestSession() }
            };
            controller.ControllerContext.HttpContext.Session.SetInt32("UserId", 1);

            var result = controller.Create("Rescue Support", DateTime.Now);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}
