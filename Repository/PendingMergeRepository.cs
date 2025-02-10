using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Repository
{
    public class PendingMergeRepository : IPendingMergeRepository
    {
        private readonly AppDbContext _dbContext;

        public PendingMergeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SavePendingMergesAsync(List<PendingMerge> pendingMerges)
        {
            await _dbContext.PendingMerges.AddRangeAsync(pendingMerges);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PendingMerge>> GetPendingMergeListAsync()
        {
            return await _dbContext.PendingMerges.Where(pm => !pm.IsProcessed).ToListAsync();
        }
    }
}
