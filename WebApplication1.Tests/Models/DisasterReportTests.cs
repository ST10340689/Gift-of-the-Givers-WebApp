using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class DisasterReportTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void ValidDisasterReport_PassesValidation()
        {
            var model = new DisasterReport
            {
                DisasterType = "Flood",
                Location = "Cape Town",
                Description = "Heavy rain and flooding",
                ReportDate = DateTime.UtcNow,
                UserProfileId = 1
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void MissingRequiredFields_FailsValidation()
        {
            var model = new DisasterReport(); // missing DisasterType and Location
            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(DisasterReport.DisasterType)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(DisasterReport.Location)));
        }
    }
}
