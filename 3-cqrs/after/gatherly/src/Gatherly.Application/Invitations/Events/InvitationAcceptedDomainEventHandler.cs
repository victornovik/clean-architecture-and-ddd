using Gatherly.Application.Abstractions;
using Gatherly.Domain.DomainEvents;
using Gatherly.Domain.Repositories;
using MediatR;

namespace Gatherly.Application.Invitations.Events;

internal sealed class InvitationAcceptedDomainEventHandler(
    IEmailService emailService,
    IGatheringRepository gatheringRepository)
    : INotificationHandler<InvitationAcceptedDomainEvent>
{
    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var gathering = await gatheringRepository.GetByIdWithCreatorAsync(
            notification.GatheringId, cancellationToken);

        if (gathering is null)
        {
            return;
        }

        await emailService.SendInvitationAcceptedEmailAsync(
            gathering,
            cancellationToken);
    }
}