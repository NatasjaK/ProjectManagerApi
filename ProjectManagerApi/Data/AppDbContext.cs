using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<ProjectOwner> ProjectOwners => Set<ProjectOwner>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Title).HasMaxLength(200).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(2000);
                entity.Property(p => p.CreatedAtUtc).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(p => p.ClientName).HasMaxLength(200);
                entity.Property(p => p.ProjectOwnerName).HasMaxLength(200);
                entity.Property(p => p.Budget).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Currency).HasMaxLength(10);
            });

            modelBuilder.Entity<Client>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.HasData(
                    new Client { Id = 1, Name = "Acme AB" },
                    new Client { Id = 2, Name = "Globex" },
                    new Client { Id = 3, Name = "Initech" }
                );
            });

            modelBuilder.Entity<ProjectOwner>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.HasData(
                    new ProjectOwner { Id = 1, Name = "Natasja Kauppi" },
                    new ProjectOwner { Id = 2, Name = "Knut Hansson" },
                    new ProjectOwner { Id = 3, Name = "David Hemler" }
                );
            });

        }
    }
}
