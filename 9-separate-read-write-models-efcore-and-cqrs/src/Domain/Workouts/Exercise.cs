using SharedKernel;

namespace Domain.Workouts;

public sealed class Exercise(Guid id, ExerciseType type, Distance distance) : Entity(id)
{
    public ExerciseType Type { get; private set; } = type;

    public Distance Distance { get; private set; } = distance;
}
