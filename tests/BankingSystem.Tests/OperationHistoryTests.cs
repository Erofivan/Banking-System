using Lab5.Application;
using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Application.Contracts.History;
using Lab5.Application.Contracts.History.Operations;
using Lab5.Domain.Accounts;
using Lab5.Domain.Operations;
using Lab5.Domain.Sessions;
using Lab5.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public sealed class OperationHistoryTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IOperationRecordRepository _operationRecordRepository;
    private readonly IOperationHistoryService _historyService;

    public OperationHistoryTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _userSessionRepository = Substitute.For<IUserSessionRepository>();
        _operationRecordRepository = Substitute.For<IOperationRecordRepository>();
        IAdminSessionRepository adminSessionRepository = Substitute.For<IAdminSessionRepository>();

        IPersistenceContext persistenceContext = Substitute.For<IPersistenceContext>();
        persistenceContext.Accounts.Returns(_accountRepository);
        persistenceContext.UserSessions.Returns(_userSessionRepository);
        persistenceContext.AdminSessions.Returns(adminSessionRepository);
        persistenceContext.OperationRecords.Returns(_operationRecordRepository);

        var services = new ServiceCollection();

        services.AddSingleton(persistenceContext);

        services.AddSingleton(
            Options.Create(new AtmOptions { SystemPassword = "test" }));

        services.AddApplication();

        ServiceProvider provider = services.BuildServiceProvider();
        _historyService = provider.GetRequiredService<IOperationHistoryService>();
    }

    [Fact]
    public void GetHistory_WithValidSession_ShouldReturnRecords()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(500));
        var token = new SessionToken(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        var session = new UserSession(token, account.Id);

        OperationRecord[] records =
        [
            new OperationRecord(new OperationRecordId(1), account.Id, OperationType.Deposit, new Amount(200)),
            new OperationRecord(new OperationRecordId(2), account.Id, OperationType.Withdrawal, new Amount(50))
        ];

        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([session]);

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        _operationRecordRepository
            .Query(Arg.Any<OperationRecordQuery>())
            .Returns(records);

        // Act
        var request = new GetHistory.Request(token.Value);
        GetHistory.Response response = _historyService.GetHistory(request);

        // Assert
        Assert.IsType<GetHistory.Response.Success>(response);

        var success = (GetHistory.Response.Success)response;
        Assert.Equal(2, success.Records.Count);
    }

    [Fact]
    public void GetHistory_WithInvalidToken_ShouldReturnUnauthorized()
    {
        // Arrange
        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([]);

        // Act
        var request = new GetHistory.Request(Guid.NewGuid());
        GetHistory.Response response = _historyService.GetHistory(request);

        // Assert
        Assert.IsType<GetHistory.Response.Unauthorized>(response);
    }

    [Fact]
    public void GetHistory_WithNoOperations_ShouldReturnEmptyList()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(0));
        var token = new SessionToken(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));
        var session = new UserSession(token, account.Id);

        _userSessionRepository
            .Query(Arg.Any<UserSessionQuery>())
            .Returns([session]);

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        _operationRecordRepository
            .Query(Arg.Any<OperationRecordQuery>())
            .Returns([]);

        // Act
        var request = new GetHistory.Request(token.Value);
        GetHistory.Response response = _historyService.GetHistory(request);

        // Assert
        Assert.IsType<GetHistory.Response.Success>(response);

        var success = (GetHistory.Response.Success)response;
        Assert.Empty(success.Records);
    }
}
