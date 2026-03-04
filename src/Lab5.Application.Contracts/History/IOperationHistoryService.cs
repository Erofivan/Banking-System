using Lab5.Application.Contracts.History.Operations;

namespace Lab5.Application.Contracts.History;

public interface IOperationHistoryService
{
    GetHistory.Response GetHistory(GetHistory.Request request);
}