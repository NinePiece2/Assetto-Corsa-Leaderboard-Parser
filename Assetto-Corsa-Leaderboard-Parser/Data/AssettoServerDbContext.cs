using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assetto_Corsa_Leaderboard_Parser.Data.Tables;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace Assetto_Corsa_Leaderboard_Parser.Data
{
    public class AssettoServerDbContext : DbContext
    {
        public AssettoServerDbContext()
        {
        }

        public AssettoServerDbContext(DbContextOptions<AssettoServerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leaderboard> Leaderboard { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["AssettoServerContextConnection"].ConnectionString);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Leaderboard>(entity =>
            {
                entity.HasKey(e => e.UID);
            });
        }
    }
}
