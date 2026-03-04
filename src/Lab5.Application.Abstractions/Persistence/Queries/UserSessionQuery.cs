using Lab5.Domain.Sessions;
using SourceKit.Generators.Builder.Annotations;

namespace Lab5.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record UserSessionQuery(SessionToken[] Tokens);