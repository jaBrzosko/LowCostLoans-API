using FluentAssertions;
using FluentValidation.Results;

namespace Services.UnitTests.Helpers;

public static class ValidationHelper
{
    public static void EnsureCorrectResult(this ValidationResult validationResult)
    {
        validationResult.IsValid.Should().Be(true);
    }

    public static void EnsureCorrectError(this ValidationResult validationResult, int errorCode)
    {
        validationResult.IsValid.Should().Be(false);
        validationResult.Errors.Select(vr => int.Parse(vr.ErrorCode))
            .Should().ContainEquivalentOf(errorCode);
    }
}
