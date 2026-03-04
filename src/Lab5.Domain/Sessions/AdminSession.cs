namespace Lab5.Domain.Sessions;

public sealed class AdminSession
{
    public AdminSession(SessionToken token)
    {
        Token = token;
    }

    public SessionToken Token { get; }
}
