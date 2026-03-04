using Lab5.Domain.Accounts;
using Lab5.Domain.ValueObjects;

namespace Lab5.Domain.Operations;

public sealed class OperationRecord
{
    public OperationRecord(
        OperationRecordId id,
        AccountId accountId,
        OperationType type,
        Amount amount)
    {
        Id = id;
        AccountId = accountId;
        Type = type;
        Amount = amount;
    }

    public OperationRecordId Id { get; }

    public AccountId AccountId { get; }

    public OperationType Type { get; }

    public Amount Amount { get; }
}