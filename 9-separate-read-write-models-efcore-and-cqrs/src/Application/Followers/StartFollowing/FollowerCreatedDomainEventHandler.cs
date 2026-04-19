using Application.Abstractions.Notifications;
using Application.Users;
using Application.Users.GetById;
using Domain.Followers;
using MediatR;
using SharedKernel;

namespace Application.Followers.StartFollowing;

internal class FollowerCreatedDomainEventHandler(
    ISender sender,
    INotificationService notificationService)
    : INotificationHandler<FollowerCreatedDomainEvent>
{
    public async Task Handle(FollowerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(notification.UserId);

        Result<UserResponse> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new UserNotFoundException(notification.UserId);
        }

        await notificationService.SendAsync(
            notification.FollowedId,
            $"{result.Value.Name} started following you!",
            cancellationToken);
    }
}
