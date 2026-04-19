using Application.Abstractions.Messaging;
using Application.Followers.GetFollowerStats;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Followers;

internal sealed class GetFollowerStatsQueryHandler(ApplicationReadDbContext dbContext) : IQueryHandler<GetFollowerStatsQuery, FollowerStatsResponse>
{
    public async Task<Result<FollowerStatsResponse>> Handle(GetFollowerStatsQuery request, CancellationToken cancellationToken)
    {
        if (!await dbContext.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
        {
            return Result.Failure<FollowerStatsResponse>(UserErrors.NotFound(request.UserId));
        }

        int followerCount = await dbContext.Followers.CountAsync(f => f.FollowedId == request.UserId, cancellationToken);
        
        int followingCount = await dbContext.Followers.CountAsync(f => f.UserId == request.UserId, cancellationToken);

        var latestFollower = await dbContext.Followers
            .Where(f => f.FollowedId == request.UserId)
            .OrderByDescending(f => f.CreatedOnUtc)
            .Take(5)
            .Select(f => new
            {
                f.User.Id,
                f.User.Name,
                f.User.HasPublicProfile
            })
            .ToListAsync(cancellationToken);

        return new FollowerStatsResponse(request.UserId, followerCount, followingCount);
    }
}
