using Microsoft.AspNetCore.Http;

namespace Contracts.Api.Offers;

public class PostAcceptOffer
{
    public Guid OfferId { get; set; }
    public IFormFile Contract { get; set; }

    public static class ErrorCodes
    {
        public const int OfferDoesNotExist = 1;
    }
}
