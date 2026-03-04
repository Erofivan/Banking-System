using Lab5.Application.Contracts.Accounts;
using Lab5.Application.Contracts.History;
using Lab5.Application.Contracts.Sessions;
using Lab5.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IAccountOperationService, AccountOperationService>();
        collection.AddScoped<ISessionService, SessionService>();
        collection.AddScoped<IOperationHistoryService, OperationHistoryService>();

        return collection;
    }
}
