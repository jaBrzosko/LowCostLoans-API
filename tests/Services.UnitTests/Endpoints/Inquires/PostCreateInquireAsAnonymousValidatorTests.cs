using Contracts.Inquires;
using Contracts.Users;
using Services.Endpoints.Inquires;
using Services.UnitTests.Helpers;
using Xunit;

namespace Services.UnitTests.Endpoints.Inquires;

public class PostCreateInquireAsAnonymousValidatorTests
{
    private readonly PostCreateInquireAsAnonymousValidator validator = new();

    private readonly PostCreateInquireAsAnonymous validRequest = new()
    {
        MoneyInSmallestUnit = 10546465,
        NumberOfInstallments = 12,
        PersonalData = new()
        {
            FirstName = "first name",
            LastName = "last name",
            GovernmentId = "55030101230",
            GovernmentIdType = GovernmentIdTypeDto.Pesel,
            JobType = JobTypeDto.SomeJobType,
        },
    };

    [Fact]
    public async Task Request_is_valid()
    {
        var request = validRequest;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }
    
    [Fact]
    public async Task Money_is_on_limit()
    {
        var request = validRequest;
        request.MoneyInSmallestUnit = 1;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }
    
    [Fact]
    public async Task NumberOfInstallments_is_on_limit()
    {
        var request = validRequest;
        request.NumberOfInstallments = 1;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectResult();
    }
    
    [Fact]
    public async Task Money_is_zero()
    {
        var request = validRequest;
        request.MoneyInSmallestUnit = 0;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateInquireAsAnonymous.ErrorCodes.MoneyHasToBePositive);
    }
    
    [Fact]
    public async Task Money_is_negative()
    {
        var request = validRequest;
        request.MoneyInSmallestUnit = -1236789;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateInquireAsAnonymous.ErrorCodes.MoneyHasToBePositive);
    }
    
    [Fact]
    public async Task NumberOfInstallments_is_zero()
    {
        var request = validRequest;
        request.NumberOfInstallments = 0;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateInquireAsAnonymous.ErrorCodes.NumberOfInstallmentsHasToBePositive);
    }
    
    [Fact]
    public async Task NumberOfInstallments_is_negative()
    {
        var request = validRequest;
        request.NumberOfInstallments = -123;

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateInquireAsAnonymous.ErrorCodes.NumberOfInstallmentsHasToBePositive);
    }
    
    [Fact]
    public async Task FirstName_of_PersonalData_is_empty()
    {
        var request = validRequest;
        request.PersonalData.FirstName = "";

        var validationResult = await validator.ValidateAsync(request);
        
        validationResult.EnsureCorrectError(PostCreateInquireAsAnonymous.ErrorCodes.PersonalDataErrors.FirstNameIsEmpty);
    }
}
