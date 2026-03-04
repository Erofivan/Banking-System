namespace Lab5.Domain.Sessions;

public readonly record struct SessionToken(Guid Value)
{
    public static SessionToken Generate()
    {
        return new SessionToken(Guid.NewGuid());
    }
}
