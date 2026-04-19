using Application.Abstractions.Messaging;
using Application.Users.GetById;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Users;

internal sealed class GetUserByIdQueryHandler(ApplicationReadDbContext context)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        UserResponse? user = await context.Users
            .Where(u => u.Id == query.UserId)
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
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return user;
    }
}
