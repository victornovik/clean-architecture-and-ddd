using Gatherly.Application.Abstractions.Messaging;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.Shared;
using Gatherly.Domain.ValueObjects;

namespace Gatherly.Application.Members.Commands.CreateMember;

internal sealed class CreateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateMemberCommand>
{
    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);

        var member = new Member(Guid.NewGuid(), emailResult.Value, firstNameResult.Value, lastNameResult.Value);
        memberRepository.Add(member);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}