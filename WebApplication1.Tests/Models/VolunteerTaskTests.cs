using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class VolunteerTaskTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void DefaultStatus_IsOpen()
        {
            var model = new VolunteerTask();
            Assert.Equal("Open", model.Status);
        }

        [Fact]
        public void ValidVolunteerTask_PassesValidation()
        {
            var model = new VolunteerTask
            {
                TaskName = "Food Distribution",
                ScheduledDate = DateTime.UtcNow,
                UserProfileId = 1
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void MissingTaskName_FailsValidation()
        {
            var model = new VolunteerTask();
            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(VolunteerTask.TaskName)));
        }
    }
}
