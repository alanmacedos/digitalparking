public class PaymentService
{
    public decimal CalculateParkingFee(ParkingSession session)
    {
        if (session.ExitTime == null)
        {
            throw new InvalidOperationException("A saída do veículo ainda não foi registrada.");
        }

        TimeSpan elapsed = session.ExitTime.Value - session.EntryTime;
        int elapsedTime = (int)elapsed.TotalMinutes;

        decimal fee = getInitialFee(session.ContractedMinutes);
        int tolerance = 5;

        if (elapsedTime > session.ContractedMinutes + tolerance)
        {
            return fee += AdditionalFee(elapsedTime - session.ContractedMinutes);
        }

        return fee;
    }

    private decimal getInitialFee(int contractedMinutes)
    {
        switch (contractedMinutes)
        {
            case 30:
                return 6;

            case 60:
                return 10;

            case 120:
                return 18;

            case 180:
                return 30;

            default:
                throw new ArgumentException("O tempo contratado é inválido.");
        }
    }
    private decimal AdditionalFee(int additional)
    {
        decimal additionalFee = 0;

        while (additional > 0)
        {
            if (additional <= 30)
            {
                additionalFee += 9;
                additional -= 30;
            }

            else if (additional <= 60)
            {
                additionalFee += 12;
                additional -= 60;
            }

            else if (additional <= 120)
            {
                additionalFee += 21;
                additional -= 120;
            }

            else
            {
                additionalFee += 33;
                additional -= 180;
            }
        }

        return additionalFee;

    }

    public PaymentSummary CreateSummary(ParkingSession session, Payment payment)
    {
        if (session.ExitTime == null)
        {
            throw new InvalidOperationException("A saída do veículo ainda não foi registrada.");
        }

        TimeSpan contractedTime = TimeSpan.FromMinutes(session.ContractedMinutes);
        TimeSpan parkedTime = session.ExitTime.Value - session.EntryTime;
        TimeSpan additionalTime = parkedTime - contractedTime;

        if (additionalTime < TimeSpan.Zero)
        {
            additionalTime = TimeSpan.Zero;
        }

        return new PaymentSummary
        {
            ContractedTime = contractedTime,
            ParkedTime = parkedTime,
            AdditionalTime = additionalTime,

            Amount = payment.Amount,
            Method = payment.Method,
            PaidAt = payment.PaidAt,

            Plate = session.Vehicle.Plate,
            Owner = session.Vehicle.Owner,
            Model = session.Vehicle.Model,

            EntryTime = session.EntryTime,
            ExitTime = session.ExitTime
        };
    }

}