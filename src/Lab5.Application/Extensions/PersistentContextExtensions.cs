using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Accounts;
using Lab5.Domain.Sessions;

namespace Lab5.Application.Extensions;

internal static class PersistentContextExtensions
{
    extension(IPersistenceContext context)
    {
        public UserSession? FindUserSession(SessionToken token)
        {
            return context.UserSessions
                .Query(UserSessionQuery.Build(builder => builder.WithToken(token)))
                .SingleOrDefault();
        }
    }

    extension(IPersistenceContext context)
    {
        public Account? FindAccount(AccountId accountId)
        {
            return context.Accounts
                .Query(AccountQuery.Build(builder => builder.WithId(accountId)))
                .SingleOrDefault();
        }
    }

    extension(IPersistenceContext context)
    {
        public AdminSession? FindAdminSession(SessionToken token)
        {
            return context.AdminSessions
                .Query(AdminSessionQuery.Build(builder => builder.WithToken(token)))
                .SingleOrDefault();
        }
    }
}