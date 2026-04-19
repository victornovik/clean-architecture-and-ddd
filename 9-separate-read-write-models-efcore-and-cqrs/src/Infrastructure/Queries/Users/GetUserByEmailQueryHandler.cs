using System.Data;
using Application.Abstractions.Messaging;
using Application.Users.GetByEmail;
using Dapper;
using Domain.Users;
using Infrastructure.Data;
using SharedKernel;

namespace Infrastructure.Queries.Users;

internal sealed class GetUserByEmailQueryHandler(DbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        using IDbConnection connection = dbConnectionFactory.CreateOpenConnection();

        const string sql =
            """
            SELECT u.Id, u.Email, u.Name, u.HasPublicProfile
            FROM Users u
            WHERE u.Email = @Email
            """;

        UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(sql, new { query.Email });

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail(query.Email));
        }

        return user;
    }
}
