using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class UserProfileTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void ValidUserProfile_PassesValidation()
        {
            var model = new UserProfile
            {
                FullName = "Jane Doe",
                Email = "jane.doe@example.com",
                PasswordHash = "hashed",
                Role = "User"
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void MissingRequiredFields_FailsValidation()
        {
            var model = new UserProfile();
            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfile.FullName)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfile.Email)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfile.PasswordHash)));
        }

        [Fact]
        public void InvalidEmail_FailsEmailValidation()
        {
            var model = new UserProfile
            {
                FullName = "Test",
                Email = "not-an-email",
                PasswordHash = "hashed"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfile.Email)));
        }
    }
}
