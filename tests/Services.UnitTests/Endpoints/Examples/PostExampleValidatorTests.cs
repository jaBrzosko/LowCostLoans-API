using Contracts.Examples;
using FluentAssertions;
using Services.Data;
using Services.Endpoints.Examples;
using Xunit;

namespace Services.UnitTests.Endpoints.Examples;

public class PostExampleValidatorTests
{
    private readonly PostExampleValidator validator = new();

    [Fact]
    public async Task Request_is_valid()
    {
        var request = new PostExample { Name = "valid name" };

        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(true);
    }
    
    [Fact]
    public async Task Name_is_empty()
    {
        var request = new PostExample { Name = "" };

        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(false);
        validationResult.Errors.Select(vr => int.Parse(vr.ErrorCode))
            .Should().ContainEquivalentOf(PostExample.ErrorCodes.NameIsEmpty);
    }
    
    [Fact]
    public async Task Name_is_too_long()
    {
        var request = new PostExample { Name = new string('a', StringLengths.ShortString + 1) };

        var validationResult = await validator.ValidateAsync(request);

        validationResult.IsValid.Should().Be(false);
        validationResult.Errors.Select(vr => int.Parse(vr.ErrorCode))
            .Should().ContainEquivalentOf(PostExample.ErrorCodes.NameIsTooLong);
    }
}
