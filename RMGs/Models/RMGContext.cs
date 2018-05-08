using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMGs.Models
{
    public class RMGContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<BookMarks> BookMarks { get; set; }

        public RMGContext(DbContextOptions<RMGContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstate>().OwnsOne(p => p.Property);
            modelBuilder.Entity<RealEstate>().OwnsOne(p => p.Location);
        }
    }
}
