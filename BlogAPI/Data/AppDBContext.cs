using Microsoft.EntityFrameworkCore;
using BlogAPI.Shares.Models.Candle;
using BlogAPI.Shares.Models.Log;
using BlogAPI.Shares.Models.User;

namespace BlogAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CandleModel> Candles { get; set; }
        public DbSet<LogModel> Logs { get; set; }
    }
}
