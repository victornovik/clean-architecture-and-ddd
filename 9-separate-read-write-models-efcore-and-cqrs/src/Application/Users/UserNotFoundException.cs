namespace Application.Users;

public sealed class UserNotFoundException(Guid userId)
    : Exception($"The user with the identifier {userId} was not found");
