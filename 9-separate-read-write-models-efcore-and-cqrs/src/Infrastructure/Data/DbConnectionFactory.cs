using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

internal sealed class DbConnectionFactory(string connectionString)
{
    public IDbConnection CreateOpenConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}
