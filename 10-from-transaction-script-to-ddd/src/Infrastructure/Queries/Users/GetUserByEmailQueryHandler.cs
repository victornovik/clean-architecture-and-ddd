using Application.Abstractions.Messaging;
using Application.Users.GetByEmail;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Users;

internal sealed class GetUserByEmailQueryHandler(ApplicationReadDbContext context)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByEmailQuery query,
        CancellationToken cancellationToken)
    {
        UserResponse? user = await context.Users
            .Where(u => u.Email == query.Email)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                HasPublicProfile = u.HasPublicProfile
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        return user;
    }
}
