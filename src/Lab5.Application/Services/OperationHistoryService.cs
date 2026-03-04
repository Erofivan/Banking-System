using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Contracts.History;
using Lab5.Application.Contracts.History.Dtos;
using Lab5.Application.Contracts.History.Operations;
using Lab5.Application.Extensions;
using Lab5.Application.Mapping;
using Lab5.Domain.Accounts;
using Lab5.Domain.Operations;
using Lab5.Domain.Sessions;

namespace Lab5.Application.Services;

public sealed class OperationHistoryService : IOperationHistoryService
{
    private readonly IPersistenceContext _persistenceContext;

    public OperationHistoryService(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public GetHistory.Response GetHistory(GetHistory.Request request)
    {
        var token = new SessionToken(request.UserToken);

        UserSession? session = _persistenceContext.FindUserSession(token);

        if (session is null)
            return new GetHistory.Response.Unauthorized("Invalid user session");

        Account? account = _persistenceContext.FindAccount(session.AccountId);

        if (account is null)
            return new GetHistory.Response.Unauthorized("Account not found");

        OperationRecord[] records = _persistenceContext.OperationRecords
            .Query(OperationRecordQuery.Build(builder => builder.WithAccountId(account.Id)))
            .ToArray();

        IReadOnlyCollection<OperationRecordDto> dtos = records.Select(r => r.ToDto()).ToList();

        return new GetHistory.Response.Success(dtos);
    }
}