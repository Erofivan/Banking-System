using Lab5.Application.Contracts.Accounts.Dtos;

namespace Lab5.Application.Contracts.Accounts.Operations;

public static class CreateAccount
{
    public readonly record struct Request(Guid AdminToken, string PinCode);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(AccountDto Account) : Response;

        public sealed record Unauthorized(string Message) : Response;
    }
}