using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Repository
{
    public class CustomerMergeRepository : ICustomerMergeRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerMergeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveProcessedMerges(List<PendingMerge> pendingMerges, List<MergedCustomerHistory> mergedHistories)
        {
            _dbContext.PendingMerges.UpdateRange(pendingMerges);
            await _dbContext.MergedCustomersHistory.AddRangeAsync(mergedHistories);
            await _dbContext.SaveChangesAsync();
        }
    }
}
