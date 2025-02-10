using CustomerRecognitionService.Entities;
using System.Text.RegularExpressions;

namespace CustomerRecognitionService.Helpers
{
    public static class CustomerHelper
    {
        public static Customer NormalizeCustomer(Customer customer)
        {
            return new Customer
            {
                FirstName = customer.FirstName.Trim(),
                LastName = customer.LastName.Trim(),
                Email = !string.IsNullOrEmpty(customer.Email) ? Regex.Replace(customer.Email, @"\s+", "") : customer.Email,
                PhoneNumber = !string.IsNullOrEmpty(customer.PhoneNumber) ? Regex.Replace(customer.PhoneNumber, @"\s+", "") : customer.PhoneNumber,
                Address = !string.IsNullOrEmpty(customer.Address) ? Regex.Replace(customer.Address, @"\s+", "") : customer.Address,
            };
        }

    }
}
