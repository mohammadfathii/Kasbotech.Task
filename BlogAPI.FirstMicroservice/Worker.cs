using BlogAPI.FirstMicroservice.Data;
using BlogAPI.FirstMicroservice.Services.IServices;
using BlogAPI.Shares.Models.Candle;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.FirstMicroservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer _timer;
        private ICandleService _candleService;


        public Worker(ILogger<Worker> logger,ICandleService candleService)
        {
            _logger = logger;
            _candleService = candleService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker service starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Worker service is creating candle at: {time}", DateTimeOffset.Now);
            // Call your method here
            CreateCandle();
        }

        public void CreateCandle()
        {
            _candleService.CreateCandle();
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
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
