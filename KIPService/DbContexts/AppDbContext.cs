using KIPService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace KIPService.DbContexts
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Отчеты
        /// </summary>
        public virtual DbSet<Report> Reports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Report>(p =>
            {
                p.HasKey(nameof(Report.ReportID));
                p.Property(s => s.ReportID).ValueGeneratedOnAdd();
                p.Property(s => s.StartDate).HasColumnType("timestamp without time zone");
                p.Property(s => s.EndDate).HasColumnType("timestamp without time zone");
                p.Property(s => s.InitDate).HasColumnType("timestamp without time zone");
            });

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
