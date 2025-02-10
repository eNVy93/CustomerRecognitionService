using CustomerRecognitionService.Entities;
using CustomerRecognitionService.Services.Interfaces;

namespace CustomerRecognitionService.Services.Background
{
    public class MergeDuplicateCustomersBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PeriodicTimer _timer;

        public MergeDuplicateCustomersBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope()) 
                    {
                        var customerMergeService = scope.ServiceProvider.GetRequiredService<ICustomerMergeService>();
                        await customerMergeService.ProcessMergesAsync();
                    }
                }
                catch (Exception ex)
                {
                    // implement logging
                }
                await _timer.WaitForNextTickAsync(stoppingToken);
            }
            _timer?.Dispose();
        }
    }
}
