using Lab5.Domain.ValueObjects;

namespace Lab5.Domain.Accounts.Results;

public abstract record WithdrawalResult
{
    private WithdrawalResult() { }

    public sealed record Success(Amount NewBalance) : WithdrawalResult;

    public sealed record InsufficientFunds(string Message) : WithdrawalResult;
}
