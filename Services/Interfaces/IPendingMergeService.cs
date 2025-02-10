using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Services.Interfaces
{
    public interface IPendingMergeService
    {
        Task<Result<List<PendingMerge>>> GetPendingMergeListAsync();
        Task<Result<string>> SavePendingMergesAsync(List<PendingMerge> pendingMerges);
    }
}