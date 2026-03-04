using Lab5.Application.Contracts.Sessions.Operations;

namespace Lab5.Application.Contracts.Sessions;

public interface ISessionService
{
    LoginUser.Response LoginUser(LoginUser.Request request);

    LoginAdmin.Response LoginAdmin(LoginAdmin.Request request);
}