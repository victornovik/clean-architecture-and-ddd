namespace Gatherly.Domain.Exceptions;

public sealed class GatheringInvitationsValidBeforeInHoursIsNullDomainException(string message)
    : DomainException(message);