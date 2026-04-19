using Application.Abstractions.Data;
using Domain.Followers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    public DbSet<Follower> Followers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationWriteDbContext).Assembly, type => type.FullName?.Contains("Configurations.Write") ?? false);
    }
}
