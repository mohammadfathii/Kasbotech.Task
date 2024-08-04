using BlogAPI.Shares.Models.Candle;

namespace BlogAPI.Services.IServices
{
    public interface ICandleService
    {
        bool CreateCandle(CreateCandleModel candle);
        List<CandleModel> GetCandles();
    }
}
