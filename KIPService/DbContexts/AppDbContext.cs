using KIPService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace KIPService.DbContexts
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Report> Reports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Report>(p =>
            {
                p.HasKey(nameof(Report.ReportID));
                p.Property(s => s.ReportID).HasDefaultValueSql("newsequentialid()");
                p.Property(s => s.StartDate).HasColumnType("timestamp");
                p.Property(s => s.EndDate).HasColumnType("timestamp");
                p.Property(s => s.InitDate).HasColumnType("timestamp");
            });

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
