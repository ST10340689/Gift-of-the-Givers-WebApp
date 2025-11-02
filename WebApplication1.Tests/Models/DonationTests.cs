using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class DonationTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void ValidDonation_PassesValidation()
        {
            var model = new Donation
            {
                Item = "Bottled Water",
                Quantity = 10,
                DateDonated = DateTime.UtcNow,
                UserProfileId = 1
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void MissingItem_FailsValidation()
        {
            var model = new Donation
            {
                Quantity = 5
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Donation.Item)));
        }
    }
}
