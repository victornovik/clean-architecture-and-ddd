using Domain.Followers;
using Domain.Users;
using FluentAssertions;

namespace Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_CreateUser_WhenNameIsValid()
    {
        // Arrange
        var email = Email.Create("test@test.com");
        var name = new Name("Full Name");

        // Act
        var user = User.Create(email, name, true);

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_Should_RaiseDomainEvent_WhenNameIsValid()
    {
        // Arrange
        var email = Email.Create("test@test.com");
        var name = new Name("Full Name");

        // Act
        var user = User.Create(email, name, true);

        // Assert
        user.DomainEvents
            .Should().ContainSingle()
            .Which
            .Should().BeOfType<UserCreatedDomainEvent>();
    }

    private class FollowerRepositoryAlreadyFollowing : IFollowerRepository
    {
        public async Task<bool> IsAlreadyFollowingAsync(Guid userId, Guid followedId, CancellationToken cancellationToken)
            => await Task.FromResult(true);

        public void Insert(Follower follower) => throw new NotImplementedException();
    }

    [Fact]
    public async Task StartFollowing_Should_ReturnSameUserError()
    {
        // Arrange
        var email = Email.Create("test@test.com");
        var name = new Name("Full Name");

        // Act
        var followee = User.Create(email, name, hasPublicProfile:false);
        var srv = new FollowerService(new FollowerRepositoryAlreadyFollowing());
        var res = await srv.StartFollowingAsync(followee, followee, DateTime.UtcNow, CancellationToken.None);

        // Assert
        res.IsFailure.Should().BeTrue();
        res.IsSuccess.Should().BeFalse();
        res.Error.Code.Should().BeSameAs(FollowerErrors.SameUser.Code);
        res.Error.Description.Should().BeSameAs(FollowerErrors.SameUser.Description);
    }

    [Fact]
    public async Task StartFollowing_Should_ReturnNonPublicProfileError()
    {
        // Arrange
        var email = Email.Create("test@test.com");
        var name = new Name("Full Name");

        // Act
        var followee = User.Create(email, name, hasPublicProfile: false);
        var follower = User.Create(email, name, hasPublicProfile: true);
        var srv = new FollowerService(new FollowerRepositoryAlreadyFollowing());
        var res = await srv.StartFollowingAsync(follower, followee, DateTime.UtcNow, CancellationToken.None);

        // Assert
        res.IsFailure.Should().BeTrue();
        res.IsSuccess.Should().BeFalse();
        res.Error.Code.Should().BeSameAs(FollowerErrors.NonPublicProfile.Code);
        res.Error.Description.Should().BeSameAs(FollowerErrors.NonPublicProfile.Description);
    }

    [Fact]
    public async Task StartFollowing_Should_ReturnAlreadyFollowingError()
    {
        // Arrange
        var email = Email.Create("test@test.com");
        var name = new Name("Full Name");

        // Act
        var followee = User.Create(email, name, hasPublicProfile: true);
        var follower = User.Create(email, name, hasPublicProfile: true);
        var srv = new FollowerService(new FollowerRepositoryAlreadyFollowing());
        var res = await srv.StartFollowingAsync(follower, followee, DateTime.UtcNow, CancellationToken.None);

        // Assert
        res.IsFailure.Should().BeTrue();
        res.IsSuccess.Should().BeFalse();
        res.Error.Code.Should().BeSameAs(FollowerErrors.AlreadyFollowing.Code);
        res.Error.Description.Should().BeSameAs(FollowerErrors.AlreadyFollowing.Description);
    }
}
