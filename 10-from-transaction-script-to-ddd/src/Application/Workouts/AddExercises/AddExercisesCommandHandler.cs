using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Domain.Workouts;
using SharedKernel;

namespace Application.Workouts.AddExercises;

internal sealed class AddExercisesCommandHandler(
    IWorkoutRepository workoutRepository,
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddExercisesCommand>
{
    public async Task<Result> Handle(
        AddExercisesCommand request,
        CancellationToken cancellationToken)
    {
        Workout? workout = await workoutRepository.GetByIdAsync(
            request.WorkoutId,
            cancellationToken);

        if (workout is null)
        {
            return Result.Failure(WorkoutErrors.NotFound(request.WorkoutId));
        }

        var results = request
            .Exercises
            .Select(exerciseRequest =>
                workout.AddExercise(
                    exerciseRequest.ExerciseType,
                    exerciseRequest.TargetType,
                    exerciseRequest.DistanceInMeters,
                    exerciseRequest.DurationInSeconds))
            .ToList();

        if (results.Any(r => r.IsFailure))
        {
            return Result.Failure(ValidationError.FromResults(results));
        }

        exerciseRepository.InsertRange(workout.Exercises);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
