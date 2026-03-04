using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Repositories;

namespace Lab5.Infrastructure.Persistence;

public sealed class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(
        IAccountRepository accounts,
        IUserSessionRepository userSessions,
        IAdminSessionRepository adminSessions,
        IOperationRecordRepository operationRecords)
    {
        Accounts = accounts;
        UserSessions = userSessions;
        AdminSessions = adminSessions;
        OperationRecords = operationRecords;
    }

    public IAccountRepository Accounts { get; }

    public IUserSessionRepository UserSessions { get; }

    public IAdminSessionRepository AdminSessions { get; }

    public IOperationRecordRepository OperationRecords { get; }
}