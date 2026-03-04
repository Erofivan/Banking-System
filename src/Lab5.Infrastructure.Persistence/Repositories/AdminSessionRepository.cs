using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Domain.Sessions;

namespace Lab5.Infrastructure.Persistence.Repositories;

public sealed class AdminSessionRepository : IAdminSessionRepository
{
    private readonly List<AdminSession> _values = [];

    public void Add(AdminSession session)
    {
        _values.Add(session);
    }

    public IEnumerable<AdminSession> Query(AdminSessionQuery query)
    {
        return _values
            .Where(x => query.Tokens is [] || query.Tokens.Contains(x.Token));
    }
}
