using BlogAPI.SecondMicroservice.Services.IServices;

namespace BlogAPI.SecondMicroservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer _timer;
        private readonly ICandleService _candleService;

        public Worker(ILogger<Worker> logger ,ICandleService candleService)
        {
            _logger = logger;
            _candleService = candleService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker service starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Worker service is creating candle at: {time}", DateTimeOffset.Now);
            // Call your method here
            UpdateAverageCandles();
        }

        public void UpdateAverageCandles()
        {
            _candleService.UpdateAverageCandles();
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker service stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
