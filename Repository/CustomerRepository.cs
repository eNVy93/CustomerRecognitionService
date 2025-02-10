using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveCustomerAsync(Customer customer)
        {
            _ = await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetCustomersByIdListAsync(List<int> customerIds)
        {
            return await _dbContext.Customers.Where(c => customerIds.Contains(c.Id)).ToListAsync();
        }

        public async Task<List<Customer>> FindDuplicateCustomersAsync(Customer newCustomer)
        {
            return await _dbContext.Customers
                    .Where(c => !c.IsMerged && (
                        c.Email == newCustomer.Email ||
                        c.PhoneNumber == newCustomer.PhoneNumber ||
                        (c.FirstName == newCustomer.FirstName && c.LastName == newCustomer.LastName)))
                    .ToListAsync();
        }
    }
}
