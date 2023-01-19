namespace Contracts.Frontend.Offers;

public class PostOfferDecide
{
    public Guid Id { get; set; }
    public bool AcceptOffer { get; set; }
    
    public static class ErrorCodes
    {
        public const int OfferDoesNotExist = 1;
    }
}