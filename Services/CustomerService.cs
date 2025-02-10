using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Helpers;
using CustomerRecognitionService.Repository.Interfaces;
using CustomerRecognitionService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CustomerRecognitionService.Services
{
    public class CustomerService : ICustomerService
    {
        private const int numberOfConcurrentDuplicateChecks = 5;
        private readonly IPendingMergeRepository _pendingMergeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerValidationService _customerValidationService;
        private static readonly SemaphoreSlim _duplicateChecksSemaphore = new(numberOfConcurrentDuplicateChecks);


        public CustomerService(
            IPendingMergeRepository pendingMergeRepository,
            ICustomerRepository customerRepository,
            ICustomerValidationService customerValidationService
        )
        {
            _pendingMergeRepository = pendingMergeRepository;
            _customerRepository = customerRepository;
            _customerValidationService = customerValidationService;
        }

        public async Task<Result<string>> SaveCustomerAsync(Customer customer)
        {
            try
            {
                customer = CustomerHelper.NormalizeCustomer(customer);
                var validationResult = _customerValidationService.ValidateCustomer(customer);

                if (!validationResult.Success)
                {
                    return Result<string>.Fail(validationResult.Message);
                }

                await _customerRepository.SaveCustomerAsync(customer);

                var duplicateCustomers = await FindDuplicateCustomersAsync(customer);

                if (duplicateCustomers.Count > 0)
                {
                    var pendingMerges = CreatePendingMerges(duplicateCustomers);

                    await _pendingMergeRepository.SavePendingMergesAsync(pendingMerges);
                }

                return Result<string>.Ok();
            }
            catch (OperationCanceledException)
            {
                return Result<string>.Fail("Operation canceled");
            }
            catch (Exception)
            {
                return Result<string>.Fail("Unexpected error occured");
            }
        }

        public async Task<Result<List<Customer>>> GetCustomersByIdListAsync(List<int> customerIds)
        {
            try
            {
                var data = await _customerRepository.GetCustomersByIdListAsync(customerIds);
                return Result<List<Customer>>.Ok(data);

            }
            catch (OperationCanceledException)
            {
                return Result<List<Customer>>.Fail("Operation canceled");
            }
            catch (Exception)
            {
                return Result<List<Customer>>.Fail("Unexpected error occured");
            }
        }

        public async Task<List<Customer>> FindDuplicateCustomersAsync(Customer newCustomer)
        {
            await _duplicateChecksSemaphore.WaitAsync();
            try
            {
                var duplicateCustomers = await _customerRepository.FindDuplicateCustomersAsync(newCustomer);
                return duplicateCustomers;
            }
            finally
            {
                _duplicateChecksSemaphore.Release();
            }
        }
        private List<PendingMerge> CreatePendingMerges(List<Customer> duplicateCustomers)
        {
            var masterCustomer = duplicateCustomers.OrderBy(c => c.CreatedAt).First();

            return duplicateCustomers
                .Where(c => c.Id != masterCustomer.Id)
                .Select(d => new PendingMerge
                {
                    Customer1Id = masterCustomer.Id,
                    Customer2Id = d.Id,
                    DetectedAt = DateTime.UtcNow,
                })
            .ToList();
        }
    }
}
