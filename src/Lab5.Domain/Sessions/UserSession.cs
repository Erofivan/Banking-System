using Lab5.Domain.Accounts;

namespace Lab5.Domain.Sessions;

public sealed class UserSession
{
    public UserSession(SessionToken token, AccountId accountId)
    {
        Token = token;
        AccountId = accountId;
    }

    public SessionToken Token { get; }

    public AccountId AccountId { get; }
}
