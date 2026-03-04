namespace Lab5.Domain.Operations;

public readonly record struct OperationRecordId(long Value)
{
    public static readonly OperationRecordId Default = new(0);
}
