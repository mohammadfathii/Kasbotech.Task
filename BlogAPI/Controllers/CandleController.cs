using BlogAPI.Services.IServices;
using BlogAPI.Shares.Models.Candle;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    public class CandleController : Controller
    {
        private ICandleService _candleService;
        public CandleController(ICandleService candleService)
        {
            _candleService = candleService;
        }

        [HttpPost("/Candle/Create")]
        public bool Create(CreateCandleModel candle)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            _candleService.CreateCandle(candle);

            return true;
        }
    }
}
