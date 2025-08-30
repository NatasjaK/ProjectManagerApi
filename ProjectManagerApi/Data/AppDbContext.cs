using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects => Set<Project>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(p => p.Description)
                      .HasMaxLength(2000);

                entity.Property(p => p.CreatedAtUtc)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
