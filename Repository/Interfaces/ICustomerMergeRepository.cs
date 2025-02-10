using CustomerRecognitionService.Entities;

namespace CustomerRecognitionService.Repository.Interfaces
{
    public interface ICustomerMergeRepository
    {
        Task SaveProcessedMerges(List<PendingMerge> pendingMerges, List<MergedCustomerHistory> mergedHistories);
    }
}