using Domain.Workouts;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

internal sealed class ExerciseRepository(ApplicationWriteDbContext context) : IExerciseRepository
{
    public void InsertRange(IEnumerable<Exercise> exercises)
    {
        context.Exercises.AddRange(exercises);
    }
}
