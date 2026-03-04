namespace Lab5.Application.Contracts.Accounts.Operations;

public static class Deposit
{
    public readonly record struct Request(Guid UserToken, int Amount);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(int NewBalance) : Response;

        public sealed record Unauthorized(string Message) : Response;
    }
}