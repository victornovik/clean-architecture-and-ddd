using Gatherly.Domain.Primitives;
using Gatherly.Domain.ValueObjects;

namespace Gatherly.Domain.Entities;

public sealed class Member(Guid id, Email email, FirstName firstName, LastName lastName)
    : Entity(id)
{
    public Email Email { get; set; } = email;

    public FirstName FirstName { get; set; } = firstName;

    public LastName LastName { get; set; } = lastName;
}