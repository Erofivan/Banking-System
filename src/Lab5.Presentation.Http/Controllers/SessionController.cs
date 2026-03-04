using Lab5.Application.Contracts.Sessions;
using Lab5.Application.Contracts.Sessions.Dtos;
using Lab5.Application.Contracts.Sessions.Operations;
using Lab5.Presentation.Http.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab5.Presentation.Http.Controllers;

[ApiController]
[Route("api/sessions")]
public sealed class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("user")]
    public ActionResult<SessionDto> LoginUser([FromBody] LoginUserRequest httpRequest)
    {
        var request = new LoginUser.Request(httpRequest.AccountId, httpRequest.PinCode);
        LoginUser.Response response = _sessionService.LoginUser(request);

        return response switch
        {
            LoginUser.Response.Success success => Ok(success.Session),
            LoginUser.Response.InvalidCredentials invalid => BadRequest(invalid.Message),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("admin")]
    public ActionResult<SessionDto> LoginAdmin([FromBody] LoginAdminRequest httpRequest)
    {
        var request = new LoginAdmin.Request(httpRequest.Password);
        LoginAdmin.Response response = _sessionService.LoginAdmin(request);

        return response switch
        {
            LoginAdmin.Response.Success success => Ok(success.Session),
            LoginAdmin.Response.InvalidPassword invalid => BadRequest(invalid.Message),
            _ => throw new UnreachableException(),
        };
    }
}
