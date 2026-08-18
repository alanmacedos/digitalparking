public class Payment
{
    public decimal Amount { get; set; }
    public string Method { get; set; }
    public DateTime PaidAt { get; set; }

    public Payment (decimal amount, string method, DateTime paidAt)
    {
        Amount = amount;
        Method = method;
        PaidAt = paidAt;
    }
}