using Contracts.Api.Offers;
using FastEndpoints;
using FluentValidation;
using Services.ValidationExtensions;

namespace Services.Endpoints.Frontend.Offers;

public class GetOfferDetailsValidator: Validator<GetOfferDetails>
{
    public GetOfferDetailsValidator()
    {
        RuleFor(req => req.OfferId)
            .MustAsync()
    }
}