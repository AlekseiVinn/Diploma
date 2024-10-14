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
        
        public DbSet<TrackDay> TrackDays{ get; set; }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackConfiguration>()
                .HasMany(e => e.Sessions)
                .WithMany(e => e.Tracks)
                .UsingEntity<TrackDay>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
