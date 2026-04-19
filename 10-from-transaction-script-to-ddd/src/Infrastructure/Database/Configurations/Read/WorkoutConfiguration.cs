using Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Read;

internal sealed class WorkoutConfiguration : IEntityTypeConfiguration<WorkoutReadModel>
{
    public void Configure(EntityTypeBuilder<WorkoutReadModel> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);

        builder.HasMany(w => w.Exercises)
            .WithOne()
            .HasForeignKey(e => e.WorkoutId);
    }
}
