using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Domain.Sessions;

namespace Lab5.Infrastructure.Persistence.Repositories;

public sealed class UserSessionRepository : IUserSessionRepository
{
    private readonly List<UserSession> _values = [];

    public void Add(UserSession session)
    {
        _values.Add(session);
    }

    public IEnumerable<UserSession> Query(UserSessionQuery query)
    {
        return _values
            .Where(x => query.Tokens is [] || query.Tokens.Contains(x.Token));
    }
}
