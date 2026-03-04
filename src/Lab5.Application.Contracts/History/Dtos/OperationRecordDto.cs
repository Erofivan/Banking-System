namespace Lab5.Application.Contracts.History.Dtos;

public record OperationRecordDto(long Id, long AccountId, string Type, int Amount);