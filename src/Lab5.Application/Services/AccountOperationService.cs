using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Contracts.Accounts;
using Lab5.Application.Contracts.Accounts.Operations;
using Lab5.Application.Extensions;
using Lab5.Application.Mapping;
using Lab5.Domain.Accounts;
using Lab5.Domain.Accounts.Results;
using Lab5.Domain.Operations;
using Lab5.Domain.Sessions;
using Lab5.Domain.ValueObjects;
using System.Diagnostics;

namespace Lab5.Application.Services;

internal sealed class AccountOperationService : IAccountOperationService
{
    private readonly IPersistenceContext _persistenceContext;

    public AccountOperationService(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public CreateAccount.Response Create(CreateAccount.Request request)
    {
        AdminSession? admin = _persistenceContext.FindAdminSession(new SessionToken(request.AdminToken));

        if (admin is null)
            return new CreateAccount.Response.Unauthorized("Invalid admin session");

        var account = new Account(
            AccountId.Default,
            new PinCode(request.PinCode),
            new Amount(0));

        account = _persistenceContext.Accounts.Add(account);

        return new CreateAccount.Response.Success(account.ToDto());
    }

    public GetBalance.Response GetBalance(GetBalance.Request request)
    {
        var token = new SessionToken(request.UserToken);
        UserSession? session = _persistenceContext.FindUserSession(token);

        if (session is null)
            return new GetBalance.Response.Unauthorized("Invalid user session");

        Account? account = _persistenceContext.FindAccount(session.AccountId);

        if (account is null)
            return new GetBalance.Response.Unauthorized("Account not found");

        return new GetBalance.Response.Success(account.Balance.Value);
    }

    public Deposit.Response Deposit(Deposit.Request request)
    {
        var token = new SessionToken(request.UserToken);
        UserSession? session = _persistenceContext.FindUserSession(token);

        if (session is null)
            return new Deposit.Response.Unauthorized("Invalid user session");

        Account? account = _persistenceContext.FindAccount(session.AccountId);

        if (account is null)
            return new Deposit.Response.Unauthorized("Account not found");

        var depositAmount = new Amount(request.Amount);

        Amount newBalance = account.Deposit(depositAmount);

        _persistenceContext.OperationRecords.Add(new OperationRecord(
            OperationRecordId.Default,
            account.Id,
            OperationType.Deposit,
            depositAmount));

        return new Deposit.Response.Success(newBalance.Value);
    }

    public Withdraw.Response Withdraw(Withdraw.Request request)
    {
        var token = new SessionToken(request.UserToken);
        UserSession? session = _persistenceContext.FindUserSession(token);

        if (session is null)
            return new Withdraw.Response.Unauthorized("Invalid user session");

        Account? account = _persistenceContext.FindAccount(session.AccountId);

        if (account is null)
            return new Withdraw.Response.Unauthorized("Account not found");

        var withdrawalAmount = new Amount(request.Amount);

        WithdrawalResult result = account.Withdraw(withdrawalAmount);

        switch (result)
        {
            case WithdrawalResult.Success success:
                _persistenceContext.OperationRecords.Add(new OperationRecord(
                    OperationRecordId.Default,
                    account.Id,
                    OperationType.Withdrawal,
                    withdrawalAmount));

                return new Withdraw.Response.Success(success.NewBalance.Value);

            case WithdrawalResult.InsufficientFunds insufficient:
                return new Withdraw.Response.InsufficientFunds(insufficient.Message);

            default:
                throw new UnreachableException();
        }
    }
}