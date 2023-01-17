using Contracts.Frontend.Employees;
using Domain.Employees;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Frontend.Employees;

public class PostRegisterValidator : Validator<PostRegister>
{
    public PostRegisterValidator()
    {
        RuleFor(req => req.UserName)
            .NotEmpty()
            .WithErrorCode(PostRegister.ErrorCodes.UserNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithErrorCode(PostRegister.ErrorCodes.UserNameIsTooLong)
            .MustAsync(IsUserNameAvailableAsync)
            .WithErrorCode(PostRegister.ErrorCodes.UserNameIsAlreadyTaken);

        RuleFor(req => req.Password)
            .MinimumLength(Employee.MinPasswordLength)
            .WithErrorCode(PostRegister.ErrorCodes.PasswordIsTooShort)
            .MaximumLength(Employee.MaxPasswordLength)
            .WithErrorCode(PostRegister.ErrorCodes.PasswordIsTooLong);
    }

    private Task<bool> IsUserNameAvailableAsync(string userName, CancellationToken cancellationToken)
    {
        return Resolve<CoreDbContext>()
            .Employees
            .AllAsync(e => e.UserName != userName, cancellationToken);
    }
}
