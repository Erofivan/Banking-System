using Lab5.Application.Contracts.History.Dtos;
using Lab5.Domain.Operations;
using System.Diagnostics;

namespace Lab5.Application.Mapping;

public static class OperationRecondMapping
{
    public static string ToDto(this OperationType type)
    {
        return type switch
        {
            OperationType.Deposit => "Deposit",
            OperationType.Withdrawal => "Withdrawal",
            _ => throw new UnreachableException(),
        };
    }

    public static OperationRecordDto ToDto(this OperationRecord record)
    {
        return new OperationRecordDto(
            record.Id.Value,
            record.AccountId.Value,
            record.Type.ToDto(),
            record.Amount.Value);
    }
}