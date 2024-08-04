using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Shares.Models.Candle
{
    public class CandleModel
    {
        [Key]
        public int Id { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        [MaybeNull]
        public decimal Average { get; set; } = 0;
    }
}
