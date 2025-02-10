using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Repository.Interfaces;
using CustomerRecognitionService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerRecognitionService.Services
{
    public class MergedCustomerHistoryService : IMergedCustomerHistoryService 
    { 
        private readonly IMergedCustomerHistoryRepository _mergedCustomerHistoryRepository;

        public MergedCustomerHistoryService(IMergedCustomerHistoryRepository mergedCustomerHistoryRepository)
        {
            _mergedCustomerHistoryRepository = mergedCustomerHistoryRepository;
        }

        public async Task<Result<string>> SaveMergedCustomerHistoryAsync(MergedCustomerHistory mergedCustomerHistory)
        {
            try
            {
                await _mergedCustomerHistoryRepository.SaveMergedCustomerHistoryAsync(mergedCustomerHistory);
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

        public async Task<Result<List<MergedCustomerHistory>>> GetMergedCostumerHistoryListAsync()
        {
            try
            {
                var data = await _mergedCustomerHistoryRepository.GetMergedCostumerHistoryListAsync();
                return Result<List<MergedCustomerHistory>>.Ok(data);
            }
            catch (OperationCanceledException)
            {
                return Result<List<MergedCustomerHistory>>.Fail("Operation canceled");
            }
            catch (Exception)
            {
                return Result<List<MergedCustomerHistory>>.Fail("Unexpected error occured");
            }
        }
    }
}
