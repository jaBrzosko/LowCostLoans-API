using Contracts.Users;
using Services.Data;
using Services.Endpoints.Users;
using Services.UnitTests.Helpers;
using Xunit;

namespace Services.UnitTests.Endpoints.Users;

public class PersonalDataValidatorTests
{
    private readonly PersonalDataValidator validator = new();
    private readonly PersonalDataDto validPersonalData = new()
    {
        FirstName = "first name",
        LastName = "last name",
        GovernmentId = "55030101230",
        GovernmentIdType = GovernmentIdTypeDto.Pesel,
        JobType = JobTypeDto.SomeJobType,
    };

    [Fact]
    public async Task Request_is_valid()
    {
        var personalData = validPersonalData;

        var validationResult = await validator.ValidateAsync(personalData);
        
        validationResult.EnsureCorrectResult();
    }

    [Fact]
    public async Task FirstName_is_empty()
    {
        var personalData = validPersonalData;
        personalData.FirstName = "";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.FirstNameIsEmpty);
    }
    
    [Fact]
    public async Task FirstName_is_whitespaces()
    {
        var personalData = validPersonalData;
        personalData.FirstName = "   ";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.FirstNameIsEmpty);
    }
    
    [Fact]
    public async Task FirstName_is_too_long()
    {
        var personalData = validPersonalData;
        personalData.FirstName = new string('a', StringLengths.ShortString + 1);

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.FirstNameIsTooLong);
    }
    
    [Fact]
    public async Task LastName_is_empty()
    {
        var personalData = validPersonalData;
        personalData.LastName = "";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.LastNameIsEmpty);
    }
    
    [Fact]
    public async Task LastName_is_whitespaces()
    {
        var personalData = validPersonalData;
        personalData.LastName = "   ";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.LastNameIsEmpty);
    }
    
    [Fact]
    public async Task LastName_is_too_long()
    {
        var personalData = validPersonalData;
        personalData.LastName = new string('a', StringLengths.ShortString + 1);

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.LastNameIsTooLong);
    }
    
    [Fact]
    public async Task GovernmentId_is_empty()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsEmpty);
    }
    
    [Fact]
    public async Task GovernmentId_is_whitespaces()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "   ";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsEmpty);
    }
    
    [Fact]
    public async Task GovernmentId_is_too_long()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = new string('a', StringLengths.MediumString + 1);

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsTooLong);
    }
    
    [Fact]
    public async Task GovernmentId_is_invalid_pesel()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "invalid pesel";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid);
    }
    
    [Fact]
    public async Task GovernmentId_is_pesel_with_invalid_control_digit()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "55030101231";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid);
    }
    
    [Fact]
    public async Task GovernmentId_is_pesel_with_invalid_length()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "550301012311";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid);
    }
    
    [Fact]
    public async Task GovernmentId_is_pesel_with_invalid_month()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "55130101233";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid);
    }
    
    [Fact]
    public async Task GovernmentId_is_pesel_with_invalid_day()
    {
        var personalData = validPersonalData;
        personalData.GovernmentId = "55034101236";

        var validationResult = await validator.ValidateAsync(personalData);

        validationResult.EnsureCorrectError(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid);
    }
}
