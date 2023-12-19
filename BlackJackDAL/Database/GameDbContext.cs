using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Windows;
using UtilityLib.Game;

namespace BlackJackDAL.Database
{
    public class GameDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStats> Stats { get; set; }
        //public DbSet<Card> Cards { get; set; }

        public GameDbContext() { }

        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BlackJack;Trusted_Connection=true;TrustServerCertificate=true");
        }
    }
}
