using Lab5.Application.Contracts.Sessions.Dtos;

namespace Lab5.Application.Contracts.Sessions.Operations;

public static class LoginAdmin
{
    public readonly record struct Request(string Password);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(SessionDto Session) : Response;

        public sealed record InvalidPassword(string Message) : Response;
    }
}
