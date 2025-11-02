using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class UserControllerTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Index_ReturnsUserProfiles()
        {
            var context = GetContext();
            context.UserProfiles.Add(new UserProfile { FullName = "John", Email = "john@test.com", PasswordHash = "123" });
            await context.SaveChangesAsync();

            var controller = new UserController(context);
            var result = await controller.Index() as ViewResult;
            var model = Assert.IsAssignableFrom<IEnumerable<UserProfile>>(result!.Model);

            Assert.Single(model);
        }       

        [Fact]
        public async Task Create_ValidUser_RedirectsToIndex()
        {
            var context = GetContext();
            var controller = new UserController(context);
            var user = new UserProfile { FullName = "Jane", Email = "jane@test.com", PasswordHash = "123" };

            var result = await controller.Create(user);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}
