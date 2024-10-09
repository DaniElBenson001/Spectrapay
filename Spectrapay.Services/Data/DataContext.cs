using Microsoft.EntityFrameworkCore;
using Spectrapay.Models;
using Spectrapay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>()
                .Property(u => u.VirtualBalance)
                .HasColumnType("decimal(18,2)")  // specify type
                .HasPrecision(18, 2);             // specify precision and scale

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)")  // specify type
                .HasPrecision(18, 2);             // specify precision and scale

            // Configure Refund entity
            modelBuilder.Entity<Refund>()
                .Property(r => r.Amount)
                .HasColumnType("decimal(18,2)")  // specify type
                .HasPrecision(18, 2);             // specify precision and scale
        }
    }
}
