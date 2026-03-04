using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Sessions;

namespace Lab5.Application.Abstractions.Persistence.Repositories;

public interface IAdminSessionRepository
{
    void Add(AdminSession session);

    IEnumerable<AdminSession> Query(AdminSessionQuery query);
}
