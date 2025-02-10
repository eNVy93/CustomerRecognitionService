using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> FindDuplicateCustomersAsync(Customer newCustomer);
        Task<List<Customer>> GetCustomersByIdListAsync(List<int> customerIds);
        Task SaveCustomerAsync(Customer customer);
    }
}