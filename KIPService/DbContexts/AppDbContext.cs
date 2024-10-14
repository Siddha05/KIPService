using KIPService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace KIPService.DbContexts
{
    public class AppDbContext : DbContext
    {
        DbSet<Report> Reports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
