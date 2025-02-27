namespace LocalRNC.Services
{
    public class DGIIHostedService(ILogger<DGIIHostedService> logger) : IHostedService, IDisposable
    {
        private readonly ILogger<DGIIHostedService> _logger = logger;
        private Timer? _timer = null;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3600));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {

            _logger.LogInformation("Timed Hosted Service is working. Count");
            var dgiiService = new DGIIService();
            await dgiiService.DownloadFileAsync(filePath: "");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
