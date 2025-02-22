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
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {

            _logger.LogInformation("Timed Hosted Service is working. Count");
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
