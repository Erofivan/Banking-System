using Lab5.Application;
using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Application.Contracts.Accounts;
using Lab5.Application.Contracts.Accounts.Operations;
using Lab5.Domain.Accounts;
using Lab5.Domain.Operations;
using Lab5.Domain.Sessions;
using Lab5.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public sealed class AccountOperationTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IAdminSessionRepository _adminSessionRepository;
    private readonly IOperationRecordRepository _operationRecordRepository;
    private readonly IAccountOperationService _accountOperationService;

    public AccountOperationTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _operationRecordRepository = Substitute.For<IOperationRecordRepository>();
        _userSessionRepository = Substitute.For<IUserSessionRepository>();
        _adminSessionRepository = Substitute.For<IAdminSessionRepository>();

        IPersistenceContext persistenceContext = Substitute.For<IPersistenceContext>();
        persistenceContext.Accounts.Returns(_accountRepository);
        persistenceContext.UserSessions.Returns(_userSessionRepository);
        persistenceContext.AdminSessions.Returns(_adminSessionRepository);
        persistenceContext.OperationRecords.Returns(_operationRecordRepository);

        var services = new ServiceCollection();

        services.AddSingleton(persistenceContext);

        services.AddSingleton(
            Options.Create(new AtmOptions { SystemPassword = "test-pass" }));

        services.AddApplication();

        ServiceProvider provider = services.BuildServiceProvider();
        _accountOperationService = provider.GetRequiredService<IAccountOperationService>();
    }

    [Fact]
    public void Deposit_ShouldIncrementBalance()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(100));
        var token = new SessionToken(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        var session = new UserSession(token, account.Id);

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([session]);

        _operationRecordRepository
            .Add(Arg.Any<OperationRecord>())
            .Returns(callInfo => callInfo.Arg<OperationRecord>());

        // Act
        var request = new Deposit.Request(token.Value, 50);
        Deposit.Response response = _accountOperationService.Deposit(request);

        // Assert
        Assert.IsType<Deposit.Response.Success>(response);

        var success = (Deposit.Response.Success)response;
        Assert.Equal(150, success.NewBalance);

        _operationRecordRepository.Received(1).Add(
            Arg.Is<OperationRecord>(r => r.Type == OperationType.Deposit && r.Amount.Value == 50));
    }

    [Fact]
    public void Withdraw_WithSufficientFunds_ShouldSucceed()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(200));
        var token = new SessionToken(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        var session = new UserSession(token, account.Id);

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([session]);

        _operationRecordRepository
            .Add(Arg.Any<OperationRecord>())
            .Returns(callInfo => callInfo.Arg<OperationRecord>());

        // Act
        var request = new Withdraw.Request(token.Value, 50);
        Withdraw.Response response = _accountOperationService.Withdraw(request);

        // Assert
        Assert.IsType<Withdraw.Response.Success>(response);

        var success = (Withdraw.Response.Success)response;
        Assert.Equal(150, success.NewBalance);

        _operationRecordRepository.Received(1).Add(
            Arg.Is<OperationRecord>(r => r.Type == OperationType.Withdrawal && r.Amount.Value == 50));
    }

    [Fact]
    public void Withdraw_WithInsufficientFunds_ShouldFail()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(30));
        var token = new SessionToken(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        var session = new UserSession(token, account.Id);

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([session]);

        // Act
        var request = new Withdraw.Request(token.Value, 100);
        Withdraw.Response response = _accountOperationService.Withdraw(request);

        // Assert
        Assert.IsType<Withdraw.Response.InsufficientFunds>(response);

        _operationRecordRepository.DidNotReceive().Add(Arg.Any<OperationRecord>());
    }

    [Fact]
    public void CreateAccount_WithValidAdminSession_ShouldSucceed()
    {
        // Arrange
        var adminToken = new SessionToken(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        var adminSession = new AdminSession(adminToken);

        _adminSessionRepository
            .Query(Arg.Any<AdminSessionQuery>())
            .Returns([adminSession]);

        _accountRepository
            .Add(Arg.Any<Account>())
            .Returns(callInfo =>
            {
                Account input = callInfo.Arg<Account>();
                return new Account(new AccountId(1), input.PinCode, input.Balance);
            });

        // Act
        var request = new CreateAccount.Request(adminToken.Value, "5678");
        CreateAccount.Response response = _accountOperationService.Create(request);

        // Assert
        Assert.IsType<CreateAccount.Response.Success>(response);

        var success = (CreateAccount.Response.Success)response;
        Assert.Equal(1, success.Account.Id);
        Assert.Equal(0, success.Account.Balance);
    }

    [Fact]
    public void CreateAccount_WithInvalidAdminSession_ShouldReturnUnauthorized()
    {
        // Arrange
        _adminSessionRepository
            .Query(Arg.Any<AdminSessionQuery>())
            .Returns([]);

        // Act
        var request = new CreateAccount.Request(Guid.NewGuid(), "1234");
        CreateAccount.Response response = _accountOperationService.Create(request);

        // Assert
        Assert.IsType<CreateAccount.Response.Unauthorized>(response);
    }

    [Fact]
    public void Deposit_WithInvalidToken_ShouldReturnUnauthorized()
    {
        // Arrange
        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([]);

        // Act
        var request = new Deposit.Request(Guid.NewGuid(), 100);
        Deposit.Response response = _accountOperationService.Deposit(request);

        // Assert
        Assert.IsType<Deposit.Response.Unauthorized>(response);

        _operationRecordRepository.DidNotReceive().Add(Arg.Any<OperationRecord>());
    }

    [Fact]
    public void Withdraw_WithInvalidToken_ShouldReturnUnauthorized()
    {
        // Arrange
        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([]);

        // Act
        var request = new Withdraw.Request(Guid.NewGuid(), 50);
        Withdraw.Response response = _accountOperationService.Withdraw(request);

        // Assert
        Assert.IsType<Withdraw.Response.Unauthorized>(response);

        _operationRecordRepository.DidNotReceive().Add(Arg.Any<OperationRecord>());
    }
}
