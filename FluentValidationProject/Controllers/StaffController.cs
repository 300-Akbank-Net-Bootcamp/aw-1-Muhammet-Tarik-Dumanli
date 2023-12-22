using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationProject.Controllers
{
    public class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(250);

            RuleFor(s => s.Email)
                .EmailAddress()
                .When(s => !string.IsNullOrEmpty(s.Email)); // Email boş olabilir, dolu ise doğrulanır.

            RuleFor(s => s.Phone)
                .Must(BeAValidPhoneNumber)
                .When(s => !string.IsNullOrEmpty(s.Phone))
                .WithMessage("Phone is not valid.");

            RuleFor(s => s.HourlySalary)
                .NotNull()
                .InclusiveBetween(30, 400)
                .When(s => s.HourlySalary != null)
                .WithMessage("Hourly salary does not fall within allowed range.");
        }

        /// <summary>
        /// Checks if a phone number is valid.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <returns><c>true</c> if the phone number is valid; otherwise, <c>false</c>.</returns>
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            string trimmedPhoneNumber = phoneNumber?.Trim();

            if (string.IsNullOrEmpty(trimmedPhoneNumber))
                return false;

            return trimmedPhoneNumber.Length == 10 && trimmedPhoneNumber.All(char.IsDigit) && !trimmedPhoneNumber.StartsWith("0");
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        public StaffController()
        {
        }

        [HttpPost]
        public IActionResult Post([FromBody] Staff value)
        {
            var validator = new StaffValidator();
            var validationResult = validator.Validate(value);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            return Ok(value);
        }
    }
}