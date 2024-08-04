using BlogAPI.SecondMicroservice.Data;
using BlogAPI.SecondMicroservice.Services.IServices;
using BlogAPI.Shares.Models.Candle;

namespace BlogAPI.SecondMicroservice.Services
{
    public class CandleService : ICandleService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CandleService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }
        public void UpdateAverageCandles()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                try
                {
                    var candles = dbContext.Candles
                        .Where(c => c.Average == 0)
                        .ToList();

                    foreach (var candle in candles)
                    {
                        candle.Average = ((candle.Open + candle.Close + candle.High + candle.Low) / 4);
                    }

                    dbContext.SaveChanges();


                    Console.WriteLine("Averages calculated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to calculate averages: {ex.Message}");
                }
            }
        }
    }
}
