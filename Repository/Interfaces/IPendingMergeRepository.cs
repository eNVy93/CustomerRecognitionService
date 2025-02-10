using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Repository.Interfaces
{
    public interface IPendingMergeRepository
    {
        Task SavePendingMergesAsync(List<PendingMerge> pendingMerges);
        Task<List<PendingMerge>> GetPendingMergeListAsync();
    }
}