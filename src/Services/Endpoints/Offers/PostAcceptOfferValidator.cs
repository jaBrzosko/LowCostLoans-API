using Contracts.Offers;
using FastEndpoints;
using FluentValidation;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferValidator: Validator<PostAcceptOffer>
{
    public PostAcceptOfferValidator()
    {
    }
}