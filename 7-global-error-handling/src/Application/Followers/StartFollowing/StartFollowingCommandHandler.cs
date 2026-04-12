using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Followers;
using Domain.Users;
using SharedKernel;

namespace Application.Followers.StartFollowing;

internal sealed class StartFollowingCommandHandler(
    IUserRepository userRepository,
    IFollowerService followerService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<StartFollowingCommand>
{
    public async Task<Result> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            return UserErrors.NotFound(command.UserId);
        }

        User? followed = await userRepository.GetByIdAsync(command.FollowedId, cancellationToken);
        if (followed is null)
        {
            return UserErrors.NotFound(command.FollowedId);
        }

        Result result = await followerService.StartFollowingAsync(
            user,
            followed,
            cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
