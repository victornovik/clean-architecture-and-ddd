using Application.Abstractions.Messaging;
using Application.Workouts.GetById;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Workouts;

internal sealed class GetWorkoutByIdQueryHandler(ApplicationReadDbContext context)
    : IQueryHandler<GetWorkoutByIdQuery, WorkoutResponse>
{
    public async Task<Result<WorkoutResponse>> Handle(
        GetWorkoutByIdQuery request,
        CancellationToken cancellationToken)
    {
        WorkoutResponse? workout = await context.Workouts
            .Select(w => new WorkoutResponse
            {
                Id = w.Id,
                UserId = w.UserId,
                Name = w.Name,
                Exercises = w.Exercises.Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    ExerciseType = e.ExerciseType,
                    TargetType = e.TargetType,
                    Distance = e.Distance,
                    Duration = e.Duration
                })
                .ToList()
            })
            .FirstOrDefaultAsync(w => w.Id == request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            return Result.Failure<WorkoutResponse>(WorkoutErrors.NotFound(request.WorkoutId));
        }

        return workout;
    }
}
