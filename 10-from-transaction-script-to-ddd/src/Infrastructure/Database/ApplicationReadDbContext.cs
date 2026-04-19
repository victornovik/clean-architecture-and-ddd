using Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

internal sealed class ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options)
    : DbContext(options)
{
    public DbSet<UserReadModel> Users { get; set; }

    public DbSet<FollowerReadModel> Followers { get; set; }

    public DbSet<WorkoutReadModel> Workouts { get; set; }

    public DbSet<ExerciseReadModel> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationReadDbContext).Assembly,
            ReadConfigurationsFilter);
    }

    private static bool ReadConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations.Read") ?? false;
}
