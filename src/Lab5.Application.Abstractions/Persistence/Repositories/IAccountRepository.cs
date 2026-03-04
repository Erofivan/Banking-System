using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Accounts;

namespace Lab5.Application.Abstractions.Persistence.Repositories;

public interface IAccountRepository
{
    Account Add(Account account);

    IEnumerable<Account> Query(AccountQuery query);
}