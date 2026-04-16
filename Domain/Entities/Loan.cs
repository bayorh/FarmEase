namespace Domain.Entities;

public class Loan : BaseEntity
{
    public Guid LoanProductId { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Amount { get; private set; }

    public Loan()
    {
    }

    public static Loan Create(Guid loanProductId, Guid customerId, decimal amount)
    {
        return new Loan
        {
            LoanProductId = loanProductId,
            CustomerId = customerId,
            Amount = amount
        };
    }
}
