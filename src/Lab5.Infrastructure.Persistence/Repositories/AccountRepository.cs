using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Domain.Accounts;

namespace Lab5.Infrastructure.Persistence.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, Account> _values = [];

    public Account Add(Account account)
    {
        account = new Account(
            new AccountId(_values.Count + 1),
            account.PinCode,
            account.Balance);

        _values.Add(account.Id, account);

        return account;
    }

    public IEnumerable<Account> Query(AccountQuery query)
    {
        return _values.Values
            .Where(x => query.Ids is [] || query.Ids.Contains(x.Id));
    }
}