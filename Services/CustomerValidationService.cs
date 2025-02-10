using CustomerRecognitionService.Entities;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CustomerRecognitionService.Services
{
    public class CustomerValidationService : ICustomerValidationService
    {
        public Result<List<string>> ValidateCustomer(Customer customer)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.LastName))
                errors.Add("First name and last name are required.");

            ValidateField(ValidateFirstName(customer.FirstName), errors);
            ValidateField(ValidateLastName(customer.LastName), errors);
            ValidateField(ValidateEmail(customer.Email), errors);

            if (!string.IsNullOrEmpty(customer.PhoneNumber))
                ValidateField(ValidatePhoneNumber(customer.PhoneNumber), errors);

            return errors.Count > 0 ? Result<List<string>>.Fail(string.Join(" ", errors), errors) : Result<List<string>>.Ok("Customer is valid");
        }

        private Result<string> ValidateFirstName(string firstName)
        {
            return firstName.Length > 24 ? Result<string>.Fail("First name cannot be longer thant 24 symbols") : Result<string>.Ok("First name is valid");
        }

        private Result<string> ValidateLastName(string lastName)
        {
            return lastName.Length > 36 ? Result<string>.Fail("First name cannot be longer thant 36 symbols") : Result<string>.Ok("First name is valid");
        }

        private Result<string> ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<string>.Fail("Email cannot be empty");

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return Result<string>.Fail("Email validation timed out");
            }
            catch (ArgumentException)
            {
                return Result<string>.Fail("Email validation argument is not valid");
            }

            try
            {
                bool isValid = Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                return isValid ? Result<string>.Ok("Email is valid") : Result<string>.Fail("Email is not valid");
            }
            catch (RegexMatchTimeoutException)
            {
                return Result<string>.Fail("Email validation timed out");
            }
        }
        private Result<string> ValidatePhoneNumber(string phoneNumber)
        {
            try
            {
                bool isValid = Regex.IsMatch(phoneNumber,
                    @"^([+]|[00]{2}|[0]{1})([0-9]|[ -])*",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                return isValid ? Result<string>.Ok("Phone number is valid") : Result<string>.Fail("Invalid phone number format.");
            }
            catch (RegexMatchTimeoutException)
            {
                return Result<string>.Fail("Phone number validation timed out.");
            }
        }

        private void ValidateField(Result<string> validationResult, List<string> errors)
        {
            if (!validationResult.Success)
                errors.Add(validationResult.Message);
        }
    }
}
