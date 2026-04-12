using System.Data;
using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

internal sealed class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateOpenConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}
