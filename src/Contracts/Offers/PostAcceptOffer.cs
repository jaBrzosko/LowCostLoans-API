using Microsoft.AspNetCore.Http;

namespace Contracts.Offers;

public class PostAcceptOffer
{
    public Guid OfferId { get; set; }
    public IFormFile Contract { get; set; }
}