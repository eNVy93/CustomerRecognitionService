using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Repository.Interfaces
{
    public interface IMergedCustomerHistoryRepository
    {
        Task<List<MergedCustomerHistory>> GetMergedCostumerHistoryListAsync();
        Task SaveMergedCustomerHistoryAsync(MergedCustomerHistory mergedCustomerHistory);
    }
}