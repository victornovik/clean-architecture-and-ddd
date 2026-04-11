using Domain.Abstractions;
using Domain.Users;

namespace Domain.Followers;

public sealed class FollowerService(IFollowerRepository followerRepository)
{
    public async Task<Result> StartFollowingAsync(User user, User followed, DateTime utcNow, CancellationToken cancellationToken)
    {
        if (user.Id == followed.Id)
        {
            return FollowerErrors.SameUser;
        }

        if (!followed.HasPublicProfile)
        {
            return FollowerErrors.NonPublicProfile;
        }

        if (await followerRepository.IsAlreadyFollowingAsync(user.Id, followed.Id, cancellationToken))
        {
            return FollowerErrors.AlreadyFollowing;
        }

        var follower = Follower.Create(user.Id, followed.Id, utcNow);

        followerRepository.Insert(follower);

        return Result.Success();
    }
}