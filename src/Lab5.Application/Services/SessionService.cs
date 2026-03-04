using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Contracts.Sessions;
using Lab5.Application.Contracts.Sessions.Dtos;
using Lab5.Application.Contracts.Sessions.Operations;
using Lab5.Domain.Accounts;
using Lab5.Domain.Sessions;
using Lab5.Domain.ValueObjects;

namespace Lab5.Application.Services;

public sealed class SessionService : ISessionService
{
    private readonly IPersistenceContext _persistence;
    private readonly AtmOptions _options;

    public SessionService(IPersistenceContext persistence, Microsoft.Extensions.Options.IOptions<AtmOptions> options)
    {
        _persistence = persistence;
        _options = options.Value;
    }

    public LoginUser.Response LoginUser(LoginUser.Request request)
    {
        var accountId = new AccountId(request.AccountId);

        var query = AccountQuery.Build(builder => builder.WithId(accountId));

        Account? account = _persistence.Accounts.Query(query).SingleOrDefault();

        if (account is null)
            return new LoginUser.Response.InvalidCredentials("Account not found");

        if (account.PinCode != new PinCode(request.PinCode))
            return new LoginUser.Response.InvalidCredentials("Invalid pin code");

        var session = new UserSession(SessionToken.Generate(), account.Id);

        _persistence.UserSessions.Add(session);

        return new LoginUser.Response.Success(new SessionDto(session.Token.Value));
    }

    public LoginAdmin.Response LoginAdmin(LoginAdmin.Request request)
    {
        if (string.Equals(request.Password, _options.SystemPassword, StringComparison.Ordinal) is false)
            return new LoginAdmin.Response.InvalidPassword("Invalid system password");

        var session = new AdminSession(SessionToken.Generate());

        _persistence.AdminSessions.Add(session);

        return new LoginAdmin.Response.Success(new SessionDto(session.Token.Value));
    }
}