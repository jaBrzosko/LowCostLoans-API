using FluentValidation;

namespace Services.ValidationExtensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int errorCode)
    {
        return rule.WithErrorCode(errorCode.ToString());
    }
}
