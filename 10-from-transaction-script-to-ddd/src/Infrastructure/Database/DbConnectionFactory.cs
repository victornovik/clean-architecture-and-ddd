using System.Data;
using Npgsql;

namespace Infrastructure.Database;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource)
{
    public IDbConnection OpenConnection()
    {
        NpgsqlConnection connection = dataSource.OpenConnection();

        return connection;
    }
}
