using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationProject.Controllers
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(250)
                .WithMessage("Invalid Name");

            RuleFor(e => e.DateOfBirth)
                .NotEmpty()
                .Must(BeValidBirthDate)
                .WithMessage("Birthdate is not valid.");

            RuleFor(e => e.Email)
                .EmailAddress()
                .WithMessage("Email address is not valid.");

            RuleFor(e => e.Phone)
                .Must(BeAValidPhoneNumber)
                .WithMessage("Phone is not valid.");

            RuleFor(e => e.HourlySalary)
                .InclusiveBetween(50, 400)
                .Must((e, hourlySalary) => BeValidSalary(e, hourlySalary))
                .WithMessage("Hourly salary does not fall within allowed range.");
        }

        /// <summary>
        /// Determines if the specified date of birth is valid.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to validate.</param>
        /// <returns>
        ///   <c>true</c> if the specified date of birth is valid; otherwise, <c>false</c>.
        /// </returns>
        private bool BeValidBirthDate(DateTime dateOfBirth)
        {
            var minAllowedBirthDate = DateTime.Today.AddYears(-65);
            return minAllowedBirthDate <= dateOfBirth;
        }

        /// <summary>
        /// Determines if the specified phone number is a valid phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>
        ///   <c>true</c> if the specified phone number is valid; otherwise, <c>false</c>.
        /// </returns>
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            string trimmedPhoneNumber = phoneNumber?.Trim();

            if (string.IsNullOrEmpty(trimmedPhoneNumber))
            {
                return false;
            }

            return trimmedPhoneNumber.Length == 10 && trimmedPhoneNumber.All(char.IsDigit) && !trimmedPhoneNumber.StartsWith("0");
        }

        /// <summary>
        /// Determines if the hourly salary is valid based on the employee's age and minimum salary requirements.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        /// <param name="hourlySalary">The hourly salary.</param>
        /// <returns>True if the hourly salary is valid, otherwise false.</returns>
        private bool BeValidSalary(Employee employee, double hourlySalary)
        {
            var dateBeforeThirtyYears = DateTime.Today.AddYears(-30);
            var isOlderThanThirtyYears = employee.DateOfBirth <= dateBeforeThirtyYears;

            if (isOlderThanThirtyYears)
            {
                return hourlySalary >= employee.MinSeniorSalary;
            }
            else
            {
                return hourlySalary >= employee.MinJuniorSalary;
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController()
        {
        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee value)
        {
            var validator = new EmployeeValidator();
            var validationResult = validator.Validate(value);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (value.DateOfBirth > DateTime.Now.AddYears(-30) && value.HourlySalary < 200)
            {
                
            }

            return Ok(value);
        }
    }
}