using Access.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        
        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.Price).HasPrecision(18,2);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Price).HasPrecision(18,2);;

            modelBuilder.Entity<Event>()
                .Property(e => e.RowVersion)
                .IsRowVersion();
        }
    }
}   
