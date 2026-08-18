public class PaymentSummary
{
    public TimeSpan ContractedTime { get; init; }
    public TimeSpan ParkedTime { get; init; }
    public TimeSpan AdditionalTime { get; init; }

    public decimal Amount { get; init; }
    public required string Method { get; init; }
    public DateTime PaidAt { get; init; }

    public required string Plate { get; init; }
    public required string Owner { get; init; }
    public required string Model { get; init; }

    public DateTime EntryTime { get; init; }
    public DateTime? ExitTime { get; init; }
}