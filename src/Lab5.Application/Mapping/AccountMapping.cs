using Lab5.Application.Contracts.Accounts.Dtos;
using Lab5.Domain.Accounts;

namespace Lab5.Application.Mapping;

internal static class AccountMapping
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto(account.Id.Value, account.Balance.Value);
    }
}