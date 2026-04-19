using Application.Abstractions.Data;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

internal sealed class ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<UserReadModel> Users { get; set; }

    public DbSet<FollowerReadModel> Followers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationReadDbContext).Assembly, type => type.FullName?.Contains("Configurations.Read") ?? false);
        //modelBuilder.ApplyConfiguration(new UserReadModelConfiguration());
        //modelBuilder.ApplyConfiguration(new FollowerReadModelConfiguration());
    }
}
