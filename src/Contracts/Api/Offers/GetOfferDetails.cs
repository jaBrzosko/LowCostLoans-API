namespace Contracts.Api.Offers;

public class GetOfferDetails
{
    public Guid OfferId { get; set; }
    
    public static class ErrorCodes
    {
        public const int OfferDoesNotExist = 1;
        public const int ContractNotUploaded = 2;
    }
}