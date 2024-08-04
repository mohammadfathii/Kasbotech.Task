using BlogAPI.FirstMicroservice.Data;
using BlogAPI.FirstMicroservice.Services.IServices;
using BlogAPI.Shares.Models.Candle;

namespace BlogAPI.FirstMicroservice.Services
{
    public class CandleService : ICandleService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CandleService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public decimal RandomPrice()
        {
            var random = new Random();
            return Convert.ToDecimal(random.NextDouble() * 100);
        }

        public void CreateCandle()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                dbContext.Candles.Add(new CandleModel()
                {
                    Low = RandomPrice(),
                    High = RandomPrice(),
                    Open = RandomPrice(),
                    Close = RandomPrice(),
                    Average = 0
                });
                dbContext.SaveChanges();

                dbContext.Logs.Add(new Shares.Models.Log.LogModel()
                {
                    Success = true,
                    TimeStamp = DateTime.Now,
                });

                dbContext.SaveChanges();
            }
        }

    }
}
