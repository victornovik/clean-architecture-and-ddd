using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repository;

internal sealed class MemberRepository(ApplicationDbContext dbContext) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Set<Member>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await dbContext.Set<Member>().AnyAsync(x => x.Email == email, cancellationToken);

    public void Add(Member member) => dbContext.Set<Member>().Add(member);
}
