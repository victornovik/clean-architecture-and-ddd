using Domain.Followers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class FollowerRepository(ApplicationWriteDbContext dbContext) : IFollowerRepository
{
    public async Task<bool> IsAlreadyFollowingAsync(
        Guid userId,
        Guid followedId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Followers.AnyAsync(f =>
            f.UserId == userId && f.FollowedId == followedId,
            cancellationToken);
    }

    public void Insert(Follower follower)
    {
        dbContext.Followers.Add(follower);
    }
}
