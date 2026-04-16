using Domain.Contracts;
using Domain.Contracts.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Services;

public class WacsService : IWacsService
{
    private readonly IAsyncRepository<User> _userRepository;
    private readonly IAsyncRepository<Customer> _customerRepository;
    private readonly IAsyncRepository<LoanProduct> _loanProductRepository;
    private readonly IAsyncRepository<Loan> _loanRepository;

    public WacsService(
        IAsyncRepository<User> userRepository,
        IAsyncRepository<Customer> customerRepository,
        IAsyncRepository<LoanProduct> loanProductRepository,
        IAsyncRepository<Loan> loanRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _loanProductRepository = loanProductRepository;
        _loanRepository = loanRepository;
    }

    public async Task<CheckUserEligibilityResponse> CheckUserEligibilityAsync(CheckUserEligibilityRequest request, CancellationToken cancellationToken = default)
    {
        ValidateUserId(request.UserId);

        var user = await _userRepository.GetSingleAsync(x => x.Id == request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with id: {request.UserId} was not found.");
        }

        var customer = await _customerRepository.GetSingleAsync(x => x.UserId == request.UserId);
        var isEligible = customer?.VerificationStatus == CustomerVerificationStatus.Verified;

        return new CheckUserEligibilityResponse
        {
            UserId = request.UserId,
            IsEligible = isEligible,
            Message = isEligible ? "User is eligible." : "User is not eligible."
        };
    }

    public async Task<LoanProductResponse> CreateLoanProductAsync(CreateLoanProductRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanProductRequest(request);

        var loanProduct = LoanProduct.Create(request.Name.Trim(), request.PrincipalAmount, request.InterestRate, request.DurationInMonths, request.LenderId);
        await _loanProductRepository.AddAsync(loanProduct);
        await _loanProductRepository.SaveCnangesAsync();

        return ToLoanProductResponse(loanProduct);
    }

    public async Task<LoanProductResponse> GetLoanProductAsync(GetLoanProductRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanProductId(request.LoanProductId);

        var loanProduct = await GetLoanProductEntityAsync(request.LoanProductId);
        return ToLoanProductResponse(loanProduct);
    }

    public async Task<LoanProductResponse> InitiateLoanProductAsync(InitiateLoanProductRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanProductId(request.LoanProductId);

        var loanProduct = await GetLoanProductEntityAsync(request.LoanProductId);
        if (loanProduct.ApprovalStatus != LoanProductApprovalStatus.Pending)
        {
            throw new BadRequestException("Only pending loan products can be initiated.");
        }

        loanProduct.Initiate();
        await _loanProductRepository.UpdateAsync(loanProduct);
        await _loanProductRepository.SaveCnangesAsync();

        return ToLoanProductResponse(loanProduct);
    }

    public async Task<LoanProductResponse> AcceptLoanProductByLenderAsync(AcceptLoanProductRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanProductId(request.LoanProductId);

        var loanProduct = await GetLoanProductEntityAsync(request.LoanProductId);
        if (!loanProduct.IsInitiated)
        {
            throw new BadRequestException("Loan product must be initiated before lender approval.");
        }

        if (loanProduct.ApprovalStatus != LoanProductApprovalStatus.Pending)
        {
            throw new BadRequestException("Only pending loan products can be accepted.");
        }

        loanProduct.Accept();
        await _loanProductRepository.UpdateAsync(loanProduct);
        await _loanProductRepository.SaveCnangesAsync();

        return ToLoanProductResponse(loanProduct);
    }

    public async Task<LoanProductResponse> RejectLoanProductByLenderAsync(RejectLoanProductRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanProductId(request.LoanProductId);
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            throw new BadRequestException("Rejection reason is required.");
        }

        var loanProduct = await GetLoanProductEntityAsync(request.LoanProductId);
        if (!loanProduct.IsInitiated)
        {
            throw new BadRequestException("Loan product must be initiated before lender rejection.");
        }

        if (loanProduct.ApprovalStatus != LoanProductApprovalStatus.Pending)
        {
            throw new BadRequestException("Only pending loan products can be rejected.");
        }

        loanProduct.Reject(request.Reason.Trim());
        await _loanProductRepository.UpdateAsync(loanProduct);
        await _loanProductRepository.SaveCnangesAsync();

        return ToLoanProductResponse(loanProduct);
    }

    public async Task<LoanResponse> InitiateLoanAsync(InitiateLoanRequest request, CancellationToken cancellationToken = default)
    {
        ValidateLoanInitiationRequest(request);

        var customer = await _customerRepository.GetSingleAsync(x => x.Id == request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with id: {request.CustomerId} was not found.");
        }

        if (customer.VerificationStatus != CustomerVerificationStatus.Verified)
        {
            throw new BadRequestException("Customer must be verified before initiating a loan.");
        }

        var loanProduct = await GetLoanProductEntityAsync(request.LoanProductId);
        if (!loanProduct.IsInitiated || loanProduct.ApprovalStatus != LoanProductApprovalStatus.Accepted)
        {
            throw new BadRequestException("Loan product must be initiated and accepted before initiating a loan.");
        }

        if (request.Amount > loanProduct.PrincipalAmount)
        {
            throw new BadRequestException("Requested amount cannot exceed loan product principal amount.");
        }

        var loan = Loan.Create(request.LoanProductId, request.CustomerId, request.Amount);
        await _loanRepository.AddAsync(loan);
        await _loanRepository.SaveCnangesAsync();

        return new LoanResponse
        {
            LoanId = loan.Id,
            LoanProductId = loan.LoanProductId,
            CustomerId = loan.CustomerId,
            Amount = loan.Amount
        };
    }

    public async Task<CustomerResponse> RegisterCustomerAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRegisterCustomerRequest(request);

        var user = await _userRepository.GetSingleAsync(x => x.Id == request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with id: {request.UserId} was not found.");
        }

        var existingByUser = await _customerRepository.GetSingleAsync(x => x.UserId == request.UserId);
        if (existingByUser != null)
        {
            throw new AlreadyExistException($"Customer already exists for user with id: {request.UserId}.");
        }

        var existingByEmail = await _customerRepository.GetSingleAsync(x => x.Email == request.Email.Trim());
        if (existingByEmail != null)
        {
            throw new AlreadyExistException($"Customer with email: {request.Email} already exists.");
        }

        var customer = Customer.Register(request.UserId, request.FullName.Trim(), request.Email.Trim());
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveCnangesAsync();

        return ToCustomerResponse(customer);
    }

    public async Task<CustomerResponse> VerifyCustomerAsync(VerifyCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCustomerId(request.CustomerId);
        if (!request.IsVerified && string.IsNullOrWhiteSpace(request.RejectionReason))
        {
            throw new BadRequestException("Rejection reason is required when customer verification is rejected.");
        }

        var customer = await _customerRepository.GetSingleAsync(x => x.Id == request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with id: {request.CustomerId} was not found.");
        }

        customer.SetVerification(
            request.IsVerified ? CustomerVerificationStatus.Verified : CustomerVerificationStatus.Rejected,
            request.IsVerified ? null : request.RejectionReason!.Trim());

        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SaveCnangesAsync();

        return ToCustomerResponse(customer);
    }

    private async Task<LoanProduct> GetLoanProductEntityAsync(Guid loanProductId)
    {
        var loanProduct = await _loanProductRepository.GetSingleAsync(x => x.Id == loanProductId);
        if (loanProduct == null)
        {
            throw new NotFoundException($"Loan product with id: {loanProductId} was not found.");
        }

        return loanProduct;
    }

    private static void ValidateLoanProductRequest(CreateLoanProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BadRequestException("Loan product name is required.");
        }

        if (request.PrincipalAmount <= 0)
        {
            throw new BadRequestException("Principal amount must be greater than zero.");
        }

        if (request.InterestRate < 0)
        {
            throw new BadRequestException("Interest rate cannot be negative.");
        }

        if (request.DurationInMonths <= 0)
        {
            throw new BadRequestException("Duration in months must be greater than zero.");
        }

        if (request.LenderId == Guid.Empty)
        {
            throw new BadRequestException("Lender id is required.");
        }
    }

    private static void ValidateLoanInitiationRequest(InitiateLoanRequest request)
    {
        ValidateLoanProductId(request.LoanProductId);
        ValidateCustomerId(request.CustomerId);

        if (request.Amount <= 0)
        {
            throw new BadRequestException("Loan amount must be greater than zero.");
        }
    }

    private static void ValidateRegisterCustomerRequest(RegisterCustomerRequest request)
    {
        ValidateUserId(request.UserId);
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new BadRequestException("Customer full name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BadRequestException("Customer email is required.");
        }
    }

    private static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new BadRequestException("User id is required.");
        }
    }

    private static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new BadRequestException("Customer id is required.");
        }
    }

    private static void ValidateLoanProductId(Guid loanProductId)
    {
        if (loanProductId == Guid.Empty)
        {
            throw new BadRequestException("Loan product id is required.");
        }
    }

    private static LoanProductResponse ToLoanProductResponse(LoanProduct loanProduct)
    {
        return new LoanProductResponse
        {
            LoanProductId = loanProduct.Id,
            Name = loanProduct.Name,
            PrincipalAmount = loanProduct.PrincipalAmount,
            InterestRate = loanProduct.InterestRate,
            DurationInMonths = loanProduct.DurationInMonths,
            IsInitiated = loanProduct.IsInitiated,
            ApprovalStatus = loanProduct.ApprovalStatus,
            RejectionReason = loanProduct.RejectionReason
        };
    }

    private static CustomerResponse ToCustomerResponse(Customer customer)
    {
        return new CustomerResponse
        {
            CustomerId = customer.Id,
            UserId = customer.UserId,
            FullName = customer.FullName,
            Email = customer.Email,
            VerificationStatus = customer.VerificationStatus,
            VerificationNote = customer.VerificationNote
        };
    }
}
