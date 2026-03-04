using Lab5.Domain.Accounts;
using SourceKit.Generators.Builder.Annotations;

namespace Lab5.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record OperationRecordQuery(AccountId[] AccountIds);