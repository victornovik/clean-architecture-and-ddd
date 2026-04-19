namespace Application.Followers.GetFollowerStats;

public sealed record FollowerStatsResponse(Guid UserId, int FollowerCount, int FollowingCount);
