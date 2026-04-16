using Domain.Dtos;

namespace Domain.Contracts;

public interface IWacsService
{
    Task<CheckUserEligibilityResponse> CheckUserEligibilityAsync(CheckUserEligibilityRequest request, CancellationToken cancellationToken = default);
    Task<LoanProductResponse> CreateLoanProductAsync(CreateLoanProductRequest request, CancellationToken cancellationToken = default);
    Task<LoanProductResponse> GetLoanProductAsync(GetLoanProductRequest request, CancellationToken cancellationToken = default);
    Task<LoanProductResponse> InitiateLoanProductAsync(InitiateLoanProductRequest request, CancellationToken cancellationToken = default);
    Task<LoanProductResponse> AcceptLoanProductByLenderAsync(AcceptLoanProductRequest request, CancellationToken cancellationToken = default);
    Task<LoanProductResponse> RejectLoanProductByLenderAsync(RejectLoanProductRequest request, CancellationToken cancellationToken = default);
    Task<LoanResponse> InitiateLoanAsync(InitiateLoanRequest request, CancellationToken cancellationToken = default);
    Task<CustomerResponse> RegisterCustomerAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default);
    Task<CustomerResponse> VerifyCustomerAsync(VerifyCustomerRequest request, CancellationToken cancellationToken = default);
}
