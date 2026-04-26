using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Products.Api.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    //private readonly PostgreSqlContainer _dbContainer =
    //    new PostgreSqlBuilder("postgres:latest")
    //        .WithDatabase("eshop")
    //        .WithUsername("postgres")
    //        .WithPassword("postgres")
    //        .Build();

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong_password_123!")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // PostgreSQL supports snake case naming convention, so we will use it.
                //options.UseNpgsql(_dbContainer.GetConnectionString()).UseSnakeCaseNamingConvention();

                // MS SQL Server does not support snake case naming convention, so we will use the default naming convention.
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}