using Domain.Enums;

namespace Domain.Dtos;

public class CheckUserEligibilityRequest
{
    public Guid UserId { get; set; }
}

public class CheckUserEligibilityResponse
{
    public Guid UserId { get; set; }
    public bool IsEligible { get; set; }
    public string Message { get; set; }
}

public class CreateLoanProductRequest
{
    public string Name { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }
    public Guid LenderId { get; set; }
}

public class GetLoanProductRequest
{
    public Guid LoanProductId { get; set; }
}

public class InitiateLoanProductRequest
{
    public Guid LoanProductId { get; set; }
}

public class AcceptLoanProductRequest
{
    public Guid LoanProductId { get; set; }
}

public class RejectLoanProductRequest
{
    public Guid LoanProductId { get; set; }
    public string Reason { get; set; }
}

public class LoanProductResponse
{
    public Guid LoanProductId { get; set; }
    public string Name { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }
    public bool IsInitiated { get; set; }
    public LoanProductApprovalStatus ApprovalStatus { get; set; }
    public string? RejectionReason { get; set; }
}

public class InitiateLoanRequest
{
    public Guid LoanProductId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}

public class LoanResponse
{
    public Guid LoanId { get; set; }
    public Guid LoanProductId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}

public class RegisterCustomerRequest
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}

public class VerifyCustomerRequest
{
    public Guid CustomerId { get; set; }
    public bool IsVerified { get; set; }
    public string? RejectionReason { get; set; }
}

public class CustomerResponse
{
    public Guid CustomerId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public CustomerVerificationStatus VerificationStatus { get; set; }
    public string? VerificationNote { get; set; }
}
