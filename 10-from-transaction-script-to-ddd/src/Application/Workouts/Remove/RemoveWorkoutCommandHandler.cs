using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Domain.Workouts;
using SharedKernel;

namespace Application.Workouts.Remove;

internal sealed class RemoveWorkoutCommandHandler(
    IWorkoutRepository workoutRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveWorkoutCommand>
{
    public async Task<Result> Handle(
        RemoveWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        Workout? workout = await workoutRepository.GetByIdAsync(
            request.WorkoutId,
            cancellationToken);

        if (workout is null)
        {
            return Result.Failure(WorkoutErrors.NotFound(request.WorkoutId));
        }

        workoutRepository.Remove(workout);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
