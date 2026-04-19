using SharedKernel;

namespace Domain.Workouts;

public sealed class Workout : Entity
{
    private readonly List<Exercise> _exercises = new();

    public Workout(Guid id, Guid userId, string name)
        : base(id)
    {
        UserId = userId;
        Name = name;
    }

    private Workout()
    {
    }

    public Guid UserId { get; private set; }

    public string Name { get; private set; }

    public List<Exercise> Exercises => _exercises.ToList();

    public Result AddExercise(
        ExerciseType exerciseType,
        TargetType targetType,
        decimal? distance,
        int? duration)
    {
        Result<Exercise> result = Exercise.Create(
            Id,
            exerciseType,
            targetType,
            distance.HasValue ? new Distance(distance.Value) : null,
            duration.HasValue ? TimeSpan.FromSeconds(duration.Value) : null);

        if (result.IsFailure)
        {
            return result;
        }

        _exercises.Add(result.Value);

        return Result.Success();
    }
}
