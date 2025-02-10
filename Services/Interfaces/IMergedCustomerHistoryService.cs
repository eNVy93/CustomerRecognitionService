using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Services.Interfaces
{
    public interface IMergedCustomerHistoryService
    {
        Task<Result<List<MergedCustomerHistory>>> GetMergedCostumerHistoryListAsync();
        Task<Result<string>> SaveMergedCustomerHistoryAsync(MergedCustomerHistory mergedCustomerHistory);
    }
}