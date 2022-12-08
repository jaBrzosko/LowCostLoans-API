namespace Contracts.Offers;

public enum OfferStatusTypeDto
{
    Made = 0, // it was generated
    Applied = 1, // user decided to go for this offer
    Accepted = 2, // bank employee accepted this loan
    Rejected = 3 // bank employee rejected this loan
}