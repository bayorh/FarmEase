using Domain.Contracts.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Services;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace UnitTests.Services;

public class WacsServiceTests
{
    private readonly Mock<IAsyncRepository<User>> _userRepository = new();
    private readonly Mock<IAsyncRepository<Customer>> _customerRepository = new();
    private readonly Mock<IAsyncRepository<LoanProduct>> _loanProductRepository = new();
    private readonly Mock<IAsyncRepository<Loan>> _loanRepository = new();

    private WacsService BuildService()
    {
        return new WacsService(
            _userRepository.Object,
            _customerRepository.Object,
            _loanProductRepository.Object,
            _loanRepository.Object);
    }

    [Fact]
    public async Task CheckUserEligibilityAsync_ReturnsEligible_WhenCustomerIsVerified()
    {
        var user = User.Create("john", "john@example.com", "Password1$", Mock.Of<Domain.Contracts.IPasswordHasher>());
        var customer = Customer.Register(user.Id, "John Doe", "john@example.com");
        customer.SetVerification(CustomerVerificationStatus.Verified, null);

        _userRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), false, It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);
        _customerRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<Customer, bool>>>(), false, It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(customer);

        var sut = BuildService();
        var result = await sut.CheckUserEligibilityAsync(new CheckUserEligibilityRequest { UserId = user.Id });

        Assert.True(result.IsEligible);
    }

    [Fact]
    public async Task CreateLoanProductAsync_CreatesPendingLoanProduct()
    {
        LoanProduct? added = null;
        _loanProductRepository.Setup(x => x.AddAsync(It.IsAny<LoanProduct>()))
            .Callback<LoanProduct>(p => added = p)
            .Returns(Task.CompletedTask);
        _loanProductRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.CreateLoanProductAsync(new CreateLoanProductRequest
        {
            Name = "Seasonal",
            PrincipalAmount = 10000m,
            InterestRate = 5m,
            DurationInMonths = 12,
            LenderId = Guid.NewGuid()
        });

        Assert.NotNull(added);
        Assert.Equal(LoanProductApprovalStatus.Pending, result.ApprovalStatus);
    }

    [Fact]
    public async Task GetLoanProductAsync_ThrowsNotFound_WhenMissing()
    {
        _loanProductRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<LoanProduct, bool>>>(), false, It.IsAny<Expression<Func<LoanProduct, object>>[]>()))
            .ReturnsAsync((LoanProduct)null!);

        var sut = BuildService();

        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetLoanProductAsync(new GetLoanProductRequest { LoanProductId = Guid.NewGuid() }));
    }

    [Fact]
    public async Task InitiateLoanProductAsync_SetsInitiated()
    {
        var product = LoanProduct.Create("Product", 5000, 3, 6, Guid.NewGuid());
        _loanProductRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<LoanProduct, bool>>>(), false, It.IsAny<Expression<Func<LoanProduct, object>>[]>()))
            .ReturnsAsync(product);
        _loanProductRepository.Setup(x => x.UpdateAsync(It.IsAny<LoanProduct>())).Returns(Task.CompletedTask);
        _loanProductRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.InitiateLoanProductAsync(new InitiateLoanProductRequest { LoanProductId = product.Id });

        Assert.True(result.IsInitiated);
    }

    [Fact]
    public async Task AcceptLoanProductByLenderAsync_AcceptsPendingInitiatedProduct()
    {
        var product = LoanProduct.Create("Product", 5000, 3, 6, Guid.NewGuid());
        product.Initiate();
        _loanProductRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<LoanProduct, bool>>>(), false, It.IsAny<Expression<Func<LoanProduct, object>>[]>()))
            .ReturnsAsync(product);
        _loanProductRepository.Setup(x => x.UpdateAsync(It.IsAny<LoanProduct>())).Returns(Task.CompletedTask);
        _loanProductRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.AcceptLoanProductByLenderAsync(new AcceptLoanProductRequest { LoanProductId = product.Id });

        Assert.Equal(LoanProductApprovalStatus.Accepted, result.ApprovalStatus);
    }

    [Fact]
    public async Task RejectLoanProductByLenderAsync_RejectsPendingInitiatedProduct()
    {
        var product = LoanProduct.Create("Product", 5000, 3, 6, Guid.NewGuid());
        product.Initiate();
        _loanProductRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<LoanProduct, bool>>>(), false, It.IsAny<Expression<Func<LoanProduct, object>>[]>()))
            .ReturnsAsync(product);
        _loanProductRepository.Setup(x => x.UpdateAsync(It.IsAny<LoanProduct>())).Returns(Task.CompletedTask);
        _loanProductRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.RejectLoanProductByLenderAsync(new RejectLoanProductRequest { LoanProductId = product.Id, Reason = "Policy mismatch" });

        Assert.Equal(LoanProductApprovalStatus.Rejected, result.ApprovalStatus);
        Assert.Equal("Policy mismatch", result.RejectionReason);
    }

    [Fact]
    public async Task InitiateLoanAsync_CreatesLoan_WhenCustomerVerifiedAndProductAccepted()
    {
        var customer = Customer.Register(Guid.NewGuid(), "John Doe", "john@example.com");
        customer.SetVerification(CustomerVerificationStatus.Verified, null);
        var product = LoanProduct.Create("Product", 5000, 3, 6, Guid.NewGuid());
        product.Initiate();
        product.Accept();

        _customerRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<Customer, bool>>>(), false, It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(customer);
        _loanProductRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<LoanProduct, bool>>>(), false, It.IsAny<Expression<Func<LoanProduct, object>>[]>()))
            .ReturnsAsync(product);
        _loanRepository.Setup(x => x.AddAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);
        _loanRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.InitiateLoanAsync(new InitiateLoanRequest
        {
            CustomerId = customer.Id,
            LoanProductId = product.Id,
            Amount = 1000
        });

        Assert.Equal(1000, result.Amount);
    }

    [Fact]
    public async Task RegisterCustomerAsync_CreatesPendingCustomer_WhenInputValid()
    {
        var user = User.Create("john", "john@example.com", "Password1$", Mock.Of<Domain.Contracts.IPasswordHasher>());
        _userRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), false, It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);
        _customerRepository.SetupSequence(x => x.GetSingleAsync(It.IsAny<Expression<Func<Customer, bool>>>(), false, It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync((Customer)null!)
            .ReturnsAsync((Customer)null!);
        _customerRepository.Setup(x => x.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _customerRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.RegisterCustomerAsync(new RegisterCustomerRequest
        {
            UserId = user.Id,
            FullName = "John Doe",
            Email = "john@example.com"
        });

        Assert.Equal(CustomerVerificationStatus.Pending, result.VerificationStatus);
    }

    [Fact]
    public async Task VerifyCustomerAsync_RejectsWithReason_WhenNotVerified()
    {
        var customer = Customer.Register(Guid.NewGuid(), "John Doe", "john@example.com");
        _customerRepository.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<Customer, bool>>>(), false, It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(customer);
        _customerRepository.Setup(x => x.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _customerRepository.Setup(x => x.SaveCnangesAsync()).Returns(Task.CompletedTask);

        var sut = BuildService();
        var result = await sut.VerifyCustomerAsync(new VerifyCustomerRequest
        {
            CustomerId = customer.Id,
            IsVerified = false,
            RejectionReason = "KYC mismatch"
        });

        Assert.Equal(CustomerVerificationStatus.Rejected, result.VerificationStatus);
        Assert.Equal("KYC mismatch", result.VerificationNote);
    }
}
