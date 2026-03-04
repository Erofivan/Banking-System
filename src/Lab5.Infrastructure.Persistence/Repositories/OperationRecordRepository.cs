using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Domain.Operations;

namespace Lab5.Infrastructure.Persistence.Repositories;

public sealed class OperationRecordRepository : IOperationRecordRepository
{
    private readonly Dictionary<OperationRecordId, OperationRecord> _values = [];

    public OperationRecord Add(OperationRecord record)
    {
        record = new OperationRecord(
            new OperationRecordId(_values.Count + 1),
            record.AccountId,
            record.Type,
            record.Amount);

        _values.Add(record.Id, record);

        return record;
    }

    public IEnumerable<OperationRecord> Query(OperationRecordQuery query)
    {
        return _values.Values
            .Where(x => query.AccountIds is [] || query.AccountIds.Contains(x.AccountId));
    }
}
