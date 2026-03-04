using Lab5.Application.Contracts.Accounts;
using Lab5.Application.Contracts.Accounts.Dtos;
using Lab5.Application.Contracts.Accounts.Operations;
using Lab5.Presentation.Http.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab5.Presentation.Http.Controllers;

[ApiController]
[Route("api/accounts")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountOperationService _accountOperationService;

    public AccountController(IAccountOperationService accountOperationService)
    {
        _accountOperationService = accountOperationService;
    }

    [HttpPost]
    public ActionResult<AccountDto> Create(
        [FromHeader(Name = "X-Admin-Token")] Guid adminToken,
        [FromBody] CreateAccountRequest httpRequest)
    {
        var request = new CreateAccount.Request(adminToken, httpRequest.PinCode);
        CreateAccount.Response response = _accountOperationService.Create(request);

        return response switch
        {
            CreateAccount.Response.Success success => Ok(success.Account),
            CreateAccount.Response.Unauthorized unauthorized => Unauthorized(),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet("balance")]
    public ActionResult<int> GetBalance([FromHeader(Name = "X-User-Token")] Guid userToken)
    {
        var request = new GetBalance.Request(userToken);
        GetBalance.Response response = _accountOperationService.GetBalance(request);

        return response switch
        {
            GetBalance.Response.Success success => Ok(success.Balance),
            GetBalance.Response.Unauthorized unauthorized => Unauthorized(),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("deposit")]
    public ActionResult<int> Deposit(
        [FromHeader(Name = "X-User-Token")] Guid userToken,
        [FromBody] MoneyOperationRequest httpRequest)
    {
        var request = new Deposit.Request(userToken, httpRequest.Amount);
        Deposit.Response response = _accountOperationService.Deposit(request);

        return response switch
        {
            Deposit.Response.Success success => Ok(success.NewBalance),
            Deposit.Response.Unauthorized unauthorized => Unauthorized(),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("withdraw")]
    public ActionResult<int> Withdraw(
        [FromHeader(Name = "X-User-Token")] Guid userToken,
        [FromBody] MoneyOperationRequest httpRequest)
    {
        var request = new Withdraw.Request(userToken, httpRequest.Amount);
        Withdraw.Response response = _accountOperationService.Withdraw(request);

        return response switch
        {
            Withdraw.Response.Success success => Ok(success.NewBalance),
            Withdraw.Response.Unauthorized unauthorized => Unauthorized(),
            Withdraw.Response.InsufficientFunds insufficient => BadRequest(insufficient.Message),
            _ => throw new UnreachableException(),
        };
    }
}
