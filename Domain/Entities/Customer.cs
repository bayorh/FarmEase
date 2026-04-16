using Domain.Enums;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    public Guid UserId { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public CustomerVerificationStatus VerificationStatus { get; private set; }
    public string? VerificationNote { get; private set; }

    public Customer()
    {
    }

    public static Customer Register(Guid userId, string fullName, string email)
    {
        return new Customer
        {
            UserId = userId,
            FullName = fullName,
            Email = email,
            VerificationStatus = CustomerVerificationStatus.Pending
        };
    }

    public void SetVerification(CustomerVerificationStatus verificationStatus, string? note)
    {
        VerificationStatus = verificationStatus;
        VerificationNote = note;
    }
}
