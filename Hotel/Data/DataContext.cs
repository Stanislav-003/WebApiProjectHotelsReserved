using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasKey(r => new { r.PlaceId, r.UserId });
            modelBuilder.Entity<Reservation>()
                .HasOne(u => u.User)
                .WithMany(r => r.Reservations)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction); 
            modelBuilder.Entity<Reservation>()
                .HasOne(u => u.Place)
                .WithMany(r => r.Reservations)
                .HasForeignKey(u => u.PlaceId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
