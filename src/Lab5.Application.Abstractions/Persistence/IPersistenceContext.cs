using Lab5.Application.Abstractions.Persistence.Repositories;

namespace Lab5.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IAccountRepository Accounts { get; }

    IUserSessionRepository UserSessions { get; }

    IAdminSessionRepository AdminSessions { get; }

    IOperationRecordRepository OperationRecords { get; }
}