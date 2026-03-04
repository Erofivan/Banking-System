using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Accounts;
using Lab5.Domain.Sessions;

namespace Lab5.Application.Extensions;

internal static class PersistentContextExtensions
{
    public static UserSession? FindUserSession(this IPersistenceContext context, SessionToken token)
    {
        return context.UserSessions
            .Query(UserSessionQuery.Build(builder => builder.WithToken(token)))
            .SingleOrDefault();
    }

    public static Account? FindAccount(this IPersistenceContext context, AccountId accountId)
    {
        return context.Accounts
            .Query(AccountQuery.Build(builder => builder.WithId(accountId)))
            .SingleOrDefault();
    }

    public static AdminSession? FindAdminSession(this IPersistenceContext context, SessionToken token)
    {
        return context.AdminSessions
            .Query(AdminSessionQuery.Build(builder => builder.WithToken(token)))
            .SingleOrDefault();
    }
}