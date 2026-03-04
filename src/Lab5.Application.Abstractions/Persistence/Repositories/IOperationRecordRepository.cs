using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Domain.Operations;

namespace Lab5.Application.Abstractions.Persistence.Repositories;

public interface IOperationRecordRepository
{
    OperationRecord Add(OperationRecord record);

    IEnumerable<OperationRecord> Query(OperationRecordQuery query);
}
