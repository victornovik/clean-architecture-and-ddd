using Domain.Users;
using SharedKernel;

namespace Domain.Followers;

public sealed class FollowerService(
    IFollowerRepository followerRepository,
    IDateTimeProvider dateTimeProvider)
    : IFollowerService
{
    public async Task<Result> StartFollowingAsync(User user, User followed, CancellationToken cancellationToken)
    {
        if (user.Id == followed.Id)
        {
            return FollowerErrors.SameUser;
        }

        if (!followed.HasPublicProfile)
        {
            return FollowerErrors.NonPublicProfile;
        }

        if (await followerRepository.IsAlreadyFollowingAsync(
                user.Id,
                followed.Id,
                cancellationToken))
        {
            return FollowerErrors.AlreadyFollowing;
        }

        var follower = Follower.Create(user.Id, followed.Id, dateTimeProvider.UtcNow);

        followerRepository.Insert(follower);

        return Result.Success();
    }
}
