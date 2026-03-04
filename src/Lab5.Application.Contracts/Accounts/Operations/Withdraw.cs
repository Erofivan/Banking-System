namespace Lab5.Application.Contracts.Accounts.Operations;

public static class Withdraw
{
    public readonly record struct Request(Guid UserToken, int Amount);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(int NewBalance) : Response;

        public sealed record Unauthorized(string Message) : Response;

        public sealed record InsufficientFunds(string Message) : Response;
    }
}