using BlogAPI.Data;
using BlogAPI.Services.IServices;
using BlogAPI.Shares.Models.Candle;

namespace BlogAPI.Services
{
    public class CandleService : ICandleService
    {
        private AppDBContext _context;
        public CandleService(AppDBContext context)
        {
            _context = context;
        }
        public bool CreateCandle(CreateCandleModel candle)
        {
            if (candle == null)
            {
                return false;
            }
            _context.Candles.Add(new CandleModel()
            {
                Low = candle.Low,
                High = candle.High,
                Open = candle.Open,
                Close = candle.Close,
                Average = 0
            });
            _context.SaveChanges();
            return true;
        }

        public List<CandleModel> GetCandles()
        {
            return _context.Candles.ToList();
        }
    }
}
