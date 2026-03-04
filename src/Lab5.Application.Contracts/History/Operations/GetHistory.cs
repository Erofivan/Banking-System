using Lab5.Application.Contracts.History.Dtos;

namespace Lab5.Application.Contracts.History.Operations;

public static class GetHistory
{
    public readonly record struct Request(Guid UserToken);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<OperationRecordDto> Records) : Response;

        public sealed record Unauthorized(string Message) : Response;
    }
}
