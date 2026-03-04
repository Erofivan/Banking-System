using Lab5.Application;
using Lab5.Application.Abstractions.Persistence;
using Lab5.Application.Abstractions.Persistence.Queries;
using Lab5.Application.Abstractions.Persistence.Repositories;
using Lab5.Application.Contracts.Sessions;
using Lab5.Application.Contracts.Sessions.Operations;
using Lab5.Domain.Accounts;
using Lab5.Domain.Sessions;
using Lab5.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public sealed class SessionServiceTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IAdminSessionRepository _adminSessionRepository;
    private readonly ISessionService _sessionService;

    public SessionServiceTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _userSessionRepository = Substitute.For<IUserSessionRepository>();
        _adminSessionRepository = Substitute.For<IAdminSessionRepository>();
        IOperationRecordRepository operationRecordRepository = Substitute.For<IOperationRecordRepository>();

        IPersistenceContext persistenceContext = Substitute.For<IPersistenceContext>();
        persistenceContext.Accounts.Returns(_accountRepository);
        persistenceContext.UserSessions.Returns(_userSessionRepository);
        persistenceContext.AdminSessions.Returns(_adminSessionRepository);
        persistenceContext.OperationRecords.Returns(operationRecordRepository);

        var services = new ServiceCollection();

        services.AddSingleton(persistenceContext);

        services.AddSingleton(
            Options.Create(new AtmOptions { SystemPassword = "secret" }));

        services.AddApplication();

        ServiceProvider provider = services.BuildServiceProvider();
        _sessionService = provider.GetRequiredService<ISessionService>();
    }

    [Fact]
    public void LoginUser_WithCorrectPin_ShouldSucceed()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(0));

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        // Act
        var request = new LoginUser.Request(1, "1234");
        LoginUser.Response response = _sessionService.LoginUser(request);

        // Assert
        Assert.IsType<LoginUser.Response.Success>(response);

        _userSessionRepository.Received(1).Add(Arg.Any<UserSession>());
    }

    [Fact]
    public void LoginUser_WithWrongPin_ShouldReturnInvalidCredentials()
    {
        // Arrange
        var account = new Account(new AccountId(1), new PinCode("1234"), new Amount(0));

        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns([account]);

        // Act
        var request = new LoginUser.Request(1, "9999");
        LoginUser.Response response = _sessionService.LoginUser(request);

        // Assert
        Assert.IsType<LoginUser.Response.InvalidCredentials>(response);

        _userSessionRepository.DidNotReceive().Add(Arg.Any<UserSession>());
    }

    [Fact]
    public void LoginUser_WithNonExistentAccount_ShouldReturnInvalidCredentials()
    {
        // Arrange
        _accountRepository
            .Query(Arg.Any<AccountQuery>())
            .Returns(Array.Empty<Account>());

        // Act
        var request = new LoginUser.Request(999, "1234");
        LoginUser.Response response = _sessionService.LoginUser(request);

        // Assert
        Assert.IsType<LoginUser.Response.InvalidCredentials>(response);
    }

    [Fact]
    public void LoginAdmin_WithCorrectPassword_ShouldSucceed()
    {
        // Act
        var request = new LoginAdmin.Request("secret");
        LoginAdmin.Response response = _sessionService.LoginAdmin(request);

        // Assert
        Assert.IsType<LoginAdmin.Response.Success>(response);

        _adminSessionRepository.Received(1).Add(Arg.Any<AdminSession>());
    }

    [Fact]
    public void LoginAdmin_WithWrongPassword_ShouldReturnInvalidPassword()
    {
        // Act
        var request = new LoginAdmin.Request("wrong-password");
        LoginAdmin.Response response = _sessionService.LoginAdmin(request);

        // Assert
        Assert.IsType<LoginAdmin.Response.InvalidPassword>(response);

        _adminSessionRepository.DidNotReceive().Add(Arg.Any<AdminSession>());
    }
}
