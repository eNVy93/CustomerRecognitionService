using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Services.Interfaces
{
    public interface ICustomerValidationService
    {
        Result<List<string>> ValidateCustomer(Customer customer);
    }
}