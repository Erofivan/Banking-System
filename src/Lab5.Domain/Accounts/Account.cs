using Lab5.Domain.Accounts.Results;
using Lab5.Domain.ValueObjects;

namespace Lab5.Domain.Accounts;

public sealed class Account
{
    public Account(AccountId id, PinCode pinCode, Amount balance)
    {
        Id = id;
        PinCode = pinCode;
        Balance = balance;
    }

    public AccountId Id { get; }

    public PinCode PinCode { get; }

    public Amount Balance { get; private set; }

    public Amount Deposit(Amount amount)
    {
        Balance += amount;

        return Balance;
    }

    public WithdrawalResult Withdraw(Amount amount)
    {
        if (Balance.TrySubtract(amount, out Amount newBalance) is false)
        {
            return new WithdrawalResult.InsufficientFunds(
                $"Insufficient balance: {Balance.Value}, requested: {amount.Value}");
        }

        Balance = newBalance;

        return new WithdrawalResult.Success(Balance);
    }
}
