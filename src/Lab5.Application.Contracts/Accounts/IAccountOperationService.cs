using Lab5.Application.Contracts.Accounts.Operations;

namespace Lab5.Application.Contracts.Accounts;

public interface IAccountOperationService
{
    CreateAccount.Response Create(CreateAccount.Request request);

    GetBalance.Response GetBalance(GetBalance.Request request);

    Deposit.Response Deposit(Deposit.Request request);

    Withdraw.Response Withdraw(Withdraw.Request request);
}