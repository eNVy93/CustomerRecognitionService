using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Services
{
    public interface ICustomerValidationService
    {
        Result<List<string>> ValidateCustomer(Customer customer);
    }
}