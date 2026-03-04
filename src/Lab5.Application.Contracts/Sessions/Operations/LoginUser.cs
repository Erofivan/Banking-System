using Lab5.Application.Contracts.Sessions.Dtos;

namespace Lab5.Application.Contracts.Sessions.Operations;

public static class LoginUser
{
    public readonly record struct Request(long AccountId, string PinCode);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(SessionDto Session) : Response;

        public sealed record InvalidCredentials(string Message) : Response;
    }
}
