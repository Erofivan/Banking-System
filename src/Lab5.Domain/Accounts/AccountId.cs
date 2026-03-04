namespace Lab5.Domain.Accounts;

public readonly record struct AccountId(long Value)
{
    public static readonly AccountId Default = new(0);
}
