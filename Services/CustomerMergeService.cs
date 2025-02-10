using CustomerRecognitionService.Controllers;
using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using CustomerRecognitionService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Services
{
    public class CustomerMergeService : ICustomerMergeService
    {
        private readonly IPendingMergeService _pendingMergeService;
        private readonly ICustomerMergeRepository _customerMergeRepository;
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerMergeService> _logger;

        public CustomerMergeService(
            IPendingMergeService pendingMergeService,
            ICustomerMergeRepository customerMergeRepository,
            ICustomerService customerService,
            ILogger<CustomerMergeService> logger
            )
        {
            _pendingMergeService = pendingMergeService;
            _customerMergeRepository = customerMergeRepository;
            _customerService = customerService;
            _logger = logger;
        }

        public async Task ProcessMergesAsync()
        {
            _logger.LogInformation($"{nameof(ProcessMergesAsync)}. Processing pending merges.");
            var pendingMergesResult = await _pendingMergeService.GetPendingMergeListAsync();

            if (!pendingMergesResult.Success)
            {
                _logger.LogError($"{nameof(ProcessMergesAsync)}. Failed to retrieve pending merges. Error: {pendingMergesResult.Message}");
                throw new (pendingMergesResult.Message);
            }

            var pendingMerges = pendingMergesResult.Data;

            if (pendingMerges == null || pendingMerges.Count == 0)
            {
                _logger.LogInformation($"{nameof(ProcessMergesAsync)}. No pending merges found.");
                return;
            }

            var customerIds = GetCustomerIdsFromPendingMerges(pendingMerges);
            var customersForMergeResult = await _customerService.GetCustomersByIdListAsync(customerIds);

            if (!customersForMergeResult.Success)
            {
                _logger.LogError($"{nameof(ProcessMergesAsync)}. Failed to retrieve customer data for merge. Error: {customersForMergeResult.Message}");
                throw new (customersForMergeResult.Message);
            }

            var customersForMerge = customersForMergeResult?.Data;

            if (customersForMerge == null || customersForMerge.Count == 0)
            {
                _logger.LogInformation($"{nameof(ProcessMergesAsync)}. No customers to merge.");
                return;
            }

            var mergedHistories = ProcessPendingMerges(pendingMerges, customersForMergeResult.Data);
            await _customerMergeRepository.SaveProcessedMerges(pendingMerges, mergedHistories);
        }

        private static List<int> GetCustomerIdsFromPendingMerges(IEnumerable<PendingMerge> pendingMerges)
        {
            return pendingMerges
                .SelectMany(pm => new[] { pm.Customer1Id, pm.Customer2Id })
                .Distinct()
                .ToList();
        }

        private List<MergedCustomerHistory> ProcessPendingMerges(IEnumerable<PendingMerge> pendingMerges, List<Customer> customers)
        {
            var mergedHistories = new List<MergedCustomerHistory>();
            var mergedCustomerIds = new HashSet<int>();

            foreach (var pendingMerge in pendingMerges)
            {
                if (mergedCustomerIds.Contains(pendingMerge.Customer1Id) || mergedCustomerIds.Contains(pendingMerge.Customer2Id))
                    continue;

                var customer1 = customers.FirstOrDefault(c => c.Id == pendingMerge.Customer1Id);
                var customer2 = customers.FirstOrDefault(c => c.Id == pendingMerge.Customer2Id);

                if (customer1 == null || customer2 == null || customer1.IsMerged || customer2.IsMerged)
                    continue;

                MergeCustomerData(customer1, customer2);
                mergedCustomerIds.Add(customer2.Id);

                mergedHistories.Add(new MergedCustomerHistory
                {
                    OriginalCustomerId = customer1.Id,
                    MergedCustomerId = customer2.Id,
                    MergeDate = DateTime.UtcNow,
                });

                pendingMerge.IsProcessed = true;
            }

            return mergedHistories;
        }

        private static void MergeCustomerData(Customer primary, Customer duplicate)
        {
            primary.FirstName = !string.IsNullOrEmpty(duplicate.FirstName) ? duplicate.FirstName : primary.FirstName;
            primary.LastName = !string.IsNullOrEmpty(duplicate.LastName) ? duplicate.LastName : primary.LastName;
            primary.Email = !string.IsNullOrEmpty(duplicate.Email) ? duplicate.Email : primary.Email;
            primary.PhoneNumber = !string.IsNullOrEmpty(duplicate.PhoneNumber) ? duplicate.PhoneNumber : primary.PhoneNumber;
            primary.Address = !string.IsNullOrEmpty(duplicate.Address) ? duplicate.Address : primary.Address;
            primary.UpdatedAt = DateTime.UtcNow;

            duplicate.IsMerged = true;
        }
    }
}
