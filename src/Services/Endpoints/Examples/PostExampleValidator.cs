using Contracts.Examples;
using FastEndpoints;
using FluentValidation;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Examples;

public class PostExampleValidator : Validator<PostExample>
{
    public PostExampleValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithErrorCode(PostExample.ErrorCodes.NameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithErrorCode(PostExample.ErrorCodes.NameIsTooLong);
    }
}
