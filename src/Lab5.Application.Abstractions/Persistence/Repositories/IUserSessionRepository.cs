using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Sessions;

namespace Lab5.Application.Abstractions.Persistence.Repositories;

public interface IUserSessionRepository
{
    void Add(UserSession session);

    IEnumerable<UserSession> Query(UserSessionQuery query);
}
