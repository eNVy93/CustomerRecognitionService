using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using CustomerRecognitionService.Services.Interfaces;

namespace CustomerRecognitionService.Services
{
    public class PendingMergeService : IPendingMergeService
    {
        private readonly IPendingMergeRepository _pendingMergeRepository;

        public PendingMergeService(IPendingMergeRepository pendingMergeRepository)
        {
            _pendingMergeRepository = pendingMergeRepository;
        }

        public async Task<Result<string>> SavePendingMergesAsync(List<PendingMerge> pendingMerges)
        {
            try
            {
                await _pendingMergeRepository.SavePendingMergesAsync(pendingMerges);
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

        public async Task<Result<List<PendingMerge>>> GetPendingMergeListAsync()
        {
            try
            {
                var data = await _pendingMergeRepository.GetPendingMergeListAsync();
                return Result<List<PendingMerge>>.Ok(data);
            }
            catch (OperationCanceledException)
            {
                return Result<List<PendingMerge>>.Fail("Operation canceled");
            }
            catch (Exception)
            {
                return Result<List<PendingMerge>>.Fail("Unexpected error occured");
            }
        }
    }
}
