 using Microsoft.EntityFrameworkCore;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.DataBaseEngine
{
    public class ScopeLapDbContext : DbContext 
    {
        public ScopeLapDbContext(DbContextOptions<ScopeLapDbContext> options) : base(options) { }
        
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Car> Cars{ get; set; }
        
        public DbSet<Commentary> Commentaries { get; set; }
        
        public DbSet<LapSession> Sessions { get; set; }
        
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<Track> Tracks { get; set; }
        
        public DbSet<TrackConfiguration> TrackConfigurations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
