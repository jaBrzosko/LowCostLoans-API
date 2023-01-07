using Contracts.Frontend.Offers;
using Services.Endpoints.Offers;
using Services.UnitTests.Helpers;
using Xunit;


namespace Services.UnitTests.Endpoints.Offers;

public class PostCreateOfferTemplateValidatorTest
{
    private readonly PostCreateOfferTemplateValidator validator = new();

    private readonly PostCreateOfferTemplate validRequest = new()
    {
        MinimumMoneyInSmallestUnit = 100_000_00,
        MaximumMoneyInSmallestUnit = 200_000_00,
        MinimumNumberOfInstallments = 48,
        MaximumNumberOfInstallments = 64,
        InteresetRate = 250
    };

    [Fact]
    public async Task Request_is_valid()
    {
        var request = validRequest;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }

    [Fact]
    public async Task MinimumMoney_is_on_limit()
    {
        var request = validRequest;
        request.MinimumMoneyInSmallestUnit = 1;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }
    
    [Fact]
    public async Task MinimumMoney_is_zero()
    {
        var request = validRequest;
        request.MinimumMoneyInSmallestUnit = 0;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.MoneyHasToBePositive);
    }
    
    [Fact]
    public async Task MinimumMoney_is_negative()
    {
        var request = validRequest;
        request.MinimumMoneyInSmallestUnit = -1;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.MoneyHasToBePositive);
    }
    
    [Fact]
    public async Task MinimumMoney_is_larger_than_MaximumMoney()
    {
        var request = validRequest;
        request.MinimumMoneyInSmallestUnit = 200_000_00;
        request.MaximumMoneyInSmallestUnit = 100_000_00;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.MaximumMoneyHasToBeSmallerThanMinimumMoney);
    }
    
    [Fact]
    public async Task MinimumInstallments_is_on_limit()
    {
        var request = validRequest;
        request.MinimumNumberOfInstallments = 1;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }
    
    [Fact]
    public async Task MinimumInstallments_is_zero()
    {
        var request = validRequest;
        request.MinimumNumberOfInstallments = 0;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.NumberOfInstallmentsHasToBePositive);
    }
    
    [Fact]
    public async Task MinimumInstallments_is_negative()
    {
        var request = validRequest;
        request.MinimumNumberOfInstallments = -1;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.NumberOfInstallmentsHasToBePositive);
    }
    
    [Fact]
    public async Task MinimumInstallments_is_larger_than_MaximumInstallments()
    {
        var request = validRequest;
        request.MinimumNumberOfInstallments = 64;
        request.MaximumNumberOfInstallments = 48;
        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateOfferTemplate.ErrorCodes.MaximumInstallmentsHasToBeSmallerThanMinimumInstallments);
    }

}