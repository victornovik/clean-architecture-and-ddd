using Application.Abstractions.Messaging;
using Application.Followers.GetFollowerStats;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Followers;

internal sealed class GetFollowerStatsQueryHandler(ApplicationReadDbContext context)
    : IQueryHandler<GetFollowerStatsQuery, FollowerStatsResponse>
{
    public async Task<Result<FollowerStatsResponse>> Handle(
        GetFollowerStatsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
        {
            return Result.Failure<FollowerStatsResponse>(UserErrors.NotFound(request.UserId));
        }

        int followerCount = await context.Followers
            .CountAsync(f => f.FollowedId == request.UserId, cancellationToken);
        
        int followingCount = await context.Followers
            .CountAsync(f => f.UserId == request.UserId, cancellationToken);

        return new FollowerStatsResponse(request.UserId, followerCount, followingCount);
    }
}
