using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Application.Abstractions.Notifications;
using Domain.Followers;
using Domain.Users;
using Domain.Workouts;
using Infrastructure.Caching;
using Infrastructure.Database;
using Infrastructure.Notifications;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddSingleton(_ =>
            new DbConnectionFactory(new NpgsqlDataSourceBuilder(connectionString).Build()));

        services.AddDbContext<ApplicationReadDbContext>(
            options => options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddSingleton<PublishDomainEventsInterceptor>();
        services.AddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddDbContext<ApplicationWriteDbContext>(
            (sp, options) => options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationWriteDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowerRepository, FollowerRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        services.AddTransient<INotificationService, NotificationService>();

        services.AddDistributedMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();
    }
}
