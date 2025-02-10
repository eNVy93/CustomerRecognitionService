using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Entities.DTOs;

namespace CustomerRecognitionService.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> FindDuplicateCustomersAsync(Customer newCustomer);
        Task<Result<List<Customer>>> GetCustomersByIdListAsync(List<int> customerIds);
        Task<Result<string>> SaveCustomerAsync(Customer customer);
    }
}