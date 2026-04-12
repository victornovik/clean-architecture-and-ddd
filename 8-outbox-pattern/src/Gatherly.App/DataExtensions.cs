using Gatherly.Persistence;
using Microsoft.EntityFrameworkCore;
// ReSharper disable SuggestVarOrType_SimpleTypes

public static class DataExtensions
{
    // Apply all migrations that have not been applied yet
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}
