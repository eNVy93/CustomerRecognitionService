using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Repository
{
    public class MergedCustomerHistoryRepository : IMergedCustomerHistoryRepository
    {
        private readonly AppDbContext _dbContext;

        public MergedCustomerHistoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveMergedCustomerHistoryAsync(MergedCustomerHistory mergedCustomerHistory)
        {
            await _dbContext.MergedCustomersHistory.AddAsync(mergedCustomerHistory);
        }

        public async Task<List<MergedCustomerHistory>> GetMergedCostumerHistoryListAsync()
        {
            return await _dbContext.MergedCustomersHistory.ToListAsync();
        }
    }
}
