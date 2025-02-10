using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Entities.DTOs;
using CustomerRecognitionService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRecognitionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerRecognitionController : ControllerBase
    {
        private readonly ILogger<CustomerRecognitionController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IMergedCustomerHistoryService _mergedCustomerHistoryService;
  
        public CustomerRecognitionController(ILogger<CustomerRecognitionController> logger, ICustomerService customerService, IMergedCustomerHistoryService mergedCustomerHistoryService)
        {
            _logger = logger;
            _customerService = customerService;
            _mergedCustomerHistoryService = mergedCustomerHistoryService;
        }

        [HttpGet("GetMergedCustomerHistory",Name = "GetMergedCustomerHistory")]
        public async Task<IActionResult> GetMergedCustomerHistoryAsync()
        {
            _logger.LogInformation($"{nameof(GetMergedCustomerHistoryAsync)} Getting merge customer history.");
            var result = await _mergedCustomerHistoryService.GetMergedCostumerHistoryListAsync();

            if (!result.Success)
            {
                _logger.LogError($"{nameof(GetMergedCustomerHistoryAsync)} Getting merge customer history failed. Message: {result.Message}");
                return Problem(result.Message);
            }

            return Ok(result.Data); 
        }

        [HttpPost("SaveCustomer",Name = "SaveCustomer")]
        public async Task<IActionResult> SaveCustomerAsync(Customer data)
        {
            _logger.LogInformation($"{nameof(SaveCustomerAsync)} Saving customer.");
            var result = await _customerService.SaveCustomerAsync(data);

            if (!result.Success)
            {
                _logger.LogError($"{nameof(SaveCustomerAsync)} Saving customer failed. Message: {result.Message}");
                return Problem(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
