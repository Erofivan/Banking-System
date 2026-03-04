namespace Lab5.Application.Contracts.Accounts.Operations;

public static class GetBalance
{
    public readonly record struct Request(Guid UserToken);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(int Balance) : Response;

        public sealed record Unauthorized(string Message) : Response;
    }
}