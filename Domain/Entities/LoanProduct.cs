using Domain.Enums;

namespace Domain.Entities;

public class LoanProduct : BaseEntity
{
    public string Name { get; private set; }
    public decimal PrincipalAmount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int DurationInMonths { get; private set; }
    public Guid LenderId { get; private set; }
    public bool IsInitiated { get; private set; }
    public LoanProductApprovalStatus ApprovalStatus { get; private set; }
    public string? RejectionReason { get; private set; }

    public LoanProduct()
    {
    }

    public static LoanProduct Create(string name, decimal principalAmount, decimal interestRate, int durationInMonths, Guid lenderId)
    {
        return new LoanProduct
        {
            Name = name,
            PrincipalAmount = principalAmount,
            InterestRate = interestRate,
            DurationInMonths = durationInMonths,
            LenderId = lenderId,
            ApprovalStatus = LoanProductApprovalStatus.Pending
        };
    }

    public void Initiate()
    {
        IsInitiated = true;
    }

    public void Accept()
    {
        ApprovalStatus = LoanProductApprovalStatus.Accepted;
        RejectionReason = null;
    }

    public void Reject(string reason)
    {
        ApprovalStatus = LoanProductApprovalStatus.Rejected;
        RejectionReason = reason;
    }
}
